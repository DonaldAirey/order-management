# Verify Off for the 64 bit environment (the runtime)
&"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\x64\sn.exe" -q -Vr "${Home}\Documents\Visual Studio 2010\Projects\Teraque\Main\Teraque\bin\Debug\Teraque.dll"
&"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin\x64\sn.exe" -q -Vr "${Home}\Documents\Visual Studio 2010\Projects\Teraque\Main\Teraque.PdfViewer\obj\Debug\Teraque.PdfViewer.dll"
&"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\x64\sn.exe" -q -Vr "${Home}\Documents\Visual Studio 2010\Projects\Teraque\Main\Teraque.PresentationFramework\bin\Debug\Teraque.PresentationFramework.dll"
# Verify Off for the 32 bit environment (which includes the Visual Studio designer)
&"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\sn.exe" -q -Vr "${Home}\Documents\Visual Studio 2010\Projects\Teraque\Main\Teraque\bin\Debug\Teraque.dll"
&"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin\sn.exe" -q -Vr "${Home}\Documents\Visual Studio 2010\Projects\Teraque\Main\Teraque.PdfViewer\obj\Debug\Teraque.PdfViewer.dll"
&"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\sn.exe" -q -Vr "${Home}\Documents\Visual Studio 2010\Projects\Teraque\Main\Teraque.PresentationFramework\bin\Debug\Teraque.PresentationFramework.dll"
