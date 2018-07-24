namespace Teraque
{

	using O2S.Components.PDFView4NET;
	using O2S.Components.PDFView4NET.Forms;
	using System;
	using System.Collections;
	using System.Diagnostics.CodeAnalysis;
	using System.IO;
	using System.Resources;
	using System.Windows;
	using System.Windows.Data;
	using System.Windows.Controls;
	using System.Windows.Forms.Integration;
	using System.Windows.Input;
	using System.Windows.Markup;
	using Teraque.Windows;
	using Teraque.Windows.Controls;
	using Teraque.Windows.Input;

	/// <summary>
	/// A page for displaying Portable Document Format (PDF) content.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public partial class PdfPage : ExplorerPage
	{

		/// <summary>
		/// PDF Page View conrol from O2S Systems.
		/// </summary>
		PDFPageView pdfPageView;

		/// <summary>
		/// Identifies the Source dependency property.
		/// </summary>
		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
			"Source",
			typeof(Uri),
			typeof(PdfPage),
			new FrameworkPropertyMetadata(null, PdfPage.OnSourcePropertyChanged));

		/// <summary>
		/// A host for the PDF Viewer (which only supports Windows.Forms).
		/// </summary>
		WindowsFormsHost windowsFormHost;

		/// <summary>
		/// Initializes a new instance of the PdfPage class.
		/// </summary>
		public PdfPage()
		{

			// Initialize the IDE maintained fields.
			this.InitializeComponent();

			// Initialize the object.
			this.pdfPageView = new PDFPageView();
			this.windowsFormHost = new WindowsFormsHost();
			this.windowsFormHost.Child = this.pdfPageView;
			this.Content = this.windowsFormHost;

			// Create a single document to view all the PDF content.
			this.pdfPageView.Document = new PDFDocument();
			this.pdfPageView.Document.SerialNumber = "PDFVW-6ATTA-DK6XD-A2XTO-AIQUM-3ECYE";

			// Since this is a page it is expected to load and unload from memory at any time and handle itself accordingly. These event handlers will take care of
			// binding to the frame application when loaded and unbinding when not part of the application anymore.
			this.Loaded += new RoutedEventHandler(this.OnLoaded);
			this.Unloaded += new RoutedEventHandler(this.OnUnloaded);

		}

		/// <summary>
		/// Gets or sets the uniform resource identifier (URI) of the current content.
		/// </summary>
		public Uri Source
		{
			get
			{
				return (Uri)this.GetValue(PdfPage.SourceProperty);
			}
			set
			{
				this.SetValue(PdfPage.SourceProperty, value);
			}
		}

		/// <summary>
		/// Occurs when the element is laid out, rendered, and ready for interaction.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="routedEventArgs">The event data.</param>
		void OnLoaded(Object sender, RoutedEventArgs routedEventArgs)
		{

			// This object will automatically bind itself to a parent TreeFrame when one is available.
			ExplorerFrame explorerFrame = VisualTreeExtensions.FindAncestor<ExplorerFrame>(this);
			if (explorerFrame != null)
			{

				// The children of a Frame do not automatically inherit the data context of the parent window.  This is likely due to the fact that pages are not
				// naturally kept alive when the navigation moves away.  So any data binding operation must be established or re-established when the page is
				// loaded and must be cleared when the page is unloaded.  This will bind this page to the context of the parent frame (for now).
				Binding dataContextBinding = new Binding();
				dataContextBinding.Path = new PropertyPath("DataContext");
				dataContextBinding.Source = explorerFrame;
				dataContextBinding.Mode = BindingMode.OneWay;
				BindingOperations.SetBinding(this, PdfPage.DataContextProperty, dataContextBinding);

				// The Source property binding allows a change to the property to propogate up to the container.
				Binding sourceBinding = new Binding();
				sourceBinding.Path = new PropertyPath("Source");
				sourceBinding.Source = explorerFrame;
				sourceBinding.Mode = BindingMode.TwoWay;
				BindingOperations.SetBinding(this, PdfPage.SourceProperty, sourceBinding);

			}

		}

		/// <summary>
		/// Invoked when the effective property value of the Source property changes.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that is issued by any event that tracks changes to the effective value of this property.
		/// </param>
		private static void OnSourcePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// This will disable the navigation when the source URI is changed retroactively to the page navigation.  That is, when the journal, forward button, 
			// backward button change the current page, the source URI is set after the fact in order to reflect the location of the selected page.  Conversly, 
			// when a breadcrumb control or tree view changes the source URI, that should be taken as an instruction to navigate to the selected page.
			PdfPage pdfPage = dependencyObject as PdfPage;
			Uri newSource = dependencyPropertyChangedEventArgs.NewValue as Uri;

			// When the source URI has changed we need a new data context for the new source URI.  This effectively chooses the items that appear in any one of the 
			// views provided by this class by selecting the IExplorerItem as the data context for the page.
			if (newSource != null)
			{
				IExplorerItem rootItem = pdfPage.DataContext as IExplorerItem;
				IExplorerItem iExplorerItem = ExplorerHelper.FindExplorerItem(rootItem, newSource);
				if (iExplorerItem != null && iExplorerItem.Data != null)
				{
					MemoryStream memoryStream = new MemoryStream(iExplorerItem.Data);
					memoryStream.Position = 0L;
					pdfPage.pdfPageView.PageDisplayLayout = PDFPageDisplayLayout.OneColumn;
					pdfPage.pdfPageView.Document.Load(memoryStream);
				}
			}

		}

		/// <summary>
		/// Occurs when the element is removed from within an element tree of loaded elements.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="routedEventArgs">The event data.</param>
		void OnUnloaded(Object sender, RoutedEventArgs routedEventArgs)
		{

			// When this control is unloaded from its parent container the binding to that parent must be cleared or errors will be generated before if the item isn't
			// immediately garbage collected.
			if (BindingOperations.IsDataBound(this, PdfPage.DataContextProperty))
				BindingOperations.ClearBinding(this, PdfPage.DataContextProperty);
			if (BindingOperations.IsDataBound(this, PdfPage.SourceProperty))
				BindingOperations.ClearBinding(this, PdfPage.SourceProperty);

		}

	}

}
