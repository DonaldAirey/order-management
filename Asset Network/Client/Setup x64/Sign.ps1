# Calculate the project root from the invocation.
${projectRoot} = $(split-path $(split-path -parent $(split-path -parent $(split-path -parent $SCRIPT:MyInvocation.MyCommand.Path))))
$keyRoot = "C:\Source\Product Keys"
$sn64 = "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\x64\sn.exe"

# Sign the partially signed assemblies.
&"${sn64}" -q -R "${projectRoot}\Teraque\bin\Release\Teraque.dll" "${keyRoot}\Key Pair.snk"
&"${sn64}" -q -R "${projectRoot}\Asset Network\Teraque.AssetNetwork\bin\Release\Teraque.AssetNetwork.dll" "${keyRoot}\Order Management\Key Pair.snk"
&"${sn64}" -q -R "${projectRoot}\Asset Network\Teraque.AssetNetwork.Blotter\bin\Release\Teraque.AssetNetwork.Blotter.dll" "${keyRoot}\Order Management\Key Pair.snk"
&"${sn64}" -q -R "${projectRoot}\Asset Network\Teraque.AssetNetwork.ClientDataModel\bin\Release\Teraque.AssetNetwork.ClientDataModel.dll" "${keyRoot}\Order Management\Key Pair.snk"
&"${sn64}" -q -R "${projectRoot}\Asset Network\Teraque.AssetNetwork.DebtBlotter\bin\Release\Teraque.AssetNetwork.DebtBlotter.dll" "${keyRoot}\Order Management\Key Pair.snk"
&"${sn64}" -q -R "${projectRoot}\Asset Network\Teraque.AssetNetwork.EquityBlotter\bin\Release\Teraque.AssetNetwork.EquityBlotter.dll" "${keyRoot}\Order Management\Key Pair.snk"
&"${sn64}" -q -R "${projectRoot}\Asset Network\Teraque.AssetNetwork.Navigator\bin\Release\Teraque.AssetNetwork.Navigator.dll" "${keyRoot}\Order Management\Key Pair.snk"
&"${sn64}" -q -R "${projectRoot}\Asset Network\Teraque.AssetNetwork.WebClient\bin\Release\Teraque.AssetNetwork.WebClient.dll" "${keyRoot}\Order Management\Key Pair.snk"
&"${sn64}" -q -R "${projectRoot}\Asset Network\Teraque.AssetNetwork.Windows\bin\Release\Teraque.AssetNetwork.Windows.dll" "${keyRoot}\Order Management\Key Pair.snk"
&"${sn64}" -q -R "${projectRoot}\Teraque.Message\bin\Release\Teraque.Message.dll" "${keyRoot}\Key Pair.snk"
&"${sn64}" -q -R "${projectRoot}\Teraque.PresentationFramework\bin\Release\Teraque.PresentationFramework.dll" "${keyRoot}\Key Pair.snk"
&"${sn64}" -q -R "${projectRoot}\Teraque.Server\bin\Release\Teraque.Server.dll" "${keyRoot}\Key Pair.snk"
