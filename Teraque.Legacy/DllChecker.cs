using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace Teraque
{
	/// <summary>
	/// static class that will check the loaded dll vs the dll that is located in the 
	/// dll's target build directory. This will ensure that dlls that should have been
	/// copied to the run directory actually are
	/// </summary>
	public static class DllChecker
	{
#if DEBUG

		/// <summary>
		/// method that will check the loaded dll vs the dll that is located in the 
		/// dll's target build directory. This will ensure that dlls that should have been
		/// copied to the run directory actually are
		/// </summary>
		/// <param name="assembly">assembly to check</param>
		/// <returns>true if match, false if does not match or dll build target is missing</returns>
		public static bool CheckDll(Assembly assembly)
		{
			string dllString = null;

			//read the assembly into a string so can pull out the pdb location
			using(FileStream fs = new FileStream(assembly.Location, FileMode.Open, FileAccess.Read))
			{
				using(StreamReader sr = new StreamReader(fs, Encoding.ASCII))
				{
					dllString = sr.ReadToEnd();
				}
			}
			//find the pdb location.. always close to the end of the file
			//there is a debugging API that can do this, or could run visualstudio/vc/bin/dumpbin /pdbpath, but that 
			//requires the path to be correct of have to copy DIA or dumpbin
			//to a know location so will just scan the file for the pdb
			int index = dllString.LastIndexOf(".pdb", StringComparison.InvariantCultureIgnoreCase);
			if (index != -1)
			{
				int startPathIndex = dllString.LastIndexOf(@":\", index, 500);
				if (startPathIndex != -1)
				{
					dllString = dllString.Substring(startPathIndex - 1, (index - startPathIndex) + 5);

					dllString = dllString.ToLowerInvariant().Replace(".pdb", ".dll");
					if (File.Exists(dllString))
					{
						DateTime buildLocationDllDate = System.IO.File.GetLastWriteTime(dllString);

						DateTime loadedDllDate = System.IO.File.GetLastWriteTime(assembly.Location);

						//return true if dates match
						return buildLocationDllDate == loadedDllDate;
					}
				}
				else
				{
					//not sure how you would get here.. but just incase
					dllString = null;
				}
			}
			else
			{
				//if not pdb location then assume this is a release dll so it is ok
				dllString = null;
				return true;
			}

			if (string.IsNullOrEmpty(dllString) == false)
			{
				//if a release dll then it is ok
				if (dllString.Contains("release"))
					return true;
			}
			//either dates dont match or missing return false
			return false;
		}
#endif
	}
}
