# Obfuscate and Sign the assemblies
echo "Obfuscating..."
# &"C:\Program Files (x86)\Microsoft Visual Studio 10.0\PreEmptive Solutions\Dotfuscator Community Edition\DotfuscatorCLI.exe" /q /in:"${Home}\Documents\Visual Studio 2010\Projects\Teraque\Main\Teraque\obj\Evaluation\Teraque.dll" /out:"${Home}\Documents\Visual Studio 2010\Projects\Teraque\Main\Teraque\obj\Evaluation" >"${Home}\Documents\Visual Studio 2010\Projects\Teraque\Main\Teraque\obj\Evaluation\obfuscate.txt"
echo "Signing..."
&"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin\NETFX 4.0 Tools\x64\sn.exe" -q -R "${Home}\Documents\Visual Studio 2010\Projects\Teraque\Main\Teraque\obj\Evaluation\Teraque.dll" "${Home}\Documents\My Keys\Teraque\Key Pair.snk"
