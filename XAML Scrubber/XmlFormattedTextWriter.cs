namespace Teraque.Tools
{

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Text;
	using System.Xml;

	/// <summary>
	/// An XML text writer that beautifies the XML stream.
	/// </summary>
	public class XmlFormattedTextWriter : XmlWriter
	{

		/// <summary>
		/// A stream used to write the XML.
		/// </summary>
		private StreamWriter streamWriter;

		/// <summary>
		/// A stack of states used to track the current element that is being written.
		/// </summary>
		private Stack<Element> elementStack;

		/// <summary>
		/// A stack of states used to track the state of the writer as nodes are written.
		/// </summary>
		private Stack<WriteState> stateStack;

		/// <summary>
		/// The default size of a tab.
		/// </summary>
		private const Int32 defaultTabSize = 4;

		/// <summary>
		/// The tab size.
		/// </summary>
		public Int32 TabSize { get; set; }

		/// <summary>
		/// Creates a new instance of the Teraque.Tools.XmlFormattedTextWriter class.
		/// </summary>
		/// <param name="path">The complete file path to write to. path can be a file name.</param>
		public XmlFormattedTextWriter(String path)
		{

			// Initialize the object.
			this.TabSize = XmlFormattedTextWriter.defaultTabSize;
			this.streamWriter = new StreamWriter(path);
			this.elementStack = new Stack<Element>();
			this.stateStack = new Stack<WriteState>();

		}

		/// <summary>
		/// Gets a string that can be used for the current indentation level when writing to the XML stream.
		/// </summary>
		private String CurrentIndent
		{

			get
			{

				Int32 totalSpace;
				StringBuilder indentText = new StringBuilder();

				// The state determines how the visual alignment is handled.
				switch (this.WriteState)
				{

				case WriteState.Attribute:

					// This creates a string that will align all the attributes under the first attribute in an element.
					totalSpace = (this.elementStack.Count - 1) * this.TabSize + this.elementStack.Peek().QualifiedName.Length + 2;
					for (int tabCount = 0; tabCount < totalSpace / this.TabSize; tabCount++)
						indentText.Append("\t");
					for (int spaceCount = 0; spaceCount < totalSpace % this.TabSize; spaceCount++)
						indentText.Append(" ");

					break;

				case WriteState.Content:

					// This lines up the content of an element at the same outline level.
					totalSpace = this.elementStack.Count * this.TabSize;
					for (int tabCount = 0; tabCount < totalSpace / this.TabSize; tabCount++)
						indentText.Append("\t");
					break;

				}

				// This string can be used when writing the stream to visually align the nodes in an XML document.
				return indentText.ToString();

			}

		}

		/// <summary>
		/// Gets the state of the writer.
		/// </summary>
		public override WriteState WriteState
		{
			get { return this.stateStack.Peek(); }
		}

		/// <summary>
		/// Closes this stream and the underlying stream.
		/// </summary>
		public override void Close()
		{

			// Close the stream and flushes the contents.
			this.streamWriter.Close();

		}

		/// <summary>
		/// Flushes whatever is in the buffer to the underlying streams and also flushes the underlying stream.
		/// </summary>
		public override void Flush()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns the closest prefix defined in the current namespace scope for the namespace URI.
		/// </summary>
		/// <param name="ns">The namespace URI whose prefix you want to find.</param>
		/// <returns>The matching prefix or nullNothingnullptra null reference (Nothing in Visual Basic) if no matching namespace URI is found in the current
		/// scope.</returns>
		public override String LookupPrefix(String ns)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Encodes the specified binary bytes as Base64 and writes out the resulting text.
		/// </summary>
		/// <param name="buffer">Byte array to encode.</param>
		/// <param name="index">The position in the buffer indicating the start of the bytes to write.</param>
		/// <param name="count">The number of bytes to write.</param>
		public override void WriteBase64(byte[] buffer, int index, int count)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Writes out a &gt;![CDATA[...]]&lt; block containing the specified text.
		/// </summary>
		/// <param name="text">The text to place inside the CDATA block.</param>
		public override void WriteCData(String text)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Forces the generation of a character entity for the specified Unicode character value.
		/// </summary>
		/// <param name="ch">The Unicode character for which to generate a character entity.</param>
		public override void WriteCharEntity(char ch)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Writes text one buffer at a time.
		/// </summary>
		/// <param name="buffer">Character array containing the text to write.</param>
		/// <param name="index">The position in the buffer indicating the start of the text to write.</param>
		/// <param name="count">The number of characters to write.</param>
		public override void WriteChars(char[] buffer, int index, int count)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Writes out a comment <!--...--> containing the specified text.
		/// </summary>
		/// <param name="text">Text to place inside the comment.</param>
		public override void WriteComment(String text)
		{

			switch (this.WriteState)
			{

			case WriteState.Content:

				// If the writer has already started writing content, then align the comment under the siblings nodes in the output.
				this.streamWriter.WriteLine("{0}<!--{1}-->", this.CurrentIndent, text);
				break;

			case WriteState.Element:

				// If the writer hasn't stared writing content yet, then terminate the start element tag and align the comment under the siblings in the output 
				// stream.
				TerminateStartElement();
				this.streamWriter.WriteLine();
				this.streamWriter.WriteLine("{0}<!--{1}-->", this.CurrentIndent, text);
				break;

			}

		}

		/// <summary>
		/// Writes the DOCTYPE declaration with the specified name and optional attributes.
		/// </summary>
		/// <param name="name">The name of the DOCTYPE. This must be non-empty.</param>
		/// <param name="pubid">If non-null it also writes PUBLIC "pubid" "sysid" where pubid and sysid are replaced with the value of the given arguments.</param>
		/// <param name="sysid">If pubid is nullNothingnullptra null reference (Nothing in Visual Basic) and sysid is non-null it writes SYSTEM "sysid" where sysid is replaced
		/// with the value of this argument.</param>
		/// <param name="subset">If non-null it writes [subset] where subset is replaced with the value of this argument.</param>
		public override void WriteDocType(String name, String pubid, String sysid, String subset)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// closes the previous WriteStartAttribute call.
		/// </summary>
		public override void WriteEndAttribute()
		{

			// Return the writer to the previous state.
			this.stateStack.Pop();

		}

		/// <summary>
		/// Closes any open elements or attributes and puts the writer back in the Start state.
		/// </summary>
		public override void WriteEndDocument()
		{

			// Return the writer to the original state.
			this.stateStack.Pop();

			if (this.stateStack.Count != 0)
				//				throw new InvalidOperationException("The state stack was not balanced at the end of writing.");
				Console.WriteLine("The state stack was not balanced at the end of writing.");

		}

		/// <summary>
		/// Closes one element and pops the corresponding namespace scope.
		/// </summary>
		public override void WriteEndElement()
		{

			// Pop the element off the stack now that we're done writing it.
			Element element = this.elementStack.Pop();

			// This terminates the element in the output stream.
			this.streamWriter.WriteLine("/>");

			// Return the writer to the state it had before we wrote this element.
			this.stateStack.Pop();

		}

		/// <summary>
		/// writes out an entity reference as &name;.
		/// </summary>
		/// <param name="name">The name of the entity reference.</param>
		public override void WriteEntityRef(String name)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Closes one element and pops the corresponding namespace scope.
		/// </summary>
		public override void WriteFullEndElement()
		{

			// Pop the element off the stack now that we're done writing it.
			Element element = this.elementStack.Pop();

			// This element had content that couldn't be written in a single tag.  This writes the end tag to the XML output stream.
			if (element.ChildCount == 0)
				this.streamWriter.WriteLine("</{1}>", this.CurrentIndent, element.QualifiedName);
			else
				this.streamWriter.WriteLine("{0}</{1}>", this.CurrentIndent, element.QualifiedName);

			// Return the writer to the state it had before we wrote this element.
			this.stateStack.Pop();

		}

		/// <summary>
		/// Writes out a processing instruction with a space between the name and text as follows: &gt;?name text?&lt;.
		/// </summary>
		/// <param name="name">The name of the processing instruction.</param>
		/// <param name="text">The text to include in the processing instruction.</param>
		public override void WriteProcessingInstruction(String name, String text)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Writes raw markup manually.
		/// </summary>
		/// <param name="data">String containing the text to write.</param>
		public override void WriteRaw(String data)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Writes raw markup manually.
		/// </summary>
		/// <param name="buffer">Character array containing the text to write.</param>
		/// <param name="index">The position within the buffer indicating the start of the text to write.</param>
		/// <param name="count">The number of characters to write.</param>
		public override void WriteRaw(char[] buffer, int index, int count)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// writes the start of an attribute with the specified prefix, local name, and namespace URI.
		/// </summary>
		/// <param name="prefix">The namespace prefix of the attribute.</param>
		/// <param name="localName">The local name of the attribute.</param>
		/// <param name="namespaceUri">The namespace URI for the attribute.</param>
		public override void WriteStartAttribute(String prefix, String localName, String namespaceUri)
		{

			// This indicates that the writer is processing an attribute.
			this.stateStack.Push(WriteState.Attribute);

			// Create an attribute from the parts.
			Attribute attribute = new Attribute(localName, namespaceUri, prefix);

			// The attributes are aligned visually under the first attribute.
			if (this.elementStack.Peek().AttributeCount == 0)
			{
				this.streamWriter.Write(" {0}=", attribute.QualifiedName);
			}
			else
			{
				this.streamWriter.WriteLine();
				this.streamWriter.Write("{0}{1}=", CurrentIndent, attribute.QualifiedName);
			}

			// This keeps track of the number of elements written.  Currently only the first one is significant.
			this.elementStack.Peek().AttributeCount = this.elementStack.Peek().AttributeCount + 1;

		}

		/// <summary>
		/// Writes the XML declaration.
		/// </summary>
		/// <param name="standalone">If true, it writes "standalone=yes"; if false, it writes "standalone=no".</param>
		public override void WriteStartDocument(bool standalone)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Writes the XML declaration with the version "1.0" and the standalone attribute.
		/// </summary>
		public override void WriteStartDocument()
		{

			// This indicates the initial state of the write operation.
			this.stateStack.Push(WriteState.Start);

		}

		/// <summary>
		/// Writes the specified start tag and associates it with the given namespace and prefix.
		/// </summary>
		/// <param name="prefix">The namespace prefix of the element.</param>
		/// <param name="localName">The local name of the element.</param>
		/// <param name="ns">The namespace URI to associate with the element.</param>
		public override void WriteStartElement(String prefix, String localName, String ns)
		{

			// If the start of an element is already being written when the writer came across a child element, then the parent start tag must be terminated 
			// properly before the rest of the stream is processed.  Note that the state changes from processing an element start tag to processing the content.
			switch (this.WriteState)
			{
			case WriteState.Element:
				TerminateStartElement();
				this.streamWriter.WriteLine();
				break;
			}

			// This keeps track of the number of child elements.
			if (this.elementStack.Count != 0)
				this.elementStack.Peek().ChildCount = this.elementStack.Peek().ChildCount + 1;

			// Create a new element and write it out to the stream at the proper indentation level.
			Element element = new Element(localName, ns, prefix);
			this.streamWriter.Write("{0}<{1}", this.CurrentIndent, element.QualifiedName);

			// The element will be needed again when all the content has been processed.
			this.elementStack.Push(element);

			// This indicates that we're processing an element start tag.
			this.stateStack.Push(WriteState.Element);

		}

		/// <summary>
		/// Writes the given text content.
		/// </summary>
		/// <param name="text">The text to write.</param>
		public override void WriteString(String text)
		{

			// Different states have different tokens for writing the text.
			switch (this.WriteState)
			{
			case WriteState.Attribute:

				// Attribute text is written in quotes.
				this.streamWriter.Write("\"{0}\"", text);
				break;

			default:

				// Text inside of an element needs to terminate the "start element" tag.
				TerminateStartElement();
				this.streamWriter.Write("{0}", text);
				break;

			}

		}

		/// <summary>
		/// Generates and writes the surrogate character entity for the surrogate character pair.
		/// </summary>
		/// <param name="lowChar">The low surrogate. This must be a value between 0xDC00 and 0xDFFF.</param>
		/// <param name="highChar">The high surrogate. This must be a value between 0xD800 and 0xDBFF.</param>
		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Writes out the given white space.
		/// </summary>
		/// <param name="ws">The string of white space characters.</param>
		public override void WriteWhitespace(String ws)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Completes the processing of a start element.
		/// </summary>
		private void TerminateStartElement()
		{

			// Terminate the start element tag and indicate that the writer is now writing the content of the element.
			this.streamWriter.Write(">");
			this.stateStack.Pop();
			this.stateStack.Push(WriteState.Content);

		}

	}

}
