# This describes the certificate
$domain = "localhost"

# This script must be run with administrator privileges before the service can be run in the development environment.
# Project root.
$projectRoot = $(split-path -parent $(split-path -parent $SCRIPT:MyInvocation.MyCommand.Path))

# Remove the previous configuration of the ports.
netsh http delete urlacl url="http://+:80/Asset Network/Market Service"

# To run this project in Visual Studio without Administrator privileges, grant access to the private key of the 'localhost' certificate to the
# current user.  In addition, this gives the current user permission to use the ports (80 and 443).  This script must be run with elevated
# (Administrator) privileges.
$user = [Environment]::UserName
netsh http add urlacl url="http://+:80/Asset Network/Market Service" user="$user"
