# Configuration
$databaseRoot = $(split-path -parent $(split-path -parent $(split-path -parent $(split-path -parent $SCRIPT:MyInvocation.MyCommand.Path))))

# Stop the Asset Network Web Service
net stop "Asset Network Web Service"

# Install the database schemas
&"$databaseRoot\Asset Network\Database\Scripts\Install Data Model.ps1" "Aspen Group"
&"$databaseRoot\Asset Network\Database\Scripts\Install Data Model.ps1" "Moorehead Brown"

# Re-start the Asset Network Web Service
net start "Asset Network Web Service"

