# Calculate the project root from the invocation.
$projectRoot = $(split-path -parent $(split-path -parent $(split-path -parent $SCRIPT:MyInvocation.MyCommand.Path)))
$sn64 = "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\x64\sn.exe"
$sn32 = "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\sn.exe"

# Turn off verification for these assemblies.
&"${sn64}" -q -Vr "${projectRoot}\Asset Network\Teraque.AssetNetwork\bin\Debug\Teraque.AssetNetwork.dll"
&"${sn64}" -q -Vr "${projectRoot}\Asset Network\Teraque.AssetNetwork.ClientDataModel\bin\Debug\Teraque.AssetNetwork.ClientDataModel.dll"
&"${sn64}" -q -Vr "${projectRoot}\Teraque\bin\Debug\Teraque.dll"
&"${sn64}" -q -Vr "${projectRoot}\Teraque.Server\bin\Debug\Teraque.Server.dll"

# Visual Studio runs in 32 bit mode and needs to be able to read signed assemblies when resolving Service References.
&"${sn32}" -q -Vr "${projectRoot}\Asset Network\Teraque.AssetNetwork\bin\Debug\Teraque.AssetNetwork.dll"
&"${sn32}" -q -Vr "${projectRoot}\Asset Network\Teraque.AssetNetwork.ClientDataModel\bin\Debug\Teraque.AssetNetwork.ClientDataModel.dll"
&"${sn32}" -q -Vr "${projectRoot}\Teraque\bin\Debug\Teraque.dll"
&"${sn32}" -q -Vr "${projectRoot}\Teraque.Server\bin\Debug\Teraque.Server.dll"
