namespace Teraque.Windows.Controls
{

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Windows;
	using System.Windows.Input;
	using System.Windows.Controls;
	using System.Windows.Controls.Primitives;
	using System.Windows.Media;
	using System.Windows.Media.Animation;
	using System.Windows.Threading;

	/// <summary>
	/// Arranges child elements into a single line that can be oriented horizontally or vertically and scrolled.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class ViewportPanel : VirtualizingPanel, IScrollInfo
	{

		/// <summary>
		/// The amount of time for the animation sequences to complete.
		/// </summary>
		Duration animationDuration;

		/// <summary>
		/// Identifies the AnimationTime dependency property.
		/// </summary>
		public static readonly DependencyProperty AnimationTimeProperty = DependencyProperty.Register(
			"AnimationTime",
			typeof(TimeSpan),
			typeof(ViewportPanel),
			new FrameworkPropertyMetadata(TimeSpan.FromMilliseconds(1000), new PropertyChangedCallback(ViewportPanel.OnAnimationTimePropertyChanged)));

		/// <summary>
		/// The average length of a slot.  Used for simulating scrolling in logical units (that is, line down, line up).
		/// </summary>
		Double averageLength = 0.0;

		/// <summary>
		/// Identifies the ViewportPanel.Left attached property.
		/// </summary>
		public static readonly DependencyProperty LeftProperty = DependencyProperty.RegisterAttached(
			"Left",
			typeof(Double),
			typeof(ViewportPanel),
			new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(ViewportPanel.OnPositioningChanged)),
			new ValidateValueCallback(ViewportPanel.IsValidCoordinate));

		/// <summary>
		/// The size of the virtual area of the panel.
		/// </summary>
		Size extent;

		/// <summary>
		/// An infinite size used to give containers as much space as they want when being measured.
		/// </summary>
		static Size infiniteSize = new Size(Double.PositiveInfinity, Double.PositiveInfinity);

		/// <summary>
		/// Indicates that a background measurement of the extent has been scheduled.
		/// </summary>
		Boolean isMeasureExtentScheduled = false;

		/// <summary>
		/// Used in the background measurment of the extent, this is the current index into the underlying collection of items.
		/// </summary>
		Int32 currentItemIndex;

		/// <summary>
		/// Used in the background measurement of the extent, this is the dummy container used to measure an item.
		/// </summary>
		ListViewItem sampleListViewItem;

		/// <summary>
		/// Used in the background measurement of the extent, this is the current location in the virtual space.
		/// </summary>
		Point location;

		/// <summary>
		/// The maximum amount of time that the background processing will block.
		/// </summary>
		static TimeSpan maxBlockingTime = TimeSpan.FromMilliseconds(100);

		/// <summary>
		/// Identifies the Orientation dependency property.
		/// </summary>
		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
			"Orientation",
			typeof(Orientation),
			typeof(ViewportPanel),
			new FrameworkPropertyMetadata(Orientation.Vertical, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(ViewportPanel.OnOrientationChanged)));

		/// <summary>
		/// The element that controls the scrolling of this panel.
		/// </summary>
		ScrollViewer scrollOwnerField;

		/// <summary>
		/// Defines the position and size of each container in the virtual space of the panel.
		/// </summary>
		List<Rect> slots = new List<Rect>();

		/// <summary>
		/// Identifies the ViewportPanel.Top attached property.
		/// </summary>
		public static readonly DependencyProperty TopProperty = DependencyProperty.RegisterAttached(
			"Top",
			typeof(Double),
			typeof(ViewportPanel),
			new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(ViewportPanel.OnPositioningChanged)),
			new ValidateValueCallback(ViewportPanel.IsValidCoordinate));

		/// <summary>
		/// Used when panning and scrolling to transform the viewport into the absolute coordinates of the panel.
		/// </summary>
		TranslateTransform translateTransform = new TranslateTransform();

		/// <summary>
		/// The offset into the viewport.
		/// </summary>
		Vector viewportOffset;

		/// <summary>
		/// The size of the viewport.
		/// </summary>
		Size viewportSize;

		/// <summary>
		/// The number of columns to scroll horizontally when the mouse wheel is used.
		/// </summary>
		const Double wheelScrollColumns = 3.0;

		/// <summary>
		/// Initializes a new instance of the ViewportPanel class.
		/// </summary>
		public ViewportPanel()
		{

			// This transform is used when scrolling or panning to translate the relative coordinates of the viewport into the absolute coordinates of the panel's
			// space.
			this.RenderTransform = this.translateTransform;

			// This initializes the animation duration to the default (1 second).
			this.animationDuration = new Duration(this.AnimationTime);

			// This will adjust the internal viewport properties when the size of the viewport changes.
			this.SizeChanged += new SizeChangedEventHandler(this.OnViewportSizeChanged);

		}

		/// <summary>
		/// Adds an item to the panel.
		/// </summary>
		/// <param name="position">The current GeneratorPosition of the item.</param>
		/// <param name="count">The number of items added.</param>
		void AddItems(GeneratorPosition position, Int32 count)
		{

			// We will need information about the collection associated with this panel in order to manage the extent and measure the new container.
			ItemCollection items = ItemsControl.GetItemsOwner(this).Items;
			Int32 itemCount = items.Count;

			// This creates a dummy container that we'll use for measuring new items.  It will be released when the background measurement task is done.
			if (this.sampleListViewItem == null)
			{
				this.sampleListViewItem = new ListViewItem();
				this.ItemContainerGenerator.PrepareItemContainer(this.sampleListViewItem);
			}

			// Measuring every item in the list can be expensive, so we're going to cheat.  If the new item belongs in the viewport, then we'll measure it and 
			// insert it in the virtual space accurately.  If it's not part of the viewport, then we're going to measure it and the extent during the idle cycles.
			// The first step is to find out what items are in the viewport.
			Int32 slotIndex = this.GetSlotIndex(this.Orientation == Orientation.Horizontal ? this.viewportOffset.X : this.viewportOffset.Y);
			Int32 firstItemIndex = Math.Min(Math.Max(slotIndex - 1, 0), itemCount - 1);
			Int32 lastItemIndex = firstItemIndex;
			Double remainingLength = this.Orientation == Orientation.Horizontal ? this.ViewportWidth : this.ViewportHeight;
			while (remainingLength > 0 && lastItemIndex < itemCount - 1)
			{
				Rect rect = this.slots[lastItemIndex++];
				remainingLength -= this.Orientation == Orientation.Horizontal ? rect.Width : rect.Height;
			}
			lastItemIndex = Math.Min(Math.Max(lastItemIndex + 1, 0), itemCount - 1);

			// This is the index of the item that is to be added to the virtual space.
			Int32 newIndex = this.ItemContainerGenerator.IndexFromGeneratorPosition(position);

			// Any item that is part of the viewport (or the first item since our cheating algorithm doesn't work on an empty space), the we'll accurately measure
			// the item and add it to the known slots.
			if ((firstItemIndex <= newIndex && newIndex <= lastItemIndex) || itemCount == 0)
			{

				// This will measure each of the new items and create slots for them in the virtual device space of the panel.
				for (Int32 counter = 0; counter < count; counter++)
				{

					// This will accurately measure the new slot needed for the new item.  Note that we're snapping to the device pixels.  There seems to be some 
					// minor benefit to snapping but it definitely makes moving items around easier if the slots are uniform.
					this.sampleListViewItem.DataContext = items[newIndex + counter];
					this.sampleListViewItem.Measure(ViewportPanel.infiniteSize);
					Rect slot = new Rect(new Point(), new Size(Math.Round(this.sampleListViewItem.DesiredSize.Width), Math.Round(this.sampleListViewItem.DesiredSize.Height)));
					this.slots.Insert(newIndex + counter, slot);

					// The extent in the non-stacking direction is the maximum length of any of the measured containers.
					if (this.Orientation == Orientation.Horizontal)
						extent.Height = Math.Max(extent.Height, slot.Height);
					else
						extent.Width = Math.Max(extent.Width, slot.Width);

				}

			}
			else
			{

				// This is where we cheat.  If the item is not visible then we're not going to try to measure it immediately.  We simply insert a slot based on the
				// last known slot size (obviously this only works after the first item has been measured).
				Rect slot = this.slots[newIndex - 1];
				for (Int32 counter = 0; counter < count; counter++)
					this.slots.Insert(newIndex + counter, new Rect(new Point(), slot.Size));

			}

			// This variable will act as a cursor for updating the slots.  We initialize it with the ending point of the last unaffected slot (or the origin of the
			// extent if we moved the first slot).
			Point cursor = new Point();
			if (newIndex != 0)
				if (this.Orientation == Orientation.Horizontal)
					cursor.X = this.slots[newIndex - 1].Right;
				else
					cursor.Y = this.slots[newIndex - 1].Bottom;

			// This will quickly run through all the slots and adjust them around the newly added item.  Since we're not dealing with any graphic objects, this
			// tight loop should run fast enough so that no one will notice it.
			for (Int32 index = newIndex; index < items.Count; index++)
			{
				Rect slot = this.slots[index];
				slot.Location = cursor;
				this.slots[index] = slot;
				if (this.Orientation == Orientation.Horizontal)
					cursor.X += slot.Width;
				else
					cursor.Y += slot.Height;
			}

			// After stacking all the measured items (and taking a guess at the unmeasured ones) we have a pretty good approximation of the absolute extent of the 
			// panel in the stacking direction.
			if (this.Orientation == Orientation.Horizontal)
				extent.Width = cursor.X;
			else
				extent.Height = cursor.Y;

			// This gives us a nice, usable value to scroll the viewport in increments of 1 line.
			this.averageLength = this.Orientation == Orientation.Horizontal ? this.extent.Width / itemCount : this.extent.Height / itemCount;

			// Notify the scroll owner that the extent has changed.  If the background task needs to make adjustments, another event will be generated when we have
			// an accurate accounting of the extent.
			if (this.IsScrolling)
				this.ScrollOwner.InvalidateScrollInfo();

			// This will seed the background task for accurately measuring the extent.  
			this.currentItemIndex = newIndex;
			this.location = new Point();
			if (newIndex != 0)
				if (this.Orientation == Orientation.Horizontal)
					this.location.X = this.slots[newIndex - 1].Right;
				else
					this.location.Y = this.slots[newIndex - 1].Bottom;

			// As an optimization, we're only going to schedule a measurement of the extent when there isn't alredy one scheduled.  The 'Add' events can come in 
			// rapid succession.  If we added, say, 1,000 items, then we'd fire off 1,000 BeginInvoke statements which start to be expensing.  Since the
			// 'MeasureExtentInBackground' is re-entrant, all we need to do is set the instance variables that control the measurement and we can hijack the
			// currently running background measurement (when there is one already running, that is).
			if (!this.isMeasureExtentScheduled)
			{
				this.isMeasureExtentScheduled = true;
				this.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, new Action(this.MeasureExtentInBackground));
			}

		}

		/// <summary>
		/// Positions child elements and determines a size for a FrameworkElement derived class.
		/// </summary>
		/// <param name="finalSize">The final area within the parent that this element should use to arrange itself and its children.</param>
		/// <returns>The actual size used.</returns>
		protected override Size ArrangeOverride(Size finalSize)
		{

			// The ItemContainerGenerator must be cast in order to make use of the 'Index from Container' function.
			ItemContainerGenerator itemContainerGenerator = this.ItemContainerGenerator as ItemContainerGenerator;

			// This is essentially the layout algorithm of a canvas.  Just arrange the child elements in the slot to which they've been assigned and use the
			// attached property, which may have been animated, for the position.
			foreach (UIElement uiElement in this.InternalChildren)
			{

				// The coordinates are taken from the container and can be animated to move the item from one position to another.
				Double left = (Double)uiElement.GetValue(ViewportPanel.LeftProperty);
				Double top = (Double)uiElement.GetValue(ViewportPanel.TopProperty);

				// The size is obtained from the slots data structure that keeps track of all the items and their current size (and position, which is used to seed
				// the left and top during the animation sequence).
				Int32 itemIndex = itemContainerGenerator.IndexFromContainer(uiElement);
				uiElement.Arrange(new Rect(new Point(left, top), this.slots[itemIndex].Size));

			}

			// The size of the control is not affected by the arrangement of the containers.
			return finalSize;

		}

		/// <summary>
		/// Generates the item at the specified index location and makes it visible.
		/// </summary>
		/// <param name="index">The index position of the item that is generated and made visible.</param>
		protected override void BringIndexIntoView(Int32 index)
		{

			// Validate the argument.
			if (index < 0 || index >= this.ItemCount)
				throw new ArgumentOutOfRangeException("index");

			// This will realize the child at the given position and call the WPF internal methods to bring it into view.
			GeneratorPosition generatorPosition = this.ItemContainerGenerator.GeneratorPositionFromIndex(index);
			using (this.ItemContainerGenerator.StartAt(generatorPosition, GeneratorDirection.Forward, true))
			{
				Boolean isNewlyRealized;
				FrameworkElement frameworkElement = this.ItemContainerGenerator.GenerateNext(out isNewlyRealized) as FrameworkElement;
				if (frameworkElement != null)
					frameworkElement.BringIntoView();
			}

		}

		/// <summary>
		/// Removes containers that are no longer part of the viewport.
		/// </summary>
		/// <param name="firstItemIndex">The first item in the viewport.</param>
		/// <param name="lastItemIndex">The last item in the viewport.</param>
		void CleanupContainers(Int32 firstItemIndex, Int32 lastItemIndex)
		{

			// This cast is required to get the index of the item based on the container.
			ItemContainerGenerator itemContainerGenerator = this.ItemContainerGenerator as ItemContainerGenerator;

			// Examine each of the child containers in the viewport and collect items that are no longer needed.
			for (Int32 containerIndex = 0; containerIndex < this.InternalChildren.Count; containerIndex++)
			{

				// Examine each container to see if it's a candiate for recycling.
				UIElement container = this.InternalChildren[containerIndex];

				// This determines if the current container is needed in the viewport.  Remember that we keep one 'phantom' item before the viewport and one 'phantom' item
				// after the viewport (and their corresponding containers) for the purpose of keyboard navigation.  Other than that, we throw all containers away that aren't
				// needed in the current viewport bounded by the firstItemIndex and the lastItemIndex.
				Int32 itemIndex = itemContainerGenerator.IndexFromContainer(container);
				if ((itemIndex < firstItemIndex || lastItemIndex < itemIndex) && !container.IsKeyboardFocusWithin && !container.HasAnimatedProperties)
				{
					this.RemoveInternalChildRange(containerIndex, 1);
					this.ItemContainerGenerator.Remove(this.ItemContainerGenerator.GeneratorPositionFromIndex(itemIndex), 1);
					containerIndex--;
				}

			}

		}

		/// <summary>
		/// Uses a binary search algorithm to locate a specific element in the ObservableCollection&lt;T&gt;.
		/// </summary>
		/// <returns>
		/// The zero-based index of the slot that corresponds to the given offset.</returns>
		public Int32 GetSlotIndex(Double offset)
		{

			// This is a standard binary search, ripped from the .NET code by Reflector.  The only real addition to the algorithm is the range check that compares
			// the item at the 'mid' index to the offset.  Since 'Double' values have slop in them (as they are real numbers), we can't test for an exact match so
			// we look for a range of a half-a-pixel in each direction when comparing the top and the bottom of the slot at the 'mid' index to the given offset.
			Int32 low = 0;
			Int32 high = this.slots.Count - 1;
			while (low <= high)
			{
				Int32 mid = low + (high - low >> 1);
				Rect midItem = this.slots[mid];
				Int32 compare = this.Orientation == Orientation.Horizontal ?
					((midItem.Right + 0.5) < offset ? -1 : (midItem.Left - 0.5) > offset ? 1 : 0) :
					((midItem.Bottom + 0.5) < offset ? -1 : (midItem.Top - 0.5) > offset ? 1 : 0);
				if (compare == 0)
					return mid;
				else
				{
					if (compare < 0)
						low = mid + 1;
					else
						high = mid - 1;
				}
			}

			// If the item isn't found using a binary search, then the completement of the 'low' variable indicates where in the list the item would go if it were
			// part of the sorted list.
			return ~low;

		}

		/// <summary>
		/// Validates a coordinate.
		/// </summary>
		/// <param name="value">The coordinate to validate.</param>
		/// <returns>true if the coordinate is valid, false otherwise.</returns>
		static Boolean IsValidCoordinate(Object value)
		{

			// This must makes sure that nothing ridiculous is given as a coordinate.  Basically it means we don't have to check for these conditions in the code.
			Double coordinate = (Double)value;
			return !Double.IsInfinity(coordinate) && !Double.IsNaN(coordinate);

		}

		/// <summary>
		/// Scrolls down within content by one logical unit.
		/// </summary>
		public virtual void LineDown()
		{
			this.SetVerticalOffset(this.VerticalOffset + (this.Orientation == Orientation.Vertical ? this.averageLength : this.averageLength * 16.0));
		}

		/// <summary>
		/// Scrolls left within content by one logical unit.
		/// </summary>
		public virtual void LineLeft()
		{
			this.SetHorizontalOffset(this.HorizontalOffset - (this.Orientation == Orientation.Horizontal ? 1.0 : 16.0));
		}

		/// <summary>
		/// Scrolls right within content by one logical unit.
		/// </summary>
		public virtual void LineRight()
		{
			this.SetHorizontalOffset(this.HorizontalOffset + (this.Orientation == Orientation.Horizontal ? 1.0 : 16.0));
		}

		/// <summary>
		/// Scrolls up within content by one logical unit.
		/// </summary>
		public virtual void LineUp()
		{
			this.SetVerticalOffset(this.VerticalOffset - (this.Orientation == Orientation.Vertical ? this.averageLength : this.averageLength * 16.0));
		}

		/// <summary>
		/// Forces content to scroll until the coordinate space of a Visual object is visible.
		/// </summary>
		/// <param name="visual">A Visual that becomes visible.</param>
		/// <param name="rectangle">A bounding rectangle that identifies the coordinate space to make visible.</param>
		/// <returns>A Rect that is visible.</returns>
		public Rect MakeVisible(Visual visual, Rect rectangle)
		{

			// Validate the parameters.
			if (visual == null)
				throw new ArgumentNullException("visual");

			// At the point where the viewport rectangle and the element rectangle are both in the device coordinates of the panel's absolute space, it is an
			// relatively simple matter to just compare the rectangles and move the offset so that the given rectangle is included in the viewport.
			Rect viewportRect = new Rect(new Point(this.viewportOffset.X, this.viewportOffset.Y), this.viewportSize);
			rectangle = visual.TransformToAncestor(this).TransformBounds(rectangle);

			// We only need to move the offset if the viewport doesn't already contain the given rectangle.
			if (!viewportRect.Contains(rectangle))
			{

				// Move the offset so that the given rectangle appears in the viewport.
				if (this.Orientation == Orientation.Horizontal)
				{
					if (rectangle.Right > viewportRect.Right)
						this.SetHorizontalOffset(rectangle.Right - viewportRect.Width);
					if (rectangle.Left < viewportRect.Left)
						this.SetHorizontalOffset(viewportRect.Left);
				}
				else
				{
					if (rectangle.Bottom > viewportRect.Bottom)
						this.SetVerticalOffset(rectangle.Bottom - viewportRect.Height);
					if (rectangle.Top < viewportRect.Top)
						this.SetVerticalOffset(rectangle.Top);
				}
			}

			// This is the rectangle translated into the panel's absolute space.
			return rectangle;

		}

		/// <summary>
		/// Calculate the metrics of the virtual space of the panel.
		/// </summary>
		void MeasureExtent()
		{

			// Unlike the WPF StackPanel, this panel uses device units instead of logical units (logical units means, basically, one unit for every item in the
			// underlying list).  The device units are essential for animation.  This presents a problem, however, because the number of device units can't be known
			// immediately whereas the logical units (the number of items in the underlying list) is.  A large, virtual report can take several seconds to measure
			// the device units properly, so we're going to do it improperly.  We're going to take an initial 'guess' at the height of each item in the panel, then
			// in the background we'll refine that guess until the extent is an exact measurement of the device units in the virtual space. The 'slots' data
			// structure keeps track of the location of each item in the ItemsControl associated with this panel.
			this.slots.Clear();

			// Measuring the extent only makes sense when we have items to measure.  
			ItemsControl itemsControl = ItemsControl.GetItemsOwner(this);
			Int32 itemCount = itemsControl.Items.Count;
			if (itemsControl == null || itemCount == 0)
				return;

			// Measuring the extent is done in several passes in what passes for multithreading in the foreground thread.  In order to prevent the main thread from
			// blocking while we measure the extent, we'll measure a part of the virtual space until a predefined amount of time has expired; enough time to do a
			// significant amount of work, but not enough time for the user to notice the blocking.  We'll then return control to the main dispatcher and schedule
			// another slice of time to complete the measurment.  These instance fields keep track of the progress and state of the virtual extent measurement.
			this.currentItemIndex = 0;

			// This instance field will keep track of the location of the slot in the absolute device space of the panel.
			this.location = new Point();

			// This will keep track of the total size of the virtual area of the panel (the extent).
			this.extent = new Size();

			// Rather than use the ItemsContainerGenerator to create a container for every item and then measure it, we're going to cheat a little.  We're going to
			// create a single container, bind the item to it, and then measure it.  This yields the same value as if we realized the item and measured it during the
			// MeasureOverride processing, but is significantly faster.
			this.sampleListViewItem = new ListViewItem();
			this.ItemContainerGenerator.PrepareItemContainer(this.sampleListViewItem);

			// This will cycle through all the items in the ItemsControl and measure it using the dummy container.
			while (this.currentItemIndex < itemCount)
			{

				// By associating the dummy container with the item we can get an accurate measurement of the space used by the item in the current slot.
				this.sampleListViewItem.DataContext = itemsControl.Items[this.currentItemIndex];
				this.sampleListViewItem.Measure(ViewportPanel.infiniteSize);

				// The slots are going to be snapped to the device units.  This is a little extra work up front, but it makes the math a little easier when moving
				// the slots around.
				Rect slot = new Rect(this.location, new Size(Math.Round(this.sampleListViewItem.DesiredSize.Width), Math.Round(this.sampleListViewItem.DesiredSize.Height)));
				this.slots.Add(slot);

				// This 'cursor' keeps track of where we are as we fill the stack in with items.
				this.location = this.Orientation == Orientation.Horizontal ?
					new Point(this.location.X + slot.Width, 0) :
					new Point(0, this.location.Y + slot.Height);

				// The extent in the non-stacking direction is the maximum length of any of the measured containers.
				if (this.Orientation == Orientation.Horizontal)
					extent.Height = Math.Max(extent.Height, slot.Height);
				else
					extent.Width = Math.Max(extent.Width, slot.Width);

				// We'll move on to the next item in the control until...
				this.currentItemIndex++;

				// This is the part where we cheat.  Instead of measuring the entire list in the ItemsControl, we're going to measure just the first viewport.  
				// After that, we're going to guess at the extent of the window by assuming that the last item measured represents all the remaining items.  This
				// guess is good enough for 99% of the cases and gives us quick visual feedback that something has happened.
				if (this.Orientation == Orientation.Horizontal)
				{
					if (this.location.X > this.ViewportWidth)
						break;
				}
				else
				{
					if (this.location.Y > this.ViewportHeight)
						break;
				}

			}

			// From this point on, we're going to guess at the extent by assuming that the last container measured represents all the remaining containers.
			Point guessedLocation = this.location;
			for (Int32 index = this.currentItemIndex; index < itemCount; index++)
			{

				// This assumes that the previous slot is a good enough guess for this unmeasured slot.
				Rect slot = new Rect(guessedLocation, this.slots[index - 1].Size);
				this.slots.Add(slot);

				// This 'cursor' keeps track of where we are as we fill the stack in with items.
				guessedLocation = this.Orientation == Orientation.Horizontal ?
					new Point(guessedLocation.X + slot.Width, 0.0) :
					new Point(0.0, guessedLocation.Y + slot.Height);

			}

			// After stacking all the measured items (and taking a guess at the unmeasured ones) we have the absolute extent of the panel in the stacking direction.
			if (this.Orientation == Orientation.Horizontal)
				extent.Width = guessedLocation.X;
			else
				extent.Height = guessedLocation.Y;

			// This gives us a nice, usable value to scroll the viewport in increments of 1 line.
			this.averageLength = this.Orientation == Orientation.Horizontal ? this.extent.Width / itemCount : this.extent.Height / itemCount;

			// Notify the scroll owner that the extent has changed.
			if (this.IsScrolling)
				this.ScrollOwner.InvalidateScrollInfo();

			// This will complete the job of measuring the containers in the background.  Note that we don't schedule a background measurement when one has already
			// been scheduled.  If a previous event caused a background measurement, then we will hijack that job.  The measurement task is re-entrant, so as long as the 
			// instance variables that control the measurement have been set to the desired values, we can use the already scheduled job.  This is intended to 
			// optimize the use of the dispatcher, especially during high frequency updates.
			if (this.currentItemIndex < itemCount && !this.isMeasureExtentScheduled)
			{
				this.isMeasureExtentScheduled = true;
				this.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, new Action(this.MeasureExtentInBackground));
			}

		}

		/// <summary>
		/// Calculate the metrics of the virtual space of the panel in a background process.
		/// </summary>
		void MeasureExtentInBackground()
		{

			// These are the items to be measured.
			ItemsControl itemsControl = ItemsControl.GetItemsOwner(this);
			Int32 itemCount = itemsControl.Items.Count;

			// This is a background process (inasmuch as it occurs when WPF is not handling anything else).  In order to prevent it from blocking the foreground 
			// thread for too long, we'll keep track of the time.  When a predefined span of time has passed, we'll return control to the main thread and wait for
			// the system to be idle again before continuing the measurement.
			DateTime startTime = DateTime.Now;

			// This will cycle through all the items in the ItemsControl and measure it using the dummy container.
			while (this.currentItemIndex < itemCount)
			{

				// By associating the dummy container with the item we can get an accurate measurement of the space used by the item in the current slot.
				this.sampleListViewItem.DataContext = itemsControl.Items[this.currentItemIndex];
				this.sampleListViewItem.Measure(ViewportPanel.infiniteSize);

				// This is where we overwrite the guess with the actual measurement of the container.  Note that we're snapping the slot to the device units.  This 
				// appears to be more efficient for the renderer.  It also makes easier to move items around because it forces the size of the slots to  be 
				// uniform.  There is a 'quirk' with the measuring system that causes rounding issues with the container size when it is measured in different 
				// passes and this seems be a cure for that eccentricity.
				Rect slot = new Rect(this.location, new Size(Math.Round(this.sampleListViewItem.DesiredSize.Width), Math.Round(this.sampleListViewItem.DesiredSize.Height)));
				this.slots[this.currentItemIndex] = slot;

				// This 'cursor' keeps track of where we are as we fill the stack in with items.
				this.location = this.Orientation == Orientation.Horizontal ?
					new Point(this.location.X + slot.Width, 0) :
					new Point(0, this.location.Y + slot.Height);

				// The extent in the non-stacking direction is the maximum length of any of the measured containers.
				if (this.Orientation == Orientation.Horizontal)
					extent.Height = Math.Max(extent.Height, slot.Height);
				else
					extent.Width = Math.Max(extent.Width, slot.Width);

				// We'll move on to the next item in the control until...
				this.currentItemIndex++;

				// This is a foreground thread.  In order to keep this tight measurement loop from blocking for too long, we've selected a span that is long enough
				// to get some solid work done, but not long enough for the user to notice that the foreground has stopped responding.
				if (DateTime.Now.Subtract(startTime) > ViewportPanel.maxBlockingTime)
					break;

			}

			// At this point, we've either completed measuring the extent, or need to surrender control to keep this job from blocking for too long.  If the latter,
			// then schedule another pass at measuring.  If the former, then make the final adjustments to the scrolling properties.
			if (this.currentItemIndex < itemCount)
				this.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, new Action(this.MeasureExtentInBackground));
			else
			{

				// This indicates that we're done with the background measurement.
				this.isMeasureExtentScheduled = false;

				// This will release our dummy container so that it can be reclaimed by the garbage collector.
				this.sampleListViewItem = null;

				// After stacking all the items in the virtual space we finally have the absolute extent of the panel in the stacking direction.  This is no longer
				// a guess, but an exact measurement of the extent.
				if (this.Orientation == Orientation.Horizontal)
					extent.Width = this.location.X;
				else
					extent.Height = this.location.Y;

				// This gives us a nice, usable value to scroll the viewport in increments of 1 line.
				this.averageLength = this.Orientation == Orientation.Horizontal ?
					this.extent.Width / itemCount :
					this.extent.Height / itemCount;

				// Notify the scroll owner that the extent has changed.
				if (this.IsScrolling)
					this.ScrollOwner.InvalidateScrollInfo();

			}

		}

		/// <summary>
		/// Measures the size in layout required for child elements and determines a size for the FrameworkElement-derived class. 
		/// </summary>
		/// <param name="availableSize">
		/// The available size that this element can give to child elements. Infinity can be specified as a value to indicate that the element will size to whatever
		/// content is available.
		/// </param>
		/// <returns>The size that this element determines it needs during layout, based on its calculations of child element sizes.</returns>
		protected override Size MeasureOverride(Size availableSize)
		{

			// The number of items in the underlying collection.
			Int32 itemCount = this.ItemCount;

			// If there are no items then there is nothing to measure.
			if (itemCount != 0)
			{

				// The main idea here is to calculate the index of the first item that appears in the viewport.  This is not necessarily the first one we see 
				// because a phantom item is kept before (and after) the viewport to allow for keyboard navigation.  Any given container can use the keyboard to
				// navigate to the next container so long as it is realized.  When we scroll off the screen, we'll pull the phantom control into the viewport and
				// then realize the next control after it.  In this way we can use the keyboard to navigate through a virtual space.  Note also that the index is
				// bounded by the number of items in the list.
				Int32 slotIndex = this.GetSlotIndex(this.Orientation == Orientation.Horizontal ? this.viewportOffset.X : this.viewportOffset.Y);
				Int32 firstItemIndex = Math.Min(Math.Max(slotIndex - 1, 0), itemCount - 1);

				// The last item also includes an extra 'phantom' item (when available) for keyboard navigation.  This item doesn't appear in the viewport but is 
				// realized so the keyboard navigation can bring the item into view when the user navigates downward.
				Int32 lastItemIndex = firstItemIndex;
				Double remainingLength = this.Orientation == Orientation.Horizontal ? availableSize.Width : availableSize.Height;
				while (remainingLength > 0 && lastItemIndex < itemCount - 1)
				{
					Rect rect = this.slots[lastItemIndex++];
					remainingLength -= this.Orientation == Orientation.Horizontal ? rect.Width : rect.Height;
				}
				lastItemIndex = Math.Min(Math.Max(lastItemIndex + 1, 0), itemCount - 1);

				// This will remove any of the containers that are no longer needed in this viewport.  When used with the 'ListViewport', these containers will be
				// recycled immediately as we scroll items into the viewer.
				this.CleanupContainers(firstItemIndex, lastItemIndex);

				// This is the starting point for generating containers for this viewport.
				GeneratorPosition generatorPosition = this.ItemContainerGenerator.GeneratorPositionFromIndex(firstItemIndex);

				// The main idea here is to generate containers for the viewport.  If an item from the underlying collection is not realized yet, we'll generate a
				// container for it. If it is realized, then we make sure it takes the right position in the viewport.
				Int32 containerIndex = 0;
				using (this.ItemContainerGenerator.StartAt(generatorPosition, GeneratorDirection.Forward, true))
					for (Int32 itemIndex = firstItemIndex; itemIndex <= lastItemIndex; itemIndex++)
					{

						// This is the slot assigned to the current item.
						Rect slot = this.slots[itemIndex];

						// Generate a container for the current item.  The 'newlyRealized' tells us whether this is a new container or one that's still part of the
						// viewport.
						Boolean newlyRealized = false;
						UIElement container = this.ItemContainerGenerator.GenerateNext(out newlyRealized) as UIElement;

						// If the container is not already part of this viewport, then insert it in the list of child containers.  Otherwise we'll assume that the
						// item is already visible in our Viewport.
						if (newlyRealized)
						{

							// This will add the newly realized container to the panel and ask the items control to prepare it for use in this window.
							this.AddInternalChild(container);
							this.ItemContainerGenerator.PrepareItemContainer(container);

							// In order to animate the item's movement from the last known slot position to its current position, we need to know the previous
							// position.  In order to know the previous position of the item, we need the item.  Then we make use of the table of previous 
							// positions.  If a previous position is found, it is used as the starting position of the animation to move this item to it's proper
							// slot.  If no previous position is found, we'll use the assigned slot for this container's position.  Note that we remove the 
							// previous position once it's been used.  You can only have a previous position once.  After that it becomes your current position.
							// That is, after the first animation to move it into position, the value in the 'slots' table represents the assigned slot for this 
							// container.
							container.SetValue(ViewportPanel.LeftProperty, slot.Left);
							container.SetValue(ViewportPanel.TopProperty, slot.Top);

						}

						// The amount of space that any container can occupy is set when the collection is changed.  However, calling this will allow the container
						// to realize any items and measure them based on the new DatContext it obtained through the ItemContainerGenerator.
						container.Measure(ViewportPanel.infiniteSize);

						// The items may be animated already, on their way 
						Double left = (Double)container.GetValue(ViewportPanel.LeftProperty);
						Double top = (Double)container.GetValue(ViewportPanel.TopProperty);

						// If the item is not where it is supposed to be horizontally or vertically, then move it.
						if (slot.Left != left)
							this.MoveContainerHorizontally(container, left, slot.Left);
						if (slot.Top != top)
							this.MoveContainerVertically(container, top, slot.Top);

						// Move up to the next container in the viewport.
						containerIndex++;

					}

			}

			// The size of the viewport is fixed by its parent.
			return availableSize;

		}

		/// <summary>
		/// Scrolls down within content after a user clicks the wheel button on a mouse.
		/// </summary>
		public virtual void MouseWheelDown()
		{
			this.SetVerticalOffset(this.VerticalOffset + SystemParameters.WheelScrollLines * (this.Orientation == Orientation.Vertical ? 1.0 : 16.0));
		}

		/// <summary>
		/// Scrolls left within content after a user clicks the wheel button on a mouse.
		/// </summary>
		public virtual void MouseWheelLeft()
		{
			this.SetHorizontalOffset(this.HorizontalOffset - ViewportPanel.wheelScrollColumns * (this.Orientation == Orientation.Horizontal ? 1.0 : 16.0));
		}

		/// <summary>
		/// Scrolls right within content after a user clicks the wheel button on a mouse.
		/// </summary>
		public virtual void MouseWheelRight()
		{
			this.SetHorizontalOffset(this.HorizontalOffset + ViewportPanel.wheelScrollColumns * (this.Orientation == Orientation.Horizontal ? 1.0 : 16.0));
		}

		/// <summary>
		/// Scrolls up within content after a user clicks the wheel button on a mouse.
		/// </summary>
		public virtual void MouseWheelUp()
		{
			this.SetVerticalOffset(this.VerticalOffset - SystemParameters.WheelScrollLines * (this.Orientation == Orientation.Vertical ? 1.0 : 16.0));
		}

		/// <summary>
		/// Animates the motion of the container horizontally from one location to another.
		/// </summary>
		/// <param name="container">The container to be moved.</param>
		/// <param name="from">The starting position of the animation.</param>
		/// <param name="to">The ending position of the animation.</param>
		void MoveContainerHorizontally(UIElement container, Double from, Double to)
		{

			// This creates the animation that moves the container from its current slot to the new slot.  Note that we don't attempt to compose the animations, one
			// is enough for the right effect.  Also note that when the animation is done we don't attempt to hold the value. The property will revert back to it's
			// normal value.  This is important because we only want to use the timer long enough to move the container and then release it so we're not holding
			// resources indefinitely.  Also note that we have a completion method that will remove the timer explicitly.
			DoubleAnimation doubleAnimation = new DoubleAnimation();
			doubleAnimation.Completed += this.OnAnimationCompleted;
			doubleAnimation.Duration = this.animationDuration;
			doubleAnimation.FillBehavior = FillBehavior.Stop;
			doubleAnimation.From = from;
			doubleAnimation.To = to;
			Timeline.SetDesiredFrameRate(doubleAnimation, 30);
			container.BeginAnimation(ViewportPanel.LeftProperty, doubleAnimation, HandoffBehavior.SnapshotAndReplace);

			// When the animation stops, we want the container to reside here.
			container.SetValue(ViewportPanel.LeftProperty, to);

		}

		/// <summary>
		/// Animates the motion of the container vertically from one location to another.
		/// </summary>
		/// <param name="container">The container to be moved.</param>
		/// <param name="from">The starting position of the animation.</param>
		/// <param name="to">The ending position of the animation.</param>
		void MoveContainerVertically(UIElement container, Double from, Double to)
		{

			// This creates the animation that moves the container from its current slot to the new slot.  Note that we don't attempt to compose the animations, one
			// is enough for the right effect.  Also note that when the animation is done we don't attempt to hold the value. The property will revert back to it's
			// normal value.  This is important because we only want to use the timer long enough to move the container and then release it so we're not holding
			// resources indefinitely.  Also note that we have a completion method that will remove the timer explicitly.
			DoubleAnimation doubleAnimation = new DoubleAnimation();
			doubleAnimation.Completed += this.OnAnimationCompleted;
			doubleAnimation.Duration = this.animationDuration;
			doubleAnimation.FillBehavior = FillBehavior.Stop;
			doubleAnimation.From = from;
			doubleAnimation.To = to;
			Timeline.SetDesiredFrameRate(doubleAnimation, 30);
			container.BeginAnimation(ViewportPanel.TopProperty, doubleAnimation, HandoffBehavior.SnapshotAndReplace);

			// When the animation stops, we want the container to reside here.
			container.SetValue(ViewportPanel.TopProperty, to);

		}

		/// <summary>
		/// Move an item from one position to another.
		/// </summary>
		/// <param name="oldPosition">The previous GeneratorPosition of the item.</param>
		/// <param name="position">The current GeneratorPosition of the item.</param>
		/// <param name="count">The number of items moved.</param>
		void MoveItems(GeneratorPosition oldPosition, GeneratorPosition position, Int32 count)
		{

			// Unfortunately the ItemContainerGenerator has destroyed the relationship between the index of the removed item and the container so we'll have to use
			// the shotgun approach and check each container to see if it's still a generated item.  This will purge the panel of any item the ItemContainerGenerator
			// no longer recognizes.
			ItemContainerGenerator itemContainerGenerator = ((ItemContainerGenerator)this.ItemContainerGenerator);
			for (Int32 index = 0; index < this.InternalChildren.Count; index++)
			{
				ListViewItem listViewItem = this.InternalChildren[index] as ListViewItem;
				if (itemContainerGenerator.IndexFromContainer(listViewItem) == -1)
				{

					// This container is no longer recognized by the generator, we need to dispose of it after cleaning it up.
					listViewItem.BeginAnimation(ViewportPanel.LeftProperty, null);
					listViewItem.BeginAnimation(ViewportPanel.TopProperty, null);
					this.RemoveInternalChildRange(index, 1);

					// The iterator needs to be adjusted to account for the deleted container.
					index--;

				}
			}

			// Calculate the old and new index of the moved item.  Note that the moved item is always unrealized as it has been reclaimed by the
			// ItemControlGenerator by the time we get this notification.  If we want it realized, we'll have to do it ourself.
			Int32 oldIndex = oldPosition.Index + oldPosition.Offset;
			Int32 newIndex = this.ItemContainerGenerator.IndexFromGeneratorPosition(position);

			// This will move the slots around as efficiently as possible.  If the slots being moved are the same size, then there is no practical good that can
			// come from measuring the slots again.  However, if they aren't the same size, then all the slots between the old index and the new index will need to
			// be measured again.
			Boolean isMeasureRequired = false;

			// In order to move an item, we need to know the old position as well as the new position.  This array keeps track of the old position.
			Rect[] oldSlots = new Rect[count];

			// For ever item that is being moved (generally there is only one, but there's no guarantee there's only one), we're going to reorder the slots.  If any
			// of the slots have changed size, then we need to measure the slots again between the modified slots.
			for (Int32 counter = 0; counter < count; counter++)
			{
				oldSlots[counter] = this.slots[oldIndex + counter];
				Rect newSlot = this.slots[newIndex + counter];
				if (oldSlots[counter].Size != newSlot.Size)
				{
					this.slots.RemoveAt(oldIndex);
					this.slots.Insert(newIndex, oldSlots[counter]);
					isMeasureRequired = true;
				}
			}

			// Most of the time the slots will be uniform, so moving one slot to another won't require measuring the slots again.  On those occations where the
			// slots are not uniform, we need to calculate the offsets of the slots again after they've moved.
			if (isMeasureRequired)
			{

				// When a slot has been moved, we only need to measure the area from where it was removed to where it was added.  All the other slots in this stack
				// will be unmodified by a move.  This sets up a loop to change only that part of the stack that was affected by the move.
				Int32 minIndex = Math.Min(oldIndex, newIndex);
				Int32 maxIndex = Math.Max(oldIndex, newIndex);

				// This variable will act as a cursor for updating the slots.  We initialize it with the ending point of the last unaffected slot (or the origin of 
				// the extent if we moved the first slot).
				Point cursor = new Point();
				if (minIndex != 0)
					if (this.Orientation == Orientation.Horizontal)
						cursor.X = this.slots[minIndex - 1].Right;
					else
						cursor.Y = this.slots[minIndex - 1].Bottom;

				// Now we can iterate through all the affected slots updating their absolute offset.
				for (Int32 index = minIndex; index <= maxIndex + count - 1; index++)
				{
					Rect slot = this.slots[index];
					slot.Location = cursor;
					this.slots[index] = slot;
					if (this.Orientation == Orientation.Horizontal)
						cursor.X += slot.Width;
					else
						cursor.Y += slot.Height;
				}
			}

			// Use the ItemContainerGenerator to realize the items that have moved and animate them to move to their new slot.
			using (this.ItemContainerGenerator.StartAt(position, GeneratorDirection.Forward, true))
			{

				// For each item that is moved (generally there is only one, but there's no guarantee of only one in the documention), realize the container and
				// move it to its new position.
				for (Int32 counter = 0; counter < count; counter++)
				{

					// Realize the moved item and add it to the panel.  Note that we don't check to see if it's newly realized; moved items area always newly
					// realized.
					Boolean newlyRealized = false;
					UIElement container = this.ItemContainerGenerator.GenerateNext(out newlyRealized) as UIElement;
					this.AddInternalChild(container);
					this.ItemContainerGenerator.PrepareItemContainer(container);

					// The position of this newly realized container is the same as the one it replaced.  No one should ever be aware that one control disappeared
					// and another that looks exactly like it appeared on the same spot.
					container.SetValue(ViewportPanel.LeftProperty, oldSlots[counter].Left);
					container.SetValue(ViewportPanel.TopProperty, oldSlots[counter].Top);

					// This allows the new container to measure itself and its children.
					container.Measure(ViewportPanel.infiniteSize);

					// This is the slot where the item is to be moved.
					Rect oldSlot = oldSlots[counter];
					Rect newSlot = this.slots[newIndex + counter];

					// If the item is not where it is supposed to be horizontally or vertically, then move it.
					if (newSlot.Left != oldSlot.Left)
						this.MoveContainerHorizontally(container, oldSlot.Left, newSlot.Left);
					if (newSlot.Top != oldSlots[counter].Top)
						this.MoveContainerVertically(container, oldSlot.Top, newSlot.Top);

				}

			}

		}

		/// <summary>
		/// Handles the completion of an animation clock.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="eventArgs">The event data.</param>
		void OnAnimationCompleted(Object sender, EventArgs eventArgs)
		{

			// When the animation is complete the clock is removed from the properties it animates.
			AnimationClock animationClock = sender as AnimationClock;
			animationClock.Controller.Remove();

			// Forcing the panel to measure itself will release any animated containers that are no longer part of the viewport.
			this.InvalidateMeasure();

		}

		/// <summary>
		/// Sets the animation duration for the panel.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">
		/// Event data that is issued by any event that tracks changes to the effective value of this property.
		/// </param>
		static void OnAnimationTimePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// The time it takes to peform an animation sequence is specified in TimeSpan units but the animation clocks work in Duration.  This will provide the
			// conversion.
			ViewportPanel viewportPanel = dependencyObject as ViewportPanel;
			TimeSpan timeSpan = (TimeSpan)dependencyPropertyChangedEventArgs.NewValue;
			viewportPanel.animationDuration = new Duration(timeSpan);

		}

		/// <summary>
		/// Supports layout behavior when a child element is resized. 
		/// </summary>
		/// <param name="child">The child element that is being resized.</param>
		protected override void OnChildDesiredSizeChanged(UIElement child)
		{

			// When the size of any of the children has changed we need to measure the extent again.
			this.MeasureExtent();

			// Calling the base class method will effectively force a MeasureOverride.
			base.OnChildDesiredSizeChanged(child);

		}

		/// <summary>
		/// Called when the Items collection that is associated with the ItemsControl for this Panel changes.
		/// </summary>
		/// <param name="sender">The Object that raised the event.</param>
		/// <param name="args">Provides data for the ItemsChanged event.</param>
		protected override void OnItemsChanged(Object sender, ItemsChangedEventArgs args)
		{

			// Validate the parameters.
			if (args == null)
				throw new ArgumentNullException("args");

			// This will handle a change to the collection of items that is presented in this panel.
			switch (args.Action)
			{

			case NotifyCollectionChangedAction.Add:

				// Insert an item into the virtual space.
				this.AddItems(args.Position, args.ItemCount);
				break;

			case NotifyCollectionChangedAction.Move:

				// Move an item from one slot to another.
				this.MoveItems(args.OldPosition, args.Position, args.ItemCount);
				break;

			case NotifyCollectionChangedAction.Reset:

				// When the item collection is reset, we need to remeasure the extent of this panel.
				this.MeasureExtent();
				break;

			case NotifyCollectionChangedAction.Remove:

				// Remove an item from the virtual space.
				this.RemoveItems(args.Position, args.ItemCount);
				break;

			}

		}

		/// <summary>
		/// Occurs when the position of a container changes.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">
		/// Event data that is issued by any event that tracks changes to the effective value of this property.
		/// </param>
		static void OnPositioningChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// This will invalidate the panel when a child within it changes its position.  It is ripped off from the Canvas and provides the ability to animate the
			// containers as they move from one slot to another.
			UIElement reference = dependencyObject as UIElement;
			if (reference != null)
			{
				ViewportPanel parent = VisualTreeHelper.GetParent(reference) as ViewportPanel;
				if (parent != null)
					parent.InvalidateArrange();
			}

		}

		/// <summary>
		/// Occurs when the orientation of the panel has changed.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">
		/// Event data that is issued by any event that tracks changes to the effective value of this property.
		/// </param>
		static void OnOrientationChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// Reset the scrolling information for this panel and lay it out again.
			ViewportPanel viewportStackPanel = dependencyObject as ViewportPanel;
			viewportStackPanel.ResetScrolling();

		}

		/// <summary>
		/// Occurs when the scrolling properties have changed.
		/// </summary>
		void OnScrollChange()
		{

			// Notify the owner of the scrolling controls that the information regarding the viewport is no longer valid.
			if (this.IsScrolling)
				this.ScrollOwner.InvalidateScrollInfo();

		}

		/// <summary>
		/// Handles the FrameworkElement.SizeChanged routed event.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="e">The event data.</param>
		void OnViewportSizeChanged(object sender, SizeChangedEventArgs e)
		{

			// Update the viewport size and notify any descendants of the change.
			Size oldViewportSize = this.viewportSize;
			this.viewportSize = e.NewSize;
			this.OnViewportSizeChanged(oldViewportSize, this.viewportSize);

			// This will determine if the offset has changed as a result of the new viewport size (we will fill in the bottom of the screen with containers if growing the
			// viewport in the stacking direction allows more items to be displayed).  If it has, we'll set the respective offsets and provide any descendants with
			// notification of the change.
			Vector newViewportOffset = new Vector(
				Math.Max(Math.Min(this.viewportOffset.X, this.extent.Width - this.viewportSize.Width), 0.0),
				Math.Max(Math.Min(this.viewportOffset.Y, this.extent.Height - this.viewportSize.Height), 0.0));
			if (this.viewportOffset != newViewportOffset)
			{
				Vector oldViewportOffset = this.viewportOffset;
				this.SetHorizontalOffset(newViewportOffset.X);
				this.SetVerticalOffset(newViewportOffset.Y);
				this.viewportOffset = newViewportOffset;
				this.OnViewportOffsetChanged(oldViewportOffset, this.viewportOffset);
			}

			// This will adjust the scrollbars to reflect the new properties of the viewport.
			this.OnScrollChange();

		}

		/// <summary>
		/// Occurs when the Viewport offset has changed.
		/// </summary>
		/// <param name="oldViewportOffset">The previous Viewport offset.</param>
		/// <param name="newViewportOffset">The current Viewport offset.</param>
		protected virtual void OnViewportOffsetChanged(Vector oldViewportOffset, Vector newViewportOffset) { }

		/// <summary>
		/// Occurs when the Viewport size has changed.
		/// </summary>
		/// <param name="oldViewportSize">The previous Viewport size.</param>
		/// <param name="newViewportSize">The current Viewport size.</param>
		protected virtual void OnViewportSizeChanged(Size oldViewportSize, Size newViewportSize) { }

		/// <summary>
		/// Invoked when the VisualCollection of a visual object is modified.
		/// </summary>
		/// <param name="visualAdded">The Visual that was added to the collection.</param>
		/// <param name="visualRemoved">The Visual that was removed from the collection.</param>
		protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
		{

			// This will release the animators used to move the containers into their slots in the virtual space.  When these children are released, they'll be 
			// reclaimed by a recycler, so any managed resources assigned to them will hang around indefinitely.  This is our last chance to dispose of any
			// resources created for the containers by this panel.
			UIElement uiElementRemoved = visualRemoved as UIElement;
			if (uiElementRemoved != null && uiElementRemoved.HasAnimatedProperties)
			{
				uiElementRemoved.BeginAnimation(ViewportPanel.LeftProperty, null);
				uiElementRemoved.BeginAnimation(ViewportPanel.TopProperty, null);
			}

			// Allow the base class to handle the rest of the event.
			base.OnVisualChildrenChanged(visualAdded, visualRemoved);

		}

		/// <summary>
		/// Scrolls down within content by one page.
		/// </summary>
		public virtual void PageDown()
		{
			this.SetVerticalOffset(this.VerticalOffset + this.ViewportHeight);
		}

		/// <summary>
		/// Scrolls left within content by one page.
		/// </summary>
		public virtual void PageLeft()
		{
			this.SetHorizontalOffset(this.HorizontalOffset - this.ViewportWidth);
		}

		/// <summary>
		/// Scrolls right within content by one page.
		/// </summary>
		public virtual void PageRight()
		{
			this.SetHorizontalOffset(this.HorizontalOffset + this.ViewportWidth);
		}

		/// <summary>
		/// Scrolls up within content by one page.
		/// </summary>
		public virtual void PageUp()
		{
			this.SetVerticalOffset(this.VerticalOffset - this.ViewportHeight);
		}

		/// <summary>
		/// Remove one or more items from the panel.
		/// </summary>
		/// <param name="position">The current GeneratorPosition of the item.</param>
		/// <param name="count">The number of items added.</param>
		void RemoveItems(GeneratorPosition position, Int32 count)
		{

			// Unfortunately the ItemContainerGenerator has destroyed the relationship between the index of the removed item and the container so we'll have to use
			// the shotgun approach and check each container to see if it's still a generated item.  This will purge the panel of any item the ItemContainerGenerator
			// no longer recognizes.
			ItemContainerGenerator itemContainerGenerator = ((ItemContainerGenerator)this.ItemContainerGenerator);
			for (Int32 index = 0; index < this.InternalChildren.Count; index++)
			{
				ListViewItem listViewItem = this.InternalChildren[index] as ListViewItem;
				if (itemContainerGenerator.IndexFromContainer(listViewItem) == -1)
				{

					// This container is no longer recognized by the generator, we need to dispose of it after cleaning it up.
					listViewItem.BeginAnimation(ViewportPanel.LeftProperty, null);
					listViewItem.BeginAnimation(ViewportPanel.TopProperty, null);
					this.RemoveInternalChildRange(index, 1);

					// The iterator needs to be adjusted to account for the deleted container.
					index--;

				}
			}

			// This will remove the slot of every item that was removed.
			Int32 newItemIndex = position.Index + position.Offset;
			for (Int32 counter = 0; counter < count; counter++)
				this.slots.RemoveAt(newItemIndex + counter);

			// Once the slot for the item has been removed we need to recalcualte the starting position of all the remaining slots.  This is a tight loop but it
			// should be very fast even for very large collections.
			Int32 itemCount = this.ItemCount;
			Point cursor = new Point();
			if (newItemIndex != 0)
				if (this.Orientation == Orientation.Horizontal)
					cursor.X = this.slots[newItemIndex - 1].Right;
				else
					cursor.Y = this.slots[newItemIndex - 1].Bottom;
			for (Int32 itemIndex = newItemIndex; itemIndex < itemCount; itemIndex++)
			{
				Rect slot = this.slots[itemIndex];
				slot.Location = cursor;
				this.slots[itemIndex] = slot;
				if (this.Orientation == Orientation.Horizontal)
					cursor.X += slot.Width;
				else
					cursor.Y += slot.Height;
			}

			// After removing an item and recalculating the slots we can remeasure the extent.
			if (this.Orientation == Orientation.Horizontal)
				extent.Width = cursor.X;
			else
				extent.Height = cursor.Y;

			// This gives us a nice, usable value to scroll the viewport in increments of 1 line.
			this.averageLength = this.Orientation == Orientation.Horizontal ? this.extent.Width / itemCount : this.extent.Height / itemCount;

			// Notify the scroll owner that the extent has changed.  If the background task needs to make adjustments, another event will be generated when we have
			// an accurate accounting of the extent.
			if (this.IsScrolling)
				this.ScrollOwner.InvalidateScrollInfo();

		}

		/// <summary>
		/// Resets the scrolling properties.
		/// </summary>
		void ResetScrolling()
		{

			// Will measure the panel again using the new properties and items.
			this.MeasureExtent();
			this.InvalidateMeasure();

		}

		/// <summary>
		/// Sets the amount of horizontal offset.
		/// </summary>
		/// <param name="offset">The degree to which content is horizontally offset from the containing viewport.</param>
		public void SetHorizontalOffset(Double offset)
		{

			// The horizontal offset cannot extend beyond the extent of the panel.
			offset = Math.Max(Math.Min(this.extent.Width - this.viewportSize.Width, offset), 0.0);

			// This will move the panel behind the viewport to make it appear as if it's scrolling.
			this.translateTransform.X = -offset;

			// If the offset has changed in the horizontal dimension, then update any descendants and measure the viewport with the new offset.
			if (offset != this.viewportOffset.X)
			{

				// This will update any descendant classes that the offset has changed.
				Vector oldViewportOffset = this.viewportOffset;
				this.viewportOffset.X = offset;
				this.OnViewportOffsetChanged(oldViewportOffset, this.viewportOffset);

				// The viewport needs to be measured again with the new offset.
				base.InvalidateMeasure();

			}

		}

		/// <summary>
		/// Sets the amount of vertical offset.
		/// </summary>
		/// <param name="offset">The degree to which content is vertically offset from the containing viewport.</param>
		public void SetVerticalOffset(Double offset)
		{

			// The vertical offset cannot extend beyond the extent of the panel.
			offset = Math.Max(Math.Min(this.extent.Height - this.viewportSize.Height, offset), 0.0);

			// This will move the panel behind the viewport to make it appear as if it's scrolling.
			this.translateTransform.Y = -offset;

			// If the offset has changed in the vertical dimension, then update any descendants and measure the viewport with the new offset.
			if (offset != this.viewportOffset.Y)
			{

				// This will update any descendant classes that the offset has changed.
				Vector oldViewportOffset = this.viewportOffset;
				this.viewportOffset.Y = offset;
				this.OnViewportOffsetChanged(oldViewportOffset, this.viewportOffset);

				// The viewport needs to be measured again with the new offset.
				base.InvalidateMeasure();

			}

		}

		/// <summary>
		/// Gets or sets the amount of time it takes for the animation to complete.
		/// </summary>
		public TimeSpan AnimationTime
		{
			get
			{
				return (TimeSpan)this.GetValue(ViewportPanel.AnimationTimeProperty);
			}
			set
			{
				this.SetValue(ViewportPanel.AnimationTimeProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a value that indicates whether scrolling on the horizontal axis is possible.
		/// </summary>
		public Boolean CanHorizontallyScroll
		{
			get
			{
				return true;
			}
			set { }
		}

		/// <summary>
		/// Gets or sets a value that indicates whether scrolling on the vertical axis is possible.
		/// </summary>
		public Boolean CanVerticallyScroll
		{
			get
			{
				return true;
			}
			set { }
		}

		/// <summary>
		/// Gets the vertical size of the extent.
		/// </summary>
		public Double ExtentHeight
		{
			get
			{
				return this.extent.Height;
			}
		}

		/// <summary>
		/// Gets the horizontal size of the extent.
		/// </summary>
		public Double ExtentWidth
		{
			get
			{
				return this.extent.Width;
			}
		}

		/// <summary>
		/// Gets a value that indicates whether this Panel arranges its descendants in a single dimension.
		/// </summary>
		protected override Boolean HasLogicalOrientation
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Gets the horizontal offset of the scrolled content.
		/// </summary>
		public Double HorizontalOffset
		{
			get
			{
				return this.viewportOffset.X;
			}
		}

		/// <summary>
		/// Gets an indication of whether the panel supports scrolling.
		/// </summary>
		internal Boolean IsScrolling
		{
			get
			{
				return this.scrollOwnerField != null;
			}
		}

		/// <summary>
		/// Gets the number of items in the panel.
		/// </summary>
		Int32 ItemCount
		{
			get
			{
				ItemsControl itemsControl = ItemsControl.GetItemsOwner(this);
				return itemsControl == null ? 0 : itemsControl.Items.Count;
			}
		}

		/// <summary>
		/// The Orientation of the panel, if the panel supports layout in only a single dimension.
		/// </summary>
		protected override Orientation LogicalOrientation
		{
			get
			{
				return this.Orientation;
			}
		}

		/// <summary>
		/// Gets or sets a value that indicates the dimension by which child elements are stacked.
		/// </summary>
		public Orientation Orientation
		{
			get
			{
				return (Orientation)base.GetValue(OrientationProperty);
			}
			set
			{
				base.SetValue(OrientationProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a ScrollViewer element that controls scrolling behavior.
		/// </summary>
		public ScrollViewer ScrollOwner
		{
			get
			{
				return this.scrollOwnerField;
			}
			set
			{
				if (value != this.scrollOwnerField)
				{
					this.scrollOwnerField = value;
					this.ResetScrolling();
				}
			}
		}

		/// <summary>
		/// Gets the vertical offset of the scrolled content.
		/// </summary>
		public Double VerticalOffset
		{
			get
			{
				return this.viewportOffset.Y;
			}
		}

		/// <summary>
		/// Gets the vertical size of the viewport for this content.
		/// </summary>
		public Double ViewportHeight
		{
			get
			{
				return this.viewportSize.Height;
			}
		}

		/// <summary>
		/// Gets the horizontal size of the viewport for this content.
		/// </summary>
		public Double ViewportWidth
		{
			get
			{
				return this.viewportSize.Width;
			}
		}

	}

}
