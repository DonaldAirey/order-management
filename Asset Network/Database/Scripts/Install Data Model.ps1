# Calculate the project root from the invocation.
$server = "."
$databaseRoot = $(split-path -parent $(split-path -parent $SCRIPT:MyInvocation.MyCommand.Path))
$sqlcmd = "C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\130\Tools\Binn\sqlcmd.exe"

# Make sure a server name was provided on the command line.
if ($args.count -eq 0)
{
	$(throw "You must provide the name of the organization")
}

# Drop the previous schema and install the new one.
write-host "Installing schemas for" $args[0]
&"${sqlcmd}" -S ${server} -E -d $args[0] -i "${databaseRoot}\Scripts\Drop All.sql"
&"${sqlcmd}" -S ${server} -E -d $args[0] -i "${databaseRoot}\Scripts\DataModel.sql"