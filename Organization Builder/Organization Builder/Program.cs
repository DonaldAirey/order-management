namespace Teraque
{

	using System;
	using System.Collections.Generic;
	using System.Xml.Linq;
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
            {"-i", ArgumentState.InputFileParam},
			{"-out", ArgumentState.OutputFileParam}
        };

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static Int32 Main(String[] args)
		{

			// These are the parameters that are parsed out of the command line.
			String inputFilePath = String.Empty;
			String outputFilePath = String.Empty;

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

					case ArgumentState.InputFileParam:

						// The next command line argument will be the input file name.
						argumentState = ArgumentState.InputFileName;
						break;

					case ArgumentState.InputFileName:

						// Expand the environment variables so that paths don't need to be absolute.
						inputFilePath = Environment.ExpandEnvironmentVariables(argument);
						argumentState = ArgumentState.InputFileParam;
						break;

					case ArgumentState.OutputFileParam:

						// The next command line argument will be the input file name.
						argumentState = ArgumentState.OutputFileName;
						break;

					case ArgumentState.OutputFileName:

						// Expand the environment variables so that paths don't need to be absolute.
						outputFilePath = Environment.ExpandEnvironmentVariables(argument);
						break;

					default:

						// The parser will revert back to looking for an input file when it doesn't recognized the switch.
						argumentState = ArgumentState.InputFileName;
						break;

					}

				}

				// Throw a usage message back at the user if no file name was given.
				if (inputFilePath == null)
					throw new Exception("Usage: \"Organization Builder\" [-b <Batch Size>] [-f] -i <FileName>");

				// This will convert the organization description into a script that can be loaded via the Script Loader.
				XDocument organizationScript = Organization.Create(XDocument.Load(inputFilePath));
				organizationScript.Save(outputFilePath);

			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.Message);
			}

			// If we reached here, the file was imported without issue.
			return 0;

		}

	}

}
