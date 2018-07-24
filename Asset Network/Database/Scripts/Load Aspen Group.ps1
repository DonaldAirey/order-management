# Configuration
$projectRoot = $(split-path -parent $(split-path -parent $(split-path -parent $(split-path -parent $SCRIPT:MyInvocation.MyCommand.Path))))
$scriptLoader = "C:\Program Files\Teraque\Asset Network\Script Loader\Script Loader.exe"

# Common Data
&"$projectRoot\Asset Network\Database\Scripts\Load Small Data Model.ps1"

# Aspen Group
&"$scriptLoader" -i "$projectRoot\Asset Network\Database\Unit Test\Aspen Group.xml"
&"$scriptLoader" -i "$projectRoot\Asset Network\Database\Unit Test\Emerging Markets Orders.xml"
&"$scriptLoader" -i "$projectRoot\Asset Network\Database\Unit Test\Strategies Orders.xml"
&"$scriptLoader" -i "$projectRoot\Asset Network\Database\Unit Test\Hector Kurtz Orders.xml"
&"$scriptLoader" -i "$projectRoot\Asset Network\Database\Unit Test\Juan Green Orders.xml"
&"$scriptLoader" -i "$projectRoot\Asset Network\Database\Unit Test\Gary Stein Orders.xml"