using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Markup;
using System.Resources;

// General information regarding the assembly.
[assembly: AssemblyTitle("Data Model")]
[assembly: AssemblyDescription("The data and model used for the sandbox.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Teraque")]
[assembly: AssemblyProduct("Client Data Model")]
[assembly: AssemblyCopyright("Copyright © Teraque 2010")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Indicates that this assembly is compliant with the Common Language Specification (CLS).
[assembly: CLSCompliant(true)]

// Disables the accessibility of this assembly to COM.
[assembly: ComVisible(false)]

// This provides XAML compatible mappings to this library.
[assembly: XmlnsDefinition("http://schemas.teraque.com/winfx/2012/xaml/data", "Teraque")]

// The neutural language for this assembly.
[assembly: NeutralResourcesLanguageAttribute("en-US")]

// Version information for this asssembly.
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// These specific messages are suppressed when the Code Analsysis is run.
[assembly: SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Teraque")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Teraque", Scope = "namespace", Target = "Teraque")]
[assembly: SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly", Scope = "type", Target = "Teraque.DataSet", Justification = "Auto-generated code")]
[assembly: SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly", Scope = "type", Target = "Teraque.DataSet+PropertyStoreDataTable", Justification = "Auto-generated code")]
[assembly: SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly", Scope = "type", Target = "Teraque.DataSet+RootDataTable", Justification = "Auto-generated code")]
[assembly: SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly", Scope = "type", Target = "Teraque.DataSet+TreeDataTable", Justification = "Auto-generated code")]
[assembly: SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly", Scope = "type", Target = "Teraque.DataSet+TypeDataTable", Justification = "Auto-generated code")]
[assembly: SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly", Scope = "type", Target = "Teraque.DataSet+PropertyDataTable", Justification = "Auto-generated code")]
