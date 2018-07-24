namespace Template_Installer
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

			// We are going to install the templates in the Visual Studio product directory.  However, the location of Visual Studio depends on a registry key.  
			// However, the location of this registry key depends, in turn, on the processor architecure.  Visual Studio 2010 and 2008 are 32 bit applications.  On 
			// a 32 bit machine, the registry keys are where you would expect to find them. On a 64 bit machine they are stored in a Wow6432Node location that is 
			// distinct from native 64 bit applications.
			String vs2008KeyPath = System.IntPtr.Size == 4 ? @"HKEY_LOCAL_MACHINE\Software\Microsoft\VisualStudio\9.0" :
				@"HKEY_LOCAL_MACHINE\Software\Wow6432Node\Microsoft\VisualStudio\9.0";
			String vs2010KeyPath = System.IntPtr.Size == 4 ? @"HKEY_LOCAL_MACHINE\Software\Microsoft\VisualStudio\10.0" :
				@"HKEY_LOCAL_MACHINE\Software\Wow6432Node\Microsoft\VisualStudio\10.0";

			// Install the Visual Studio 2008 version of the templates.
			this.InstallVersion(
				@"Project Templates\Visual Studio 2008\Visual C#\Windows",
				@"ProjectTemplates\CSharp\Windows\1033",
				vs2008KeyPath,
				"InstallDir");

			// Install the Visual Studio 2010 version of the templates.
			this.InstallVersion(
				@"Project Templates\Visual Studio 2010\Visual C#\Windows",
				@"ProjectTemplates\CSharp\Windows\1033",
				vs2010KeyPath,
				"InstallDir");

		}

		/// <summary>
		/// Installs a specific version of the templates into the IDE.
		/// </summary>
		/// <param name="sourcePath">A fragment of the path where the source templates can be found.</param>
		/// <param name="destinationPath">A fragement of the destination path for the templates.</param>
		/// <param name="keyName">A registry key that indicates where the IDE has been installed.</param>
		/// <param name="valueName">The registry value that indicates where the IDE has been installed.</param>
		void InstallVersion(String sourcePath, String destinationPath, String keyName, String valueName)
		{

			try
			{

				// The main idea here is to move the template files into the Visual Studio directories so they can be used by any user on a machine.  But due to a
				// bug in the way the parameters are expanded, the '[TARGETDIR]' variable must have a '\' character at the end.  Otherwise it will generate strange
				// messages that have nothing to do with the installer.  This will remove the terminator character that was forced on us as a workaround.
				String installerTargetDirectory = (this.Context.Parameters["targetDir"] as String).TrimEnd('\\');
				if (installerTargetDirectory == null)
					return;

				// The installer must put the files on the machine before they can be moved.  This is a temporary condition.  The installation directory becomes the
				// source for the templates during this operation.
				String sourceTemplateDirectory = Path.Combine(installerTargetDirectory, sourcePath);

				// This will attempt to find the directory where the development environment is installed.  If the IDE hasn't been installed, then don't try to copy
				// the templates.
				Object visualStudioDirectoryValue = Registry.GetValue(keyName, valueName, null);
				if (visualStudioDirectoryValue == null)
					return;

				// At this point the Visual Studio version has been installed and we can attempt to place the templates in the development environment for everyone
				// on this machine to use.  Note that this directory not only has the templates, but contains the 'devenv' program which is used to update the
				// template cache to include the newly installed template.
				String visualStudioDirectory = visualStudioDirectoryValue as String;

				// The destination for our template is this directory in the development environment path.
				String destinationTemplateDirectory = Path.Combine(visualStudioDirectory, destinationPath);

				// The destination directory should be there but if it isn't, it's save to create it.
				if (!Directory.Exists(destinationTemplateDirectory))
					Directory.CreateDirectory(destinationTemplateDirectory);

				// This will move the source template to the destination directory and overwrite any templates that may already be there.  Note that we leave the
				// templates in this directory so that at some later stage we have a list of what templates need to be removed.
				foreach (String sourceFileName in Directory.GetFiles(sourceTemplateDirectory))
					File.Copy(sourceFileName, Path.Combine(destinationTemplateDirectory, Path.GetFileName(sourceFileName)), true);

				// Update the Visual Studio Templates.
				this.InstallVisualStudioTemplates(visualStudioDirectory);

			}
			catch (Exception exception)
			{

				// Any errors will be logged but won't interrupt the installation.
				EventLog.WriteEntry("Explorer Chrome", exception.Message, EventLogEntryType.Error);

			}

		}

		/// <summary>
		/// Installs/Updates the Visual Studio Templates.
		/// </summary>
		/// <param name="visualStudioDirectory">The directory where the development environment resides.</param>
		void InstallVisualStudioTemplates(String visualStudioDirectory)
		{

			// Once the templates have been added or deleted the cache needs to be updated.  This process will take a long time, so we'll start it here and let it
			// finish in its own sweet time.  It's not an ideal solution but it does guarantee that all users will be able to access the templates.  Note that we
			// run the process as an administrator.
			Process process = new Process();
			process.StartInfo.Arguments = "/installvstemplates";
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.FileName = Path.Combine(visualStudioDirectory, "devenv");
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.Verb = "runas";
			process.Start();
			process.WaitForExit();

		}

		/// <summary>
		/// Removes an installation.
		/// </summary>
		/// <param name="iDictionary">An IDictionary that contains the state of the computer after the installation was complete.</param>
		public override void Uninstall(IDictionary savedState)
		{

			// We need to remove the templates from the Visual Studio product directory.  However, the location of Visual Studio depends on a registry key. However,
			// the location of this registry key depends, in turn, on the processor architecure.  Visual Studio 2010 and 2008 are 32 bit applications.  On a 32 bit
			// machine, the registry keys are where you would expect to find them. On a 64 bit machine they are stored in a Wow6432Node location that is distinct
			// from native 64 bit applications.
			String vs2008KeyPath = System.IntPtr.Size == 4 ? @"HKEY_LOCAL_MACHINE\Software\Microsoft\VisualStudio\9.0" :
				@"HKEY_LOCAL_MACHINE\Software\Wow6432Node\Microsoft\VisualStudio\9.0";
			String vs2010KeyPath = System.IntPtr.Size == 4 ? @"HKEY_LOCAL_MACHINE\Software\Microsoft\VisualStudio\10.0" :
				@"HKEY_LOCAL_MACHINE\Software\Wow6432Node\Microsoft\VisualStudio\10.0";

			// Uninstall the Visual Studio 2008 version of the templates.
			this.UninstallVersion(
				@"Project Templates\Visual Studio 2008\Visual C#\Windows",
				@"ProjectTemplates\CSharp\Windows\1033",
				vs2008KeyPath,
				"InstallDir");

			// Install the Visual Studio 2010 version of the templates.
			this.UninstallVersion(
				@"Project Templates\Visual Studio 2010\Visual C#\Windows",
				@"ProjectTemplates\CSharp\Windows\1033",
				vs2010KeyPath,
				"InstallDir");

		}

		/// <summary>
		/// Removes a specific version of the templates from the IDE.
		/// </summary>
		/// <param name="sourcePath">A fragment of the path where the source templates can be found.</param>
		/// <param name="destinationPath">A fragement of the destination path for the templates.</param>
		/// <param name="keyName">A registry key that indicates where the IDE has been installed.</param>
		/// <param name="valueName">The registry value that indicates where the IDE has been installed.</param>
		void UninstallVersion(String sourcePath, String destinationPath, String keyName, String valueName)
		{

			try
			{

				// The main idea here is to m ve the template files into the Visual Studio directories so they can be used by any user on a machine.  But due to a 
				// bug in the way the parameters are expanded, the '[TARGETDIR]' variable must have a '\' character at the end.  Otherwise it will generate strange
				// messages that have nothing to do with the installer.  This will remove the terminator character that was forced on us as a workaround.
				String installerTargetDirectory = (this.Context.Parameters["targetDir"] as String).TrimEnd('\\');
				if (installerTargetDirectory == null)
					return;

				// The installer must put the files on the machine before they can be moved.  This is a temporary condition.  The installation directory becomes the
				// source for the templates during this operation.
				String sourceTemplateDirectory = Path.Combine(installerTargetDirectory, sourcePath);

				// This will attempt to find the directory where the development environment is installed.
				Object visualStudioDirectoryValue = Registry.GetValue(keyName, valueName, null);
				if (visualStudioDirectoryValue == null)
					return;

				// At this point the Visual Studio version has been installed and we can attempt to place the templates in the development environment for 
				// everyone on this machine to use.  Note that this directory not only has the templates, but contains the 'devenv' program which is used to
				// update the template cache to include the newly installed template.
				String visualStudioDirectory = visualStudioDirectoryValue as String;

				// The destination for our template is this directory in the development environment path.
				String destinationTemplateDirectory = Path.Combine(visualStudioDirectory, destinationPath);

				// This will delete the templates based on the ones we installed.
				foreach (String sourceFileName in Directory.GetFiles(sourceTemplateDirectory))
					File.Delete(Path.Combine(destinationTemplateDirectory, Path.GetFileName(sourceFileName)));

				// Update the Visual Studio Templates to remove the item from the cache.
				this.InstallVisualStudioTemplates(visualStudioDirectory);

			}
			catch (Exception exception)
			{

				// Any errors will be logged but won't interrupt the installation.
				EventLog.WriteEntry("Explorer Chrome", exception.Message, EventLogEntryType.Error);

			}

		}

	}

}