Write-Host "Generating and Installing SQL"
&"C:\Program Files (x86)\Teraque\Data Model Generator\Teraque.DataModelGenerator.SqlCompiler.exe" "${Home}\Documents\Visual Studio 2010\Projects\Teraque\Main\Asset Network\Teraque.AssetNetwork.ServerDataModel\DataModel.xsd" -out "${Home}\Documents\Visual Studio 2010\Projects\Teraque\Main\Asset Network\Database\Scripts\DataModel.sql"
&"C:\Users\Donald Roy Airey\Documents\Visual Studio 2010\Projects\Teraque\Main\Asset Network\Database\Scripts\Install Data Models.ps1"
