using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;

// General information about the assembly.
[assembly: AssemblyTitle("Explorer Chrome Example")]
[assembly: AssemblyDescription("An example of the Explorer Chrome Suite")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Teraque, Inc.")]
[assembly: AssemblyProduct("Explorer Chrome Suite Demo")]
[assembly: AssemblyCopyright("Copyright © Teraque 2011")]
[assembly: AssemblyTrademark("The Essential Element")]
[assembly: AssemblyCulture("")]

// Indicates that this assembly is compliant with the Common Language Specification (CLS).
[assembly: CLSCompliant(true)]

// Disables the accessibility of this assembly to COM.
[assembly: ComVisible(false)]

// Describes the default language used for the resources.
[assembly: NeutralResourcesLanguageAttribute("en-US")]

// Version information for this asssembly.
[assembly: AssemblyVersion("1.1.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// This instructs the loader that WPF themes and the generic theme can be found in this library.
[assembly: ThemeInfo(ResourceDictionaryLocation.SourceAssembly, ResourceDictionaryLocation.SourceAssembly)]

// These specific messages are suppressed when the Code Analsysis is run.
[assembly: SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Teraque")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Teraque", Scope = "namespace", Target = "Teraque.Exchange")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", MessageId = "Teraque", Scope = "resource", Target = "Teraque.ExplorerChromeExample.Properties.Resources.resources")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", MessageId = "teraque", Scope = "resource", Target = "Teraque.ExplorerChromeExample.Properties.Resources.resources")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Teraque", Scope = "namespace", Target = "Teraque")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Teraque", Scope = "namespace", Target = "Teraque.ExplorerChromeExample")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Scope = "member", Target = "Teraque.Exchange.WindowMain.#System.Windows.Markup.IComponentConnector.Connect(System.Int32,System.Object)")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Scope = "member", Target = "Teraque.ExplorerChromeExample.WindowAbout.#System.Windows.Markup.IComponentConnector.Connect(System.Int32,System.Object)")]
[assembly: SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "XamlGeneratedNamespace.GeneratedInternalTypeHelper.#AddEventHandler(System.Reflection.EventInfo,System.Object,System.Delegate)", Justification = "Auto-generated code")]
[assembly: SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Scope = "member", Target = "XamlGeneratedNamespace.GeneratedInternalTypeHelper.#CreateDelegate(System.Type,System.Object,System.String)", Justification = "Auto-generated code")]
[assembly: SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "XamlGeneratedNamespace.GeneratedInternalTypeHelper.#GetPropertyValue(System.Reflection.PropertyInfo,System.Object,System.Globalization.CultureInfo)", Justification = "Auto-generated code")]
[assembly: SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "XamlGeneratedNamespace.GeneratedInternalTypeHelper.#SetPropertyValue(System.Reflection.PropertyInfo,System.Object,System.Object,System.Globalization.CultureInfo)", Justification = "Auto-generated code")]
