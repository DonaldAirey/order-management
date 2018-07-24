namespace Teraque.AssetNetwork.Client
{

	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Globalization;
	using System.IO;
	using System.Reflection;
	using System.ServiceModel;
	using System.ServiceModel.Security;
	using System.Transactions;
	using System.Xml.Linq;
	using Teraque;

	/// <summary>
	/// Runs a file-based script of operations against the data model.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class ScriptLoader
	{

		/// <summary>
		/// Element types to describe values used by parameters.
		/// </summary>
		enum parameterElementType { Import, Load, None, Value };

		/// <summary>
		/// Maps the symbolic name of the channel to an actual communication channel to the data model.
		/// </summary>
		Dictionary<String, ClientChannel> clientTable;

		/// <summary>
		/// Default amount of time (in seconds) between ticks when the progress indicator is giving feedback.
		/// </summary>
		const Int32 defaultTickTime = 10;

		/// <summary>
		/// The endpoint to use for communications.
		/// </summary>
		public String Endpoint { get; set; }

		/// <summary>
		/// The name of the file loaded.
		/// </summary>
		public String FileName { get; set; }

		/// <summary>
		/// Prompts the user with login credentials before running the script.
		/// </summary>
		public Boolean ForceLogin { get; set; }

		/// <summary>
		/// Indicates that the script had errors.
		/// </summary>
		public Boolean HasErrors { get; set; }

		/// <summary>
		/// The number of method executed.
		/// </summary>
		public Int32 MethodCount { get; set; }

		/// <summary>
		/// Used to provide a custom output device for the progress messages.
		/// </summary>
		public event EventHandler<MessageEventArgs> NotifyMessage;

		/// <summary>
		/// Dictionary mapping element XNames to the type of element.
		/// </summary>
		static Dictionary<XName, parameterElementType> parameterElementDictionary = new Dictionary<XName, parameterElementType>()
		{
			{"load", parameterElementType.Load},
			{"import", parameterElementType.Import},
			{"value", parameterElementType.Value}
		};

		/// <summary>
		/// The user-friendly name of the script.
		/// </summary>
		public String ScriptName;

		/// <summary>
		/// The amount of time between ticks when the progress indicator is running.
		/// </summary>
		public TimeSpan TickTime { get; set; }

		/// <summary>
		/// Gets or sets the setting for how much progress information is emitted during the processing of the scripts.
		/// </summary>
		public Verbosity Verbosity { get; set; }

		/// <summary>
		/// A stack of the transactions.
		/// </summary>
		Stack<TransactionScope> tranactionScopeStack = new Stack<TransactionScope>();

		/// <summary>
		/// Creates an object that runs a script against a middle tier.
		/// </summary>
		public ScriptLoader()
		{

			// This table is used to map the client names to a channel object.
			this.clientTable = new Dictionary<String, ClientChannel>();

			// Users can be forced to specify the connection information even when preferences have been set by a previous session.  The default is to not force the
			// login.
			this.ForceLogin = false;

			// This is used to print the statistics at the end of the execution.
			this.MethodCount = 0;

			// This default message handler will route messages to the console.  This can be overridden by an application to send the messages to an event log, to 
			// the screen or any other device depending on the application.
			this.NotifyMessage += ScriptLoader.ConsoleMessageHandler;

			// This is the amount of time that will pass before issuing a 'Tick' event.
			this.TickTime = TimeSpan.FromSeconds(ScriptLoader.defaultTickTime);

			// Minimal output by default.
			this.Verbosity = Verbosity.Minimal;

		}

		/// <summary>
		/// Default message handler.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="messageEventArgs">The event arguments.</param>
		static void ConsoleMessageHandler(Object sender, MessageEventArgs messageEventArgs)
		{

			// Ticks are not automatically given a new line.  This allows us to print out the default progress of periods '.' to show.  All other messages are 
			// emitted to the console with a newline.
			if (messageEventArgs.IsProgressTick)
				Console.Write(messageEventArgs.Message);
			else
				Console.WriteLine(messageEventArgs.Message);

		}

		/// <summary>
		/// Converts an XElement describing a value to a native CLR value.
		/// </summary>
		/// <param name="type">The target datatype.</param>
		/// <param name="parameterElement">An XElement containing a String representation of a value.</param>
		/// <param name="directoryName">The directory where external files can be found.</param>
		/// <returns>The native CLR value of the described value.</returns>
		static Object ConvertElement(Type type, XElement parameterElement, String directoryName)
		{

			// If the target parameter is an array, then construct a vector parameter.
			if (type == typeof(Object[]))
			{

				// The values can be found in a single attribute of the 'parameter' element or be listed as children.  This list collects both methods of describing
				// values and constructs a single array when all elements and attributes are parsed.
				List<Object> valueList = new List<Object>();

				// An attribute can be used to desribe a value.  An optional 'Type' attribute can specify what type of conversion is used to evaluate the CLR value.
				XAttribute valueAttribute = parameterElement.Attribute("value");
				if (valueAttribute != null)
					valueList.Add(ScriptLoader.ConvertValue(ScriptLoader.GetElementType(typeof(String), parameterElement), valueAttribute.Value));

				// It is possible to specify the value using the content of an XML element or through an "import" statement.  This will cycle through any nodes of
				// the parameter looking for additional nodes containing the data for the parameter.
				foreach (XObject xObject in parameterElement.Nodes())
				{

					// This uses the element content as the value for the parameter.
					if (xObject is XText)
					{
						XText xText = xObject as XText;
						valueList.Add(ConvertValue(typeof(String), xText.Value));
					}

					// Elements can be nested inside the parameter element to greater detail to the parameter.
					if (xObject is XElement)
					{

						// This element holds special instructions for the parameter.
						XElement xElement = xObject as XElement;

						// Values for a key can be specified as child elements of the parameter.
						if (xElement.Name == "value")
							valueList.Add(ConvertValue(GetElementType(typeof(String), xElement), xElement.Value));

						// This special instruction allows the value of a parameter to come from an external file.  This is used primary to load XML content into a
						// record.
						if (xElement.Name == "import")
						{
							XAttribute xAttribute = xElement.Attribute("path");
							String path = Path.IsPathRooted(xAttribute.Value) ? xAttribute.Value : Path.Combine(directoryName, xAttribute.Value);
							XDocument xDocument = XDocument.Load(path);
							valueList.Add(ConvertValue(type, xDocument.ToString()));
						}

						// A 'load' element will read a binary resource into a byte array.
						if (xElement.Name == "load")
						{
							XAttribute xAttribute = xElement.Attribute("path");
							String path = Path.IsPathRooted(xAttribute.Value) ? xAttribute.Value : Path.Combine(directoryName, xAttribute.Value);
							Byte[] binaryData;
							using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
							{
								binaryData = new Byte[fileStream.Length];
								fileStream.Read(binaryData, 0, Convert.ToInt32(fileStream.Length));
							}
							return ConvertValue(type, Convert.ToBase64String(binaryData));
						}

					}

				}

				// This array is most often used as a key to find a record in a table.
				return valueList.ToArray();

			}
			else
			{

				// If the XML specifies an override for the native data type of the parameter, make sure that the new value's type is compatible with the parameter.
				Type originalType = type;
				type = ScriptLoader.GetElementType(type, parameterElement);
				if (type != originalType && !type.IsSubclassOf(originalType))
					throw new Exception(String.Format("Can't cast a parameter of type {0} to {1}.", type, originalType));

				// It is possible to specify the value using the content of an XML element or through an "import" statement.  This will cycle through any nodes of
				// the parameter looking for additional nodes containing the data for the parameter.  Of course, for a scalar, there is only a single value that
				// can be returned, so we'll take the first element we find that converts into a value.
				foreach (XObject xObject in parameterElement.DescendantNodes())
					return ScriptLoader.ParseValue(type, xObject, directoryName);

				// If no elements are specified, then we'll simply convert the attribute into a value for this parameter.  This is by far the most common method of
				// providing values for parameter.
				XAttribute valueAttribute = parameterElement.Attribute("value");
				return valueAttribute == null ? DBNull.Value : ScriptLoader.ConvertValue(type, valueAttribute.Value);

			}

		}

		/// <summary>
		/// Convert text to a CLR object based on the target datatype.
		/// </summary>
		/// <param name="type">The target datatype.</param>
		/// <param name="value">The String representation of the value.</param>
		/// <returns>The CLR equivalent of the given value.</returns>
		static Object ConvertValue(Type type, String value)
		{

			// Use the destination type to drive the creation of a statement that will convert text into a CLR value.
			switch (type.ToString())
			{

			case "System.Object":
				return value;

			case "System.Boolean":
				return Convert.ToBoolean(value);

			case "System.Int16":
				return Convert.ToInt16(value);

			case "System.Int32":
				return Convert.ToInt32(value);

			case "System.Int64":
				return Convert.ToInt64(value);

			case "System.Decimal":
				return Convert.ToDecimal(value);

			case "System.Double":
				return Convert.ToDouble(value);

			case "System.DateTime":
				return Convert.ToDateTime(value);

			case "System.String":
				return value;

			case "System.Guid":
				return new Guid(value);

			case "System.Byte[]":
				return Convert.FromBase64String(value);

			default:
				if (type.IsEnum)
					return Enum.Parse(type, value);
				break;

			}

			// Throw the exception to catch any data types that aren't converted above.
			throw new Exception(String.Format("There is no conversion expression that can be created for a {0} type.", type));

		}

		/// <summary>
		/// Convert text to a CLR object based on the target datatype.
		/// </summary>
		/// <param name="type">The target datatype.</param>
		/// <param name="element">The xml element.</param>
		/// <returns>The CLR equivalent of the given value.</returns>
		static Object ConvertValue(Type type, XElement element)
		{

			if (element.IsEmpty)
				return DBNull.Value;
			else
				return ScriptLoader.ConvertValue(type, element.Value);

		}

		/// <summary>
		/// Creates a channel to communicate to the data model.
		/// </summary>
		/// <param name="clientElement">An XML description of the channel.</param>
		void CreateClient(XElement clientElement)
		{

			// This will extract the name of the client from the XML specification.  A script can have one or more channels defined to the data model.  The client
			// name allows the XML for methods to specify which channel to use.  If no name is given, a default name (of an emtpy string) is assumed.
			XAttribute nameAttribute = clientElement.Attribute("name");
			String clientName = nameAttribute == null ? String.Empty : nameAttribute.Value;

			// This will extract the Managed type information for the chanel.
			XAttribute typeAttribute = clientElement.Attribute("type");
			String typeDescription = typeAttribute == null ? String.Empty : typeAttribute.Value;

			// Use reflection to pull the 'Type' attribute apart to find the assembly and type where the client channel is declared.  This shreds the attribute for
			// the values needed by reflection to find the assembly and type information dynamically.
			String[] parts = typeDescription.Split(',');
			String typeName = parts[0];
			String assemblyString = String.Empty;
			for (Int32 index = 1; index < parts.Length; index++)
				assemblyString += (assemblyString == String.Empty ? String.Empty : ",") + parts[index];
			Assembly assembly = Assembly.Load(assemblyString);
			Type channelType = assembly.GetType(typeName);
			if (channelType == null)
				throw new Exception(String.Format("The specified client {0} in assembly {1} can't be located", typeName, assemblyString));

			// This does the actual work of creating a channel from the type information found in the script statement.  Note that the endpoint is specified either
			// in the application settings or on the command line and fed into the initializer of the channel here.
			Object channelObject = assembly.CreateInstance(
				typeName,
				false,
				BindingFlags.CreateInstance,
				null,
				new Object[] { this.Endpoint },
				CultureInfo.InvariantCulture,
				null);
			if (channelObject == null)
				throw new Exception(String.Format("Can't create a channel of type '{0}'.", typeName));

			// Each 'method' element contains a symbolic reference to the client that is to be used for that method's execution.  This constructs a table that
			// decodes the symbolic link into a channel used to execute the method.
			this.clientTable.Add(clientName, new ClientChannel(channelObject, channelType));

		}

		/// <summary>
		/// Process a transaction.
		/// </summary>
		/// <param name="xElement">The node containing the transaction.</param>
		void ExecuteTransaction(XElement xElement)
		{

			// Create an explicit required transaction for the methods found at this node that will wait 10 minutes before timing out.
			using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(10)))
			{

				// Transactions can be nested.  When they are nested, the transaction will only complete when the most outer layer has completed.
				this.tranactionScopeStack.Push(transactionScope);

				// These variables are used to count the number of successful methods.  If a transaction fails, then all the methods in the transaction have
				// failed.  The total count of methods executed by the class does not reflect the failed methods in the failed transaction.
				bool isSuccessful = true;
				int methodCount = 0;

				// This will execute each of the methods in the transaction.  If a single method fails, then all the methods, even the successful ones, will be
				// rolled back.
				foreach (XElement childElement in xElement.Elements())
				{
					// The element name defines how the node is handled.
					switch (childElement.Name.LocalName)
					{
					case "client":

						// This will create an endpoint to connect to the data model.
						this.CreateClient(childElement);
						break;

					case "method":

						// Execute a single method (without a transaction).
						if (this.ExecuteMethod(childElement))
							methodCount++;
						else
							isSuccessful = false;

						break;

					case "transaction":

						// This will create the child elements of this node as a single unit.
						this.ExecuteTransaction(childElement);
						break;

					}

				}

				// At this point, all the methods were successful and the transaction can be committed and the global counter of good methods reflects the
				// successes.  If we don't call the 'Complete' method, then the transaction will be rolled back implicity when this scope exits.
				if (isSuccessful)
				{
					this.MethodCount += methodCount;
					transactionScope.Complete();
				}

				// This will restore the previous transaction scope when all the elements of this scope have been evaluated.
				this.tranactionScopeStack.Pop();

			}

		}

		/// <summary>
		/// Creates a method plan from the parameters listed.
		/// </summary>
		/// <param name="methodElement">An XML node where the method and parameters are found.</param>
		bool ExecuteMethod(XElement methodElement)
		{

			// Indicates the success or failure of an individual method execution.
			bool isSuccessful = false;

			try
			{

				// The client channel on which this method is to be executed is described by the 'client' attribute.
				XAttribute clientAttribte = methodElement.Attribute("client");
				String clientName = clientAttribte == null ? String.Empty : clientAttribte.Value;
				ClientChannel client;
				if (!this.clientTable.TryGetValue(clientName, out client))
					throw new Exception(String.Format("The client {0} hasn't been defined", clientName == String.Empty ? "<default>" : clientName));

				// Reflection is used here to find the method to be executed.
				XAttribute methodNameAttribute = methodElement.Attribute("name");
				String methodName = methodNameAttribute == null ? String.Empty : methodNameAttribute.Value;
				MethodInfo methodInfo = client.ChannelType.GetMethod(methodNameAttribute.Value);
				if (methodInfo == null)
					throw new Exception(String.Format("The method {0} isn't part of the library", methodNameAttribute.Value));

				// This will pull apart the XML that contains the parameters and construct a dictionary of method arguments.
				Dictionary<String, XElement> parameterDictionary = new Dictionary<String, XElement>();
				foreach (XElement parameterElement in methodElement.Elements("parameter"))
				{
					XAttribute parameterNameAttribute = parameterElement.Attribute("name");
					parameterDictionary.Add(parameterNameAttribute.Value, parameterElement);
				}

				// This will correlate the parameter in the XML with the parameter of the actual method found through reflection and convert the parameter into the
				// proper destination type.  The end result is a parameter array that is compatible with a reflection call to the method specified in the XML.
				ParameterInfo[] parameterInfoArray = methodInfo.GetParameters();
				Object[] parameterArray = new Object[parameterInfoArray.Length];
				for (int index = 0; index < parameterInfoArray.Length; index++)
				{
					ParameterInfo parameterInfo = parameterInfoArray[index];
					XElement parameterElement;
					if (parameterDictionary.TryGetValue(parameterInfo.Name, out parameterElement))
						parameterArray[index] = ScriptLoader.ConvertElement(parameterInfo.ParameterType, parameterElement, Path.GetDirectoryName(this.FileName));
				}

				try
				{

					// At this point, the only thing left to do is call the method using the parsed parameters.
					methodInfo.Invoke(client.ChannelObject, parameterArray);

				}
				catch (TargetInvocationException targetInvocationException)
				{

					// We use reflection to execute the method, so naturally we're going to get a reflection error.  This will dig out the real reason that this 
					// method failed and throw it.
					throw targetInvocationException.InnerException;

				}

				// The method invocation was successful at this point.
				isSuccessful = true;

			}
			catch (FaultException<IndexNotFoundFault> indexNotFoundException)
			{

				// The record wasn't found.
				Console.WriteLine(Teraque.Properties.Resources.IndexNotFoundError, indexNotFoundException.Detail.IndexName,
					indexNotFoundException.Detail.TableName);

			}
			catch (FaultException<RecordNotFoundFault> recordNotFoundException)
			{

				// The record wasn't found.
				Console.WriteLine(Teraque.Properties.Resources.RecordNotFoundError,
					CommonConversion.FromArray(recordNotFoundException.Detail.Key),
					recordNotFoundException.Detail.TableName);

			}
			catch (FaultException<ConstraintFault> constraintFaultException)
			{

				// The arguments weren't in the proper range.
				Console.WriteLine(constraintFaultException.Detail.Message);

			}
			catch (FaultException<ArgumentFault> argumentFaultException)
			{

				// The arguments weren't in the proper range.
				Console.WriteLine(argumentFaultException.Detail.Message);

			}
			catch (FaultException<FormatFault> formatFaultException)
			{

				// The arguments weren't in the proper range.
				Console.WriteLine(formatFaultException.Detail.Message);

			}
			catch (FaultException<ExceptionDetail> exceptionDetail)
			{

				// This is a general purpose exception for debugging.
				Console.WriteLine(exceptionDetail.Message);

			}

			// This is the final indication of whether the method was successful or not.
			return isSuccessful;

		}

		/// <summary>
		/// Extracts the data type from an XElement.
		/// </summary>
		/// <param name="originalType">The original element type.</param>
		/// <param name="xElement">The element describing the new type.</param>
		/// <returns>Searches all the loaded assemblies for the type declared in the 'xElement' parameter.</returns>
		static Type GetElementType(Type originalType, XElement xElement)
		{

			// All the currently loaded assemblies are searched for a datatype described by the 'dataType' attribute.
			XAttribute dataTypeAttribute = xElement.Attribute("type");
			if (dataTypeAttribute != null)
				foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
				{
					Type type = assembly.GetType(dataTypeAttribute.Value);
					if (type != null)
						return type;
				}

			// If there is no override the use the original type.
			return originalType;

		}

		/// <summary>
		/// Load the XML script.
		/// </summary>
		[SuppressMessage("Microsoft.Usage", "CA2200:RethrowToPreserveStackDetails")]
		public void Load()
		{

			// This flag is set when an error occurs anywhere in the processing of the XML file.
			this.HasErrors = false;

			// This will display the name of the loaded file when verbose output is requested.
			if (this.NotifyMessage != null && this.Verbosity == Verbosity.Verbose)
				this.NotifyMessage(this, new MessageEventArgs(String.Format("Loading {0}", FileName)));

            // Load the script.
            XDocument xDocument = null;
            try
            {
                xDocument = XDocument.Load(this.FileName);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return;
            }

			// The script name is stored in the root node.  The name is used in status and debugging messages.
			XAttribute nameAttribute = xDocument.Root.Attribute("name");
			this.ScriptName = nameAttribute == null ? "<unnamed>" : nameAttribute.Value;

			// This will prompt the user for credentials the next time a channel is opened.
			InteractiveChannelInitializer.IsPrompted = this.ForceLogin;

			// This is used for the tick indicator.  After each element in the XML file is sent, we will check the time.  When enough time has gone by, we'll emit 
			// another tick.
			DateTime startTime = DateTime.Now;

			// The script contains statements that are interpreted and executed.  Most of the statements are 'methods' which are CRUD operations that are executed
			// against the data model.  There are also statements that define a client channel for connecting to the server and statements that bind a group of
			// statements into a single transaction.
			foreach (XElement xElement in xDocument.Root.Elements())
			{

				// This provides progress verbose feedback when a predefine amount of time has passed.
				if (DateTime.Now.Subtract(startTime) > this.TickTime && this.Verbosity == Verbosity.Verbose)
				{
					if (this.NotifyMessage != null && this.Verbosity == Verbosity.Verbose)
						this.NotifyMessage(this, new MessageEventArgs(true, "."));
					startTime = DateTime.Now;
				}

				try
				{

					// The element name defines how the node is handled.
					switch (xElement.Name.LocalName)
					{
					case "client":

						// This will create an endpoint to connect to the data model.
						this.CreateClient(xElement);
						break;

					case "method":

						// Execute a single method (without a transaction).
						if (this.ExecuteMethod(xElement))
							this.MethodCount++;
						break;

					case "transaction":

						// This will create the child elements of this node as a single unit.
						this.ExecuteTransaction(xElement);
						break;

					}

				}
				catch (MessageSecurityException messageSecurityException)
				{

					// Extract the real reason this failed to run on the server and log it.
					Exception innerException = messageSecurityException.InnerException;
					Log.Error(String.Format("{0} {1}", innerException.Message, innerException.StackTrace));

					// Broadcast the real error to any listeners.
					if (this.NotifyMessage != null)
						this.NotifyMessage(this, new MessageEventArgs(innerException.Message));

					// A security exception is a terminal exception.  Throw it again to end the script.
					throw innerException;

				}
				catch (Exception exception)
				{

					// Log the error.
					Log.Error(String.Format("{0} {1}", exception.Message, exception.StackTrace));

					// Broadcast the error to any listeners.
					if (NotifyMessage != null)
						this.NotifyMessage(this, new MessageEventArgs(exception.Message));

					// Any errors caught at this level are terminal.  Throw the exception again to end the script.
					throw exception;

				}
			}

			// This will provide a newline character after the progress indicators above have moved horizontally across the console.  Any new messages will appear
			// on a separate line from the dots.
			if (this.NotifyMessage != null && this.Verbosity == Verbosity.Verbose)
				this.NotifyMessage(this, new MessageEventArgs(String.Empty));

			// One or more channels may have been opened for this script.  This will close them gracefully.
			foreach (KeyValuePair<String, ClientChannel> clientPair in this.clientTable)
			{
				MethodInfo closeMethod = clientPair.Value.ChannelType.GetMethod("Close");
				closeMethod.Invoke(clientPair.Value.ChannelObject, null);
			}

		}

		/// <summary>
		/// Parses a single CLR value out of an XML element.
		/// </summary>
		/// <param name="type">The destination type.</param>
		/// <param name="xObject">The XObject containing the value.</param>
		/// <param name="directoryName">The name of the directory where external files can be found.</param>
		/// <returns>The CLR value of the given XML node.</returns>
		static Object ParseValue(Type type, XObject xObject, String directoryName)
		{

			// This will evaluate a literal text object as the value.
			if (xObject is XText)
			{
				XText xText = xObject as XText;
				return ScriptLoader.ConvertValue(typeof(String), xText.Value);
			}

			// This will interpret an element as a value.
			if (xObject is XElement)
			{

				// This element holds special instructions for the parameter.
				XElement xElement = xObject as XElement;

				// Translate the element type into an Enum.
				parameterElementType parameterElementType = parameterElementType.None;
				if (!ScriptLoader.parameterElementDictionary.TryGetValue(xElement.Name, out parameterElementType))
					parameterElementType = parameterElementType.None;

				// Common variables used below.
				XAttribute pathAttribute;
				String path;

				// This will convert the element according to the kind of element it is.
				switch (parameterElementType)
				{

				case parameterElementType.Value:

					// This will convert a simple text value into the CLR equivalent.
					return ConvertValue(ScriptLoader.GetElementType(typeof(String), xElement), xElement);

				case parameterElementType.Load:

					// This will load a resource file as a Base64 string.
					pathAttribute = xElement.Attribute("path");
					path = Path.IsPathRooted(pathAttribute.Value) ? pathAttribute.Value : Path.Combine(directoryName, pathAttribute.Value);
					using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
					{
						Byte[] binaryData = new Byte[fileStream.Length];
						fileStream.Read(binaryData, 0, Convert.ToInt32(fileStream.Length));
						return ScriptLoader.ConvertValue(type, Convert.ToBase64String(binaryData));
					}

				case parameterElementType.Import:

					// This will load an XML file as a text string.
					pathAttribute = xElement.Attribute("path");
					path = Path.IsPathRooted(pathAttribute.Value) ? pathAttribute.Value : Path.Combine(directoryName, pathAttribute.Value);
					XDocument xDocument = XDocument.Load(path);
					return ScriptLoader.ConvertValue(type, xDocument.ToString());

				}

			}

			// Any node that can't be parsed is assumed to be a DBNull value.
			return DBNull.Value;

		}
		
	}

}
