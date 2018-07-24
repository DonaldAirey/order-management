# Calculate the project root from the invocation.
$projectRoot = $(split-path $(split-path -parent $(split-path -parent $(split-path -parent $SCRIPT:MyInvocation.MyCommand.Path))))
$keyRoot = "C:\Source\Product Keys"
$sn64 = "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\x64\sn.exe"

# Sign the partially signed assemblies.
&"${sn64}" -q -R "${projectRoot}\Asset Network\Script Loader\Script Loader\bin\release\Teraque.dll" "${keyRoot}\Key Pair.snk"
&"${sn64}" -q -R "${projectRoot}\Asset Network\Script Loader\Script Loader\bin\release\Teraque.AssetNetwork.dll" "${keyRoot}\Order Management\Key Pair.snk"
&"${sn64}" -q -R "${projectRoot}\Asset Network\Script Loader\Script Loader\bin\release\Teraque.AssetNetwork.ClientDataModel.dll" "${keyRoot}\Order Management\Key Pair.snk"
&"${sn64}" -q -R "${projectRoot}\Asset Network\Script Loader\Script Loader\bin\release\Teraque.Server.dll" "${keyRoot}\Key Pair.snk"