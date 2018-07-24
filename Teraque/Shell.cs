namespace Teraque
{

	using System;
	using System.Globalization;
	using Microsoft.Win32;

	/// <summary>
	/// A set of methods for interacting with the shell.
	/// </summary>
	public static class Shell
	{

		/// <summary>
		/// Gets the program identifier associated with the given extension.
		/// </summary>
		/// <param name="extension">The file extension.</param>
		/// <returns>Finds the program associated with the given file extension.</returns>
		public static String GetProgId(String extension)
		{

			// Gets the ProgId associated with the given extension.
			Object value = Shell.ReadRegistryKey(extension, String.Empty);
			if (value == null)
				return String.Empty;
			return value.ToString();

		}

		/// <summary>
		/// Gets the command to open a file given the program's identifier.
		/// </summary>
		/// <param name="progId">The program identifier.</param>
		/// <returns>The command used to open files associated with the given ProgId.</returns>
		public static String GetOpenCommand(String progId)
		{

			// Gets the command for opening the given ProgId.
			RegistryKey classesRoot = Registry.ClassesRoot;
			RegistryKey progIdKey = classesRoot.OpenSubKey(progId);
			RegistryKey shellKey = progIdKey.OpenSubKey("shell");
			RegistryKey openKey = shellKey.OpenSubKey("Open");
			RegistryKey commandKey = openKey.OpenSubKey("command");
			return Convert.ToString(commandKey.GetValue(String.Empty), CultureInfo.InvariantCulture);

		}

		/// <summary>
		/// Reads the given registry key.
		/// </summary>
		/// <param name="path">The absolute path of the key in the registry.</param>
		/// <param name="valueName">The namve of the value to read in the key.</param>
		/// <returns>The value associated the value name in the given registry key.</returns>
		static Object ReadRegistryKey(String path, String valueName)
		{

			// Reads the registry key by breaking up the path into its components and then recursing in the registry until the given valueName is found.
			RegistryKey registryKey = Registry.ClassesRoot;
			String[] pathParts = path.Split('\\');
			for (Int32 index = 0; index < pathParts.Length; index++)
			{
				registryKey = registryKey.OpenSubKey(pathParts[index]);
				if (registryKey == null)
					return null;
			}
			return registryKey.GetValue(valueName, null, RegistryValueOptions.DoNotExpandEnvironmentNames);

		}

	}

}
