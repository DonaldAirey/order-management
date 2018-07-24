namespace Teraque.Windows.Documents
{

	using System;
	using System.Windows;
	using System.Windows.Documents;
	using System.Windows.Media;

	/// <summary>
	/// Aligns the contents on physical device (pixel) units.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class PixelSnapper : AdornerDecorator
	{

		/// <summary>
		/// Identifies the Offset dependency property key.
		/// </summary>
		public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register(
			"Offset",
			typeof(Point),
			typeof(PixelSnapper),
			new FrameworkPropertyMetadata(new Point(), FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));

		/// <summary>
		/// Initializes the PixelSnapper class.
		/// </summary>
		public PixelSnapper()
		{

			// This event handler will calculate the screen offset to this control.
			this.LayoutUpdated += new EventHandler(this.OnLayoutUpdated);

		}

		/// <summary>
		/// Gets or set the fractional offset to this adorner in screen units.
		/// </summary>
		public Point Offset
		{
			get
			{
				return (Point)this.GetValue(PixelSnapper.OffsetProperty);
			}
			set
			{
				this.SetValue(PixelSnapper.OffsetProperty, value);
			}
		}

		/// <summary>
		/// Positions child elements and determines a size for the PixelSnapper.
		/// </summary>
		/// <param name="finalSize">The size reserved for this element by its parent.</param>
		/// <returns>The actual size needed by the element. This return value is typically the same as the value passed to finalSize.</returns>
		protected override Size ArrangeOverride(Size finalSize)
		{

			// This is where all the work is done for this class.  This places the child content on an integer boundary for the device.
			this.Child.Arrange(new Rect(this.Offset, this.Child.DesiredSize));

			// There's no need to change the size after arranging the content as the MeasureOverride has already considered the extra space needed to place the 
			// control on an even pixel boundary.
			return finalSize;

		}

		/// <summary>
		/// Measures the size required for child elements and determines a size for the PixelSnapper.
		/// </summary>
		/// <param name="constraint">A size to constrain the AdornerDecorator to.</param>
		/// <returns>A Size object representing the amount of layout space needed by the AdornerDecorator.</returns>
		protected override Size MeasureOverride(Size constraint)
		{

			// Increase the size needed for this control by the fractional space required to place it on an integer (pixel) boundary.
			constraint.Width += this.Offset.X > 0.0 ? this.Offset.X : 0.0;
			constraint.Height += this.Offset.Y > 0.0 ? this.Offset.Y : 0.0;

			// Allow the child to measure itself within this new constraint and return that value to the caller.  Adorners have only one child so that's
			// the only element that needs to be considered when calculating the size of this control.
			this.Child.Measure(constraint);
			return this.Child.DesiredSize;

		}

		/// <summary>
		/// Handles the layout of the various visual elements associated with the current Dispatcher changes.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains no event data.</param>
		private void OnLayoutUpdated(Object sender, EventArgs e)
		{

			// This Point will eventually contain the fractional offset to the child control.
			Point point = new Point();

			// The PresentationSource can be used to find the root visual of the current visual tree.
			PresentationSource presentationSource = PresentationSource.FromVisual(this);
			if (presentationSource != null)
			{

				// This is the root of the visual tree: the main window of this application.  This is not the screen root but it appears that all top-level windows
				// are manipulated on even integer boundaries, so it will do for the purpose of rounding out the pixels.
				Visual rootVisual = presentationSource.RootVisual;

				// Transform the origin of this control into the coordinates of the application window, round them to the nearest whole pixel and then transform 
				// them back into offsets for the adorner.
				point = this.TransformToAncestor(rootVisual).Transform(point);
				point.X = Math.Round(point.X);
				point.Y = Math.Round(point.Y);
				point = rootVisual.TransformToDescendant(this).Transform(point);

			}

			// This offset will move the child element to the nearest whole pixel boundary.
			this.Offset = point;

		}

	}

}
