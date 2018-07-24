# Configuration
$projectRoot = $(split-path -parent $(split-path -parent $(split-path -parent $(split-path -parent $SCRIPT:MyInvocation.MyCommand.Path))))
$scriptLoader = "C:\Program Files\Teraque\Asset Network\Script Loader\Script Loader.exe"

# Common Data
&"$projectRoot\Asset Network\Database\Scripts\Load Small Data Model.ps1"

# Moorehead Brown
&"$scriptLoader" -i "$projectRoot\Asset Network\Database\Unit Test\Moorehead Brown.xml"
&"$scriptLoader" -i "$projectRoot\Asset Network\Database\Unit Test\Mitsuo Tanaka Orders.xml"
&"$scriptLoader" -i "$projectRoot\Asset Network\Database\Unit Test\Donovan Snow Orders.xml"
&"$scriptLoader" -i "$projectRoot\Asset Network\Database\Unit Test\Sergei Nabokov Orders.xml"
