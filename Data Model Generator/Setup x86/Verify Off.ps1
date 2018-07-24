# Calculate the project root from the invocation.
$projectRoot = $(split-path -parent $(split-path -parent $(split-path -parent $SCRIPT:MyInvocation.MyCommand.Path)))
$sn64 = "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\x64\sn.exe"
$sn32 = "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\sn.exe"

# Turn off verification for 64 bit applications.
&"${sn64}" -q -Vr "${projectRoot}\Teraque\bin\Debug\Teraque.dll"
&"${sn64}" -q -Vr "${projectRoot}\Teraque.Server\bin\Debug\Teraque.Server.dll"
&"${sn64}" -q -Vr "${projectRoot}\Data Model Generator\Teraque.DataModelGenerator\bin\Debug\Teraque.DataModelGenerator.dll"
&"${sn64}" -q -Vr "${projectRoot}\Data Model Generator\Teraque.DataModelGenerator.Client\bin\Debug\Teraque.DataModelGenerator.Client.dll"
&"${sn64}" -q -Vr "${projectRoot}\Data Model Generator\Teraque.DataModelGenerator.Server\bin\Debug\Teraque.DataModelGenerator.Server.dll"
&"${sn64}" -q -Vr "${projectRoot}\Data Model Generator\SQL Compiler\bin\Debug\Teraque.DataModelGenerator.SqlCompiler.exe"
&"${sn64}" -q -Vr "${projectRoot}\Data Model Generator\Client Compiler\bin\Debug\Teraque.DataModelGenerator.ClientCompiler.exe"
&"${sn64}" -q -Vr "${projectRoot}\Data Model Generator\Server Compiler\bin\Debug\Teraque.DataModelGenerator.ServerCompiler.exe"
&"${sn64}" -q -Vr "${projectRoot}\Data Model Generator\Schema Scrubber\bin\Debug\Teraque.DataModelGenerator.SchemaScrubber.exe"
