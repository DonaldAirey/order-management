﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;

// General information about the assembly.
[assembly: AssemblyTitle("Teraque.AssetNetwork.Windows")]
[assembly: AssemblyDescription("Windows Library for the Teraque Asset Network")]
[assembly: AssemblyCompany("Teraque, Inc.")]
[assembly: AssemblyProduct("Teraque.AssetNetwork.Windows")]
[assembly: AssemblyCopyright("Copyright © 2010-2012, Teraque, Inc.  All rights reserved.")]
[assembly: AssemblyTrademark("The Essential Element")]

// Indicates that this assembly is compliant with the Common Language Specification (CLS).
[assembly: CLSCompliant(true)]

// Disables the accessibility of this assembly to COM.
[assembly: ComVisible(false)]

// Describes the default language used for the resources.
[assembly: NeutralResourcesLanguageAttribute("en-US")]

// Version information for this asssembly.
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// This instructs the loader that WPF themes and the generic theme can be found in this library.
[assembly: ThemeInfo(ResourceDictionaryLocation.SourceAssembly, ResourceDictionaryLocation.SourceAssembly)]

// XAML namespace mappings.
[assembly: XmlnsDefinition("http://schemas.teraque.com/winfx/2006/xaml/asset/presentation", "Teraque.AssetNetwork.Windows")]
[assembly: XmlnsDefinition("http://schemas.teraque.com/winfx/2006/xaml/asset/presentation", "Teraque.AssetNetwork.Windows.Controls")]
[assembly: XmlnsDefinition("http://schemas.teraque.com/winfx/2006/xaml/asset/presentation", "Teraque.AssetNetwork.Windows.Input")]

// These specific messages are suppressed when the Code Analsysis is run.
[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Teraque", Scope = "namespace", Target = "Teraque.AssetNetwork.Windows")]
[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Teraque.AssetNetwork.Windows.Controls")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Teraque", Scope = "namespace", Target = "Teraque.AssetNetwork.Windows.Controls")]
[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Teraque.AssetNetwork.Windows.Input")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Teraque", Scope = "namespace", Target = "Teraque.AssetNetwork.Windows.Input")]
[assembly: SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Teraque")]