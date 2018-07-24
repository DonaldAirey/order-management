# Calculate the project root from the invocation.
$projectRoot = $(split-path -parent $(split-path -parent $(split-path -parent $SCRIPT:MyInvocation.MyCommand.Path)))
$sn64 = "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\x64\sn.exe"
$sn32 = "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\sn.exe"

# Turn off verification for 64 bit applications.
&"$sn64" -q -Vr "$projectRoot\Asset Network\Teraque.AssetNetwork\bin\Debug\Teraque.AssetNetwork.dll"
&"$sn64" -q -Vr "$projectRoot\Asset Network\Teraque.AssetNetwork.ServerDataModel\bin\Debug\Teraque.AssetNetwork.ServerDataModel.dll"
&"$sn64" -q -Vr "$projectRoot\Asset Network\Teraque.AssetNetwork.MarketEngine\bin\Debug\Teraque.AssetNetwork.MarketEngine.dll"
&"$sn64" -q -Vr "$projectRoot\Asset Network\Teraque.AssetNetwork.WebService\bin\Debug\Teraque.AssetNetwork.WebService.dll"
&"$sn64" -q -Vr "$projectRoot\Asset Network\Web Service Host\bin\Debug\Web Service Host.exe"
&"$sn64" -q -Vr "$projectRoot\Teraque\bin\Debug\Teraque.dll"
&"$sn64" -q -Vr "$projectRoot\Teraque.Server\bin\Debug\Teraque.Server.dll"
&"$sn64" -q -Vr "$projectRoot\Teraque.Message\bin\Debug\Teraque.Message.dll"

# Turn off verification for 32 bit applications.  This is important for the Visual Studio IDE design surface.
&"$sn32" -q -Vr "$projectRoot\Asset Network\Teraque.AssetNetwork\bin\Debug\Teraque.AssetNetwork.dll"
&"$sn32" -q -Vr "$projectRoot\Asset Network\Teraque.AssetNetwork.ServerDataModel\bin\Debug\Teraque.AssetNetwork.ServerDataModel.dll"
&"$sn32" -q -Vr "$projectRoot\Asset Network\Teraque.AssetNetwork.MarketEngine\bin\Debug\Teraque.AssetNetwork.MarketEngine.dll"
&"$sn32" -q -Vr "$projectRoot\Asset Network\Teraque.AssetNetwork.WebService\bin\Debug\Teraque.AssetNetwork.WebService.dll"
&"$sn32" -q -Vr "$projectRoot\Asset Network\Web Service Host\bin\Debug\Web Service Host.exe"
&"$sn32" -q -Vr "$projectRoot\Teraque\bin\Debug\Teraque.dll"
&"$sn32" -q -Vr "$projectRoot\Teraque.Server\bin\Debug\Teraque.Server.dll"
&"$sn32" -q -Vr "$projectRoot\Teraque.Message\bin\Debug\Teraque.Message.dll"
