using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;

// General information about the assembly.
[assembly: AssemblyTitle("Teraque.Server")]
[assembly: AssemblyDescription("The full version of the library of essential types.")]
[assembly: AssemblyCompany("Teraque, Inc.")]
[assembly: AssemblyProduct("Teraque")]
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
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "Teraque")]

// These specific messages are suppressed when the Code Analsysis is run.
[assembly: SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Teraque")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Teraque", Scope = "namespace", Target = "Teraque")]
[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "XamlGeneratedNamespace")]
[assembly: SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "XamlGeneratedNamespace.GeneratedInternalTypeHelper.#AddEventHandler(System.Reflection.EventInfo,System.Object,System.Delegate)", Justification = "Auto-generated code")]
[assembly: SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Scope = "member", Target = "XamlGeneratedNamespace.GeneratedInternalTypeHelper.#CreateDelegate(System.Type,System.Object,System.String)", Justification = "Auto-generated code")]
[assembly: SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "XamlGeneratedNamespace.GeneratedInternalTypeHelper.#GetPropertyValue(System.Reflection.PropertyInfo,System.Object,System.Globalization.CultureInfo)", Justification = "Auto-generated code")]
[assembly: SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "XamlGeneratedNamespace.GeneratedInternalTypeHelper.#SetPropertyValue(System.Reflection.PropertyInfo,System.Object,System.Object,System.Globalization.CultureInfo)", Justification = "Auto-generated code")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", MessageId = "lic", Scope = "resource", Target = "Teraque.Properties.Resources.resources")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Teraque", Scope = "namespace", Target = "Teraque")]
[assembly: SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.Windows.Media.Effects.ShaderEffect", Scope = "member", Target = "Teraque.GrayscaleEffect.#.cctor()")]
[assembly: SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.Windows.Media.Effects.ShaderEffect", Scope = "member", Target = "Teraque.OuterGlowEffect.#.cctor()")]
[assembly: SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.Windows.Media.Effects.ShaderEffect", Scope = "member", Target = "Teraque.EmbossedEffect.#.cctor()")]
[assembly: SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.ComponentModel.IEditableCollectionView", Scope = "member", Target = "Teraque.ViewableCollection.#System.ComponentModel.IEditableCollectionView.AddNew()")]
[assembly: SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.ComponentModel.IEditableCollectionView", Scope = "member", Target = "Teraque.ViewableCollection.#System.ComponentModel.IEditableCollectionView.CanAddNew")]
[assembly: SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.ComponentModel.IEditableCollectionView", Scope = "member", Target = "Teraque.ViewableCollection.#System.ComponentModel.IEditableCollectionView.CanCancelEdit")]
[assembly: SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.ComponentModel.IEditableCollectionView", Scope = "member", Target = "Teraque.ViewableCollection.#System.ComponentModel.IEditableCollectionView.CancelEdit()")]
[assembly: SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.ComponentModel.IEditableCollectionView", Scope = "member", Target = "Teraque.ViewableCollection.#System.ComponentModel.IEditableCollectionView.CancelNew()")]
[assembly: SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.ComponentModel.IEditableCollectionView", Scope = "member", Target = "Teraque.ViewableCollection.#System.ComponentModel.IEditableCollectionView.CanRemove")]
[assembly: SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.ComponentModel.IEditableCollectionView", Scope = "member", Target = "Teraque.ViewableCollection.#System.ComponentModel.IEditableCollectionView.CommitEdit()")]
[assembly: SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.ComponentModel.IEditableCollectionView", Scope = "member", Target = "Teraque.ViewableCollection.#System.ComponentModel.IEditableCollectionView.CommitNew()")]
[assembly: SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.ComponentModel.IEditableCollectionView", Scope = "member", Target = "Teraque.ViewableCollection.#System.ComponentModel.IEditableCollectionView.CurrentAddItem")]
[assembly: SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.ComponentModel.IEditableCollectionView", Scope = "member", Target = "Teraque.ViewableCollection.#System.ComponentModel.IEditableCollectionView.CurrentEditItem")]
[assembly: SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.ComponentModel.IEditableCollectionView", Scope = "member", Target = "Teraque.ViewableCollection.#System.ComponentModel.IEditableCollectionView.EditItem(System.Object)")]
[assembly: SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.ComponentModel.IEditableCollectionView", Scope = "member", Target = "Teraque.ViewableCollection.#System.ComponentModel.IEditableCollectionView.IsAddingNew")]
[assembly: SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.ComponentModel.IEditableCollectionView", Scope = "member", Target = "Teraque.ViewableCollection.#System.ComponentModel.IEditableCollectionView.IsEditingItem")]
[assembly: SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.ComponentModel.IEditableCollectionView", Scope = "member", Target = "Teraque.ViewableCollection.#System.ComponentModel.IEditableCollectionView.NewItemPlaceholderPosition")]
[assembly: SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.ComponentModel.NewItemPlaceholderPosition", Scope = "member", Target = "Teraque.ViewableCollection.#System.ComponentModel.IEditableCollectionView.NewItemPlaceholderPosition")]
[assembly: SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.ComponentModel.IEditableCollectionView", Scope = "member", Target = "Teraque.ViewableCollection.#System.ComponentModel.IEditableCollectionView.NewItemPlaceholderPosition")]
[assembly: SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.ComponentModel.NewItemPlaceholderPosition", Scope = "member", Target = "Teraque.ViewableCollection.#System.ComponentModel.IEditableCollectionView.NewItemPlaceholderPosition")]
[assembly: SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.ComponentModel.IEditableCollectionView", Scope = "member", Target = "Teraque.ViewableCollection.#System.ComponentModel.IEditableCollectionView.Remove(System.Object)")]
[assembly: SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.ComponentModel.IEditableCollectionView", Scope = "member", Target = "Teraque.ViewableCollection.#System.ComponentModel.IEditableCollectionView.RemoveAt(System.Int32)")]
