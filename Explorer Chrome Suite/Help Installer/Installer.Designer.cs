namespace Teraque.HelpInstaller
{

	using Microsoft.Win32;
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.IO;

	/// <summary>
	/// Installer for the Visual Studio Templates that are part of the Explorer Chrome Package.
	/// </summary>
	/// <copyright>Copyright © 2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	partial class Installer : System.Configuration.Install.Installer
	{

		/// <summary>
		/// A basic container for the components added to this installer.
		/// </summary>
		IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{

			// This will explicitly dispose of any components added by the designer.
			if (disposing && this.components != null)
				this.components.Dispose();

			// Allow the base class to finish cleaning up.
			base.Dispose(disposing);

		}

		/// <summary>
		/// Initialize the component container used by this installer.
		/// </summary>
		void InitializeComponent()
		{

			// This container keeps track of all the components that are added through the IDE.
			this.components = new Container();

		}

		/// <summary>
		/// Performs the installation.
		/// </summary>
		/// <param name="savedState">An IDictionary used to save information needed to perform a commit, rollback, or uninstall operation.</param>
		public override void Install(IDictionary savedState)
		{

			// The main idea here is to move the template files into the Visual Studio directories so they can be used by any user on a machine.  But due to a bug
			// in the way the parameters are expanded, the '[TARGETDIR]' variable must have a '\' character at the end.  Otherwise it will generate strange messages
			// that have nothing to do with the installer.  This will remove the terminator character that was forced on us as a workaround.
			String installerTargetDirectory = (this.Context.Parameters["targetDir"] as String).TrimEnd('\\');
			if (installerTargetDirectory == null)
				return;

			// Install the Visual Studio 2008 Help System.
			this.InstallVisualStudio2008Help(installerTargetDirectory);

			// Install the Visual Studio 2010 Help System.
			this.InstallVisualStudio2010Help(installerTargetDirectory);

		}

		/// <summary>
		/// Installs the Visual Studio 2008 Help.
		/// </summary>
		/// <param name="visualStudioDirectory">The directory where the development environment resides.</param>
		void InstallVisualStudio2008Help(String installDirectory)
		{

			try
			{

				// This will set up the process launch variables to install the help files.  It is important to provide absolute paths to the help installer as it 
				// can get confused otherwise.
				String workingDirectory = Path.Combine(installDirectory, "Help");
				String fileName = Path.Combine(workingDirectory, "H2Reg.exe");
				String arguments = "-q -r";

				// Once the templates have been added or deleted the cache needs to be updated.  This process will take a long time, so we'll start it here and let 
				// it finish in its own sweet time.  It's not an ideal solution but it does guarantee that all users will be able to access the templates.  Note
				// that we run the process as an administrator.
				if (File.Exists(fileName))
				{
					Process process = new Process();
					process.StartInfo.Arguments = arguments;
					process.StartInfo.WorkingDirectory = workingDirectory;
					process.StartInfo.CreateNoWindow = true;
					process.StartInfo.FileName = fileName;
					process.StartInfo.RedirectStandardOutput = true;
					process.StartInfo.UseShellExecute = false;
					process.StartInfo.Verb = "runas";
					process.Start();
				}

			}
			catch (Exception exception)
			{

				// Any errors will be logged but won't interrupt the installation.
				EventLog.WriteEntry("Explorer Help Installer", exception.Message, EventLogEntryType.Error);

			}

		}

		/// <summary>
		/// Installs the Visual Studio 2010 Help.
		/// </summary>
		/// <param name="visualStudioDirectory">The directory where the development environment resides.</param>
		void InstallVisualStudio2010Help(String installDirectory)
		{

			try
			{

				// This will set up the process launch variables to install the help files.  It is important to provide absolute paths to the help installer as it 
				// can get confused otherwise.
				String workingDirectory = Path.Combine(installDirectory, "Help");
				String fileName = Path.Combine(workingDirectory, "HelpLibraryManagerLauncher.exe");
				String brandingPackage = Path.Combine(workingDirectory, "Dev10.mshc");
				String sourceMedia = Path.Combine(workingDirectory, "Documentation.msha");
				String arguments = String.Format(
					"/product \"VS\" /silent /version \"100\" /silent /locale en-us /brandingPackage \"{0}\" /sourceMedia \"{1}\"",
					brandingPackage,
					sourceMedia);

				// Once the templates have been added or deleted the cache needs to be updated.  This process will take a long time, so we'll start it here and let 
				// it finish in its own sweet time.  It's not an ideal solution but it does guarantee that all users will be able to access the templates.  Note
				// that we run the process as an administrator.
				Process process = new Process();
				process.StartInfo.Arguments = arguments;
				process.StartInfo.WorkingDirectory = workingDirectory;
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.FileName = fileName;
				process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.Verb = "runas";
				process.Start();

			}
			catch (Exception exception)
			{

				// Any errors will be logged but won't interrupt the installation.
				EventLog.WriteEntry("Explorer Help Installer", exception.Message, EventLogEntryType.Error);

			}

		}

		/// <summary>
		/// Removes an installation.
		/// </summary>
		/// <param name="iDictionary">An IDictionary that contains the state of the computer after the installation was complete.</param>
		public override void Uninstall(IDictionary savedState)
		{

			// The main idea here is to m ve the template files into the Visual Studio directories so they can be used by any user on a machine.  But due to a bug
			// in the way the parameters are expanded, the '[TARGETDIR]' variable must have a '\' character at the end.  Otherwise it will generate strange messages
			// that have nothing to do with the installer.  This will remove the terminator character that was forced on us as a workaround.
			String installerTargetDirectory = (this.Context.Parameters["targetDir"] as String).TrimEnd('\\');
			if (installerTargetDirectory == null)
				return;

			// Remove the Visual Studio 2008 Help System.
			this.UninstallVisualStudio2008Help(installerTargetDirectory);

			// Remove the Visual Studio 2010 Help System.
			this.UninstallVisualStudio2010Help(installerTargetDirectory);

		}

		/// <summary>
		/// Installs the Visual Studio 2008 Help.
		/// </summary>
		/// <param name="visualStudioDirectory">The directory where the development environment resides.</param>
		void UninstallVisualStudio2008Help(String installDirectory)
		{

			try
			{

				// This will set up the process launch variables to install the help files.  It is important to provide absolute paths to the help installer as it can 
				// get confused otherwise.
				String workingDirectory = Path.Combine(installDirectory, "Help");
				String fileName = Path.Combine(workingDirectory, "H2Reg.exe");
				String arguments = "-q -u";

				// Once the templates have been added or deleted the cache needs to be updated.  This process will take a long time, so we'll start it here and let it
				// finish in its own sweet time.  It's not an ideal solution but it does guarantee that all users will be able to access the templates.  Note that we
				// run the process as an administrator.
				if (File.Exists(fileName))
				{
					Process process = new Process();
					process.StartInfo.Arguments = arguments;
					process.StartInfo.WorkingDirectory = workingDirectory;
					process.StartInfo.CreateNoWindow = true;
					process.StartInfo.FileName = fileName;
					process.StartInfo.RedirectStandardOutput = true;
					process.StartInfo.UseShellExecute = false;
					process.StartInfo.Verb = "runas";
					process.Start();

					// The uninstaller requires files in the installation directory to be in place when it removes the help files.  Specifically the 'H2Reg.ini' 
					// file must still be there.  Unfortunately, the uninstaller will proceed to delete these files if this process is allowed to run in parallel.  
					// We must wait for the process to complete before proceeding otherwise it will cause errors with the H2Reg.exe execution.
					process.WaitForExit();

				}

			}
			catch (Exception exception)
			{

				// Any errors will be logged but won't interrupt the installation.
				EventLog.WriteEntry("Explorer Help Installer", exception.Message, EventLogEntryType.Error);

			}

		}

		/// <summary>
		/// Installs the Visual Studio 2010 Help.
		/// </summary>
		/// <param name="visualStudioDirectory">The directory where the development environment resides.</param>
		void UninstallVisualStudio2010Help(String installDirectory)
		{

			try
			{

				// This will set up the process launch variables to install the help files.  It is important to provide absolute paths to the help installer as it can 
				// get confused otherwise.
				String workingDirectory = Path.Combine(installDirectory, "Help");
				String fileName = Path.Combine(workingDirectory, "HelpLibraryManagerLauncher.exe");
				String arguments = "/product \"VS\" /version \"100\" /locale en-us /uninstall /silent /vendor \"Teraque, Inc.\"" +
					" /mediaBookList \"Explorer Chrome Documentation\" /productName \"Explorer Chrome Documentation\"";

				// Once the templates have been added or deleted the cache needs to be updated.  This process will take a long time, so we'll start it here and let it
				// finish in its own sweet time.  It's not an ideal solution but it does guarantee that all users will be able to access the templates.  Note that we
				// run the process as an administrator.
				if (File.Exists(fileName))
				{
					Process process = new Process();
					process.StartInfo.Arguments = arguments;
					process.StartInfo.WorkingDirectory = workingDirectory;
					process.StartInfo.CreateNoWindow = true;
					process.StartInfo.FileName = fileName;
					process.StartInfo.RedirectStandardOutput = true;
					process.StartInfo.UseShellExecute = false;
					process.StartInfo.Verb = "runas";
					process.Start();
				}

			}
			catch (Exception exception)
			{

				// Any errors will be logged but won't interrupt the installation.
				EventLog.WriteEntry("Explorer Help Installer", exception.Message, EventLogEntryType.Error);

			}

		}

	}

}