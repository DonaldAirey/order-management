# Turn off verification for these assemblies.
&"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\x64\sn.exe"-q -Vr "${Home}\Documents\Visual Studio 2010\Projects\Teraque\Main\Asset Network\Teraque.AssetNetwork\bin\Debug\Teraque.AssetNetwork.dll"
&"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\x64\sn.exe"-q -Vr "${Home}\Documents\Visual Studio 2010\Projects\Teraque\Main\Asset Network\Teraque.AssetNetwork.WebClient\bin\Debug\Teraque.AssetNetwork.WebClient.dll"
&"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\x64\sn.exe"-q -Vr "${Home}\Documents\Visual Studio 2010\Projects\Teraque\Main\Teraque\bin\Debug\Teraque.dll"
&"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\x64\sn.exe"-q -Vr "${Home}\Documents\Visual Studio 2010\Projects\Teraque\Main\Teraque.Message\bin\Debug\Teraque.Message.dll"
&"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\x64\sn.exe"-q -Vr "${Home}\Documents\Visual Studio 2010\Projects\Teraque\Main\Teraque.PresentationFramework\bin\Debug\Teraque.PresentationFramework.dll"
&"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\x64\sn.exe"-q -Vr "${Home}\Documents\Visual Studio 2010\Projects\Teraque\Main\Teraque.Server\bin\Debug\Teraque.Server.dll"

# Visual Studio runs in 32 bits.  We need to turn verification off to use the assemblies in this environment also.
&"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\sn.exe"-q -Vr "${Home}\Documents\Visual Studio 2010\Projects\Teraque\Main\Asset Network\Teraque.AssetNetwork\bin\Debug\Teraque.AssetNetwork.dll"
&"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\sn.exe"-q -Vr "${Home}\Documents\Visual Studio 2010\Projects\Teraque\Main\Asset Network\Teraque.AssetNetwork.WebClient\bin\Debug\Teraque.AssetNetwork.WebClient.dll"
&"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\sn.exe"-q -Vr "${Home}\Documents\Visual Studio 2010\Projects\Teraque\Main\Teraque\bin\Debug\Teraque.dll"
&"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\sn.exe"-q -Vr "${Home}\Documents\Visual Studio 2010\Projects\Teraque\Main\Teraque.Message\bin\Debug\Teraque.Message.dll"
&"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\sn.exe"-q -Vr "${Home}\Documents\Visual Studio 2010\Projects\Teraque\Main\Teraque.PresentationFramework\bin\Debug\Teraque.PresentationFramework.dll"
&"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\sn.exe"-q -Vr "${Home}\Documents\Visual Studio 2010\Projects\Teraque\Main\Teraque.Server\bin\Debug\Teraque.Server.dll"
