namespace Teraque.AssetNetwork.Client
{

	using System;
	using System.Collections.Generic;
	using Teraque;

	/// <summary>
	/// Tool to load data into the data model.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class Program
	{

		/// <summary>
		/// Dictionary of command line parameter switches and the states they invoke in the parser.
		/// </summary>
		private static Dictionary<String, ArgumentState> argumentStates = new Dictionary<String, ArgumentState>()
        {
            {"-f", ArgumentState.ForceLoginParam},
            {"-i", ArgumentState.InputFileParam},
			{"-v", ArgumentState.VerbosityParam}
        };

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static Int32 Main(String[] args)
		{

			// This object is used to load the script and keeps track of loading statistics.
			ScriptLoader scriptLoader = new ScriptLoader();

			// The endpoint is pulled from the application settings first.  It can be overridden with the command line.
			scriptLoader.Endpoint = Properties.Settings.Default.DataModelEndpoint;

			// These are the parameters that are parsed out of the command line.
			Boolean forceLogin = false;
			String inputFilePath = String.Empty;
			Verbosity verbosity = Verbosity.Minimal;

			try
			{

				// The command line parser is driven by different states that are triggered by the flags read.  Unless a flag has been parsed, the command line
				// parser assumes that it's reading the file name from the command line.
				ArgumentState argumentState = ArgumentState.InputFileName;

				// Parse the command line for arguments.
				foreach (String argument in args)
				{

					// Use the dictionary to transition from one state to the next based on the input parameters.
					ArgumentState nextArgumentState;
					if (Program.argumentStates.TryGetValue(argument, out nextArgumentState))
						argumentState = nextArgumentState;

					// The parsing state will determine which variable is read next.
					switch (argumentState)
					{

					case ArgumentState.EndpointParam:

						// The next command line argument will be endpoint.
						argumentState = ArgumentState.Endpoint;
						break;

					case ArgumentState.Endpoint:

						// This will set the endpoint to use when communicating with the service.
						scriptLoader.Endpoint = argument;
						break;

					case ArgumentState.ForceLoginParam:

						// This will cause the script loader to prompt for login credentials.
						forceLogin = true;
						argumentState = ArgumentState.InputFileName;
						break;

					case ArgumentState.InputFileParam:

						// The next command line argument will be the input file name.
						argumentState = ArgumentState.InputFileName;
						break;
		
					case ArgumentState.InputFileName:

						// Expand the environment variables so that paths don't need to be absolute.
						inputFilePath = Environment.ExpandEnvironmentVariables(argument);
						break;

					case ArgumentState.VerbosityParam:

						// Verbose output.
						argumentState = ArgumentState.Verbosity;
						break;

					case ArgumentState.Verbosity:

						try
						{
							// Make sure any parsing errors are ignored.
							verbosity = (Verbosity)Enum.Parse(typeof(Verbosity), argument);
						}
						catch {	}

						break;

					default:

						// The parser will revert back to looking for an input file when it doesn't recognized the switch.
						argumentState = ArgumentState.InputFileName;
						break;

					}

				}

				// Throw a usage message back at the user if no file name was given.
				if (inputFilePath == null)
					throw new Exception("Usage: \"Script Loader\" [-b <Batch Size>] [-f] -i <FileName>");

				// Expand the environment variables and load the resulting file.
				scriptLoader.ForceLogin = forceLogin;
				scriptLoader.FileName = inputFilePath;
				scriptLoader.Verbosity = verbosity;
				scriptLoader.Load();

			}
			catch (Exception exception)
			{
                // Dump the error.
                Console.WriteLine(exception.Message);

                // This will force an abnormal exit from the program.
                scriptLoader.HasErrors = true;

			}

			// Print the final status of the load.
			Console.WriteLine(String.Format("{0} {1}: {2} Methods Executed", DateTime.Now.ToString("u"), scriptLoader.ScriptName, scriptLoader.MethodCount));

			// If an error happened anywhere, don't exit normally.
			if (scriptLoader.HasErrors)
				return 1;

			// After a successful load, the properties (specifically user name and password) can be saved for the next invocation of this application.
			Properties.Settings.Default.Save();

			// If we reached here, the file was imported without issue.
			return 0;

		}

	}

}
