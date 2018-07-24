namespace Teraque.Windows.Controls
{

	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using Teraque.Windows.Input;

	/// <summary>
	/// A button used to hide or unhide the preview pane.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class PreviewButton : Gadget
	{

		/// <summary>
		/// The image source for the button when it can show the preview pane.
		/// </summary>
		static ImageSource showPreviewPaneSource = new BitmapImage(new Uri("/Teraque.PresentationFramework;component/Resources/Show Preview Pane.png", UriKind.Relative));

		/// <summary>
		/// The image source for the button when it can hide the preview pane.
		/// </summary>
		static ImageSource hidePreviewPaneSource = new BitmapImage(new Uri("/Teraque.PresentationFramework;component/Resources/Hide Preview Pane.png", UriKind.Relative));

		/// <summary>
		/// Initializes the PreviewButton class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static PreviewButton()
		{

			// This allows the templates in resource files to be properly associated with this new class.  Without this override, the type of the base class would 
			// be used as the key in any lookup involving resources dictionaries.
			PreviewButton.DefaultStyleKeyProperty.OverrideMetadata(typeof(PreviewButton), new FrameworkPropertyMetadata(typeof(Gadget)));

		}

		/// <summary>
		/// Initializes a new instance of PreviewButton class.
		/// </summary>
		public PreviewButton()
		{

			// This command will cause the state of the preview pane visibility to toggle when invoked.
			this.Command = Commands.ViewPreviewPane;

			// This allows the frame to control the visual appearance of the button.
			Binding isCheckedBinding = new Binding();
			isCheckedBinding.Path = new PropertyPath("IsPreviewVisible");
			isCheckedBinding.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(ExplorerFrame), 1);
			BindingOperations.SetBinding(this, PreviewButton.IsCheckedProperty, isCheckedBinding);

		}

		/// <summary>
		/// Called when the template's tree is generated.
		/// </summary>
		public override void OnApplyTemplate()
		{

			// This will initialize the icon based on the state of the button.
			this.Icon = new Image() { Source = this.IsChecked ? PreviewButton.hidePreviewPaneSource : PreviewButton.showPreviewPaneSource };

			// The tool tip will reflect the effects of pressing this button.
			this.ToolTip = this.IsChecked ? Properties.Resources.HidePreviewToolTip : Properties.Resources.ShowPreviewToolTip;

			// Allow the base class to complete the initialization.
			base.OnApplyTemplate();

		}

		/// <summary>
		/// Called when the IsChecked property becomes true. This method raises the Checked routed event.
		/// </summary>
		/// <param name="e">The event data for the Checked event.</param>
		protected override void OnChecked(RoutedEventArgs e)
		{

			// Checking will select the icon that allows the button to hide the preview.
			this.Icon = new Image() { Source = PreviewButton.hidePreviewPaneSource };

			// The tool tip will reflect the effects of pressing this button.
			this.ToolTip = Properties.Resources.HidePreviewToolTip;

			// Allow the base class to handle the rest of the event.
			base.OnChecked(e);

		}

		/// <summary>
		/// Called when the IsChecked property becomes false. This method raises the Unchecked routed event.
		/// </summary>
		/// <param name="e">The event data for the Unchecked event.</param>
		protected override void OnUnchecked(RoutedEventArgs e)
		{

			// Un-checking will select the icon that allows the button to show the preview.
			this.Icon = new Image() { Source = PreviewButton.showPreviewPaneSource };

			// The tool tip will reflect the effects of pressing this button.
			this.ToolTip = Properties.Resources.ShowPreviewToolTip;

			// Allow the base class to handle the rest of the event.
			base.OnUnchecked(e);

		}

	}

}
