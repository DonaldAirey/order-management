# Calculate the project root from the invocation.
$projectRoot = $(split-path -parent $(split-path -parent $(split-path -parent $SCRIPT:MyInvocation.MyCommand.Path)))
$sn64 = "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\x64\sn.exe"
$sn32 = "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\sn.exe"

# Turn off verification for 64 bit applications.
&"${sn64}" -q -Vr "$projectRoot\Asset Network\Teraque.AssetNetwork\bin\Debug\Teraque.AssetNetwork.dll"
&"${sn64}" -q -Vr "$projectRoot\Asset Network\Teraque.AssetNetwork.Blotter\bin\Debug\Teraque.AssetNetwork.Blotter.dll"
&"${sn64}" -q -Vr "$projectRoot\Asset Network\Teraque.AssetNetwork.ClientDataModel\bin\Debug\Teraque.AssetNetwork.ClientDataModel.dll"
&"${sn64}" -q -Vr "$projectRoot\Asset Network\Teraque.AssetNetwork.DebtBlotter\bin\Debug\Teraque.AssetNetwork.DebtBlotter.dll"
&"${sn64}" -q -Vr "$projectRoot\Asset Network\Teraque.AssetNetwork.EquityBlotter\bin\Debug\Teraque.AssetNetwork.EquityBlotter.dll"
&"${sn64}" -q -Vr "$projectRoot\Asset Network\Teraque.AssetNetwork.Navigator\bin\Debug\Teraque.AssetNetwork.Navigator.dll"
&"${sn64}" -q -Vr "$projectRoot\Asset Network\Teraque.AssetNetwork.WebClient\bin\Debug\Teraque.AssetNetwork.WebClient.dll"
&"${sn64}" -q -Vr "$projectRoot\Asset Network\Teraque.AssetNetwork.Windows\bin\Debug\Teraque.AssetNetwork.Windows.dll"
&"${sn64}" -q -Vr "$projectRoot\Teraque\bin\Debug\Teraque.dll"
&"${sn64}" -q -Vr "$projectRoot\Teraque.Message\bin\Debug\Teraque.Message.dll"
&"${sn64}" -q -Vr "$projectRoot\Teraque.PortableDocumentFormat\obj\Debug\Teraque.PdfViewer.dll"
&"${sn64}" -q -Vr "$projectRoot\Teraque.PresentationFramework\bin\Debug\Teraque.PresentationFramework.dll"
&"${sn64}" -q -Vr "$projectRoot\Teraque.Server\bin\Debug\Teraque.Server.dll"

# Turn off verification for 32 bit applications.
&"${sn32}" -q -Vr "$projectRoot\Asset Network\Teraque.AssetNetwork\bin\Debug\Teraque.AssetNetwork.dll"
&"${sn32}" -q -Vr "$projectRoot\Asset Network\Teraque.AssetNetwork.Blotter\bin\Debug\Teraque.AssetNetwork.Blotter.dll"
&"${sn32}" -q -Vr "$projectRoot\Asset Network\Teraque.AssetNetwork.ClientDataModel\bin\Debug\Teraque.AssetNetwork.ClientDataModel.dll"
&"${sn32}" -q -Vr "$projectRoot\Asset Network\Teraque.AssetNetwork.DebtBlotter\bin\Debug\Teraque.AssetNetwork.DebtBlotter.dll"
&"${sn32}" -q -Vr "$projectRoot\Asset Network\Teraque.AssetNetwork.EquityBlotter\bin\Debug\Teraque.AssetNetwork.EquityBlotter.dll"
&"${sn32}" -q -Vr "$projectRoot\Asset Network\Teraque.AssetNetwork.Navigator\bin\Debug\Teraque.AssetNetwork.Navigator.dll"
&"${sn32}" -q -Vr "$projectRoot\Asset Network\Teraque.AssetNetwork.WebClient\bin\Debug\Teraque.AssetNetwork.WebClient.dll"
&"${sn32}" -q -Vr "$projectRoot\Asset Network\Teraque.AssetNetwork.Windows\bin\Debug\Teraque.AssetNetwork.Windows.dll"
&"${sn32}" -q -Vr "$projectRoot\Teraque\bin\Debug\Teraque.dll"
&"${sn32}" -q -Vr "$projectRoot\Teraque.Message\bin\Debug\Teraque.Message.dll"
&"${sn32}" -q -Vr "$projectRoot\Teraque.PortableDocumentFormat\obj\Debug\Teraque.PdfViewer.dll"
&"${sn32}" -q -Vr "$projectRoot\Teraque.PresentationFramework\bin\Debug\Teraque.PresentationFramework.dll"
&"${sn32}" -q -Vr "$projectRoot\Teraque.Server\bin\Debug\Teraque.Server.dll"
