namespace Teraque
{

	using System;

	/// <summary>
	/// Contains the event data for a ItemPropertyChanged event.
	/// </summary>
	public class ItemPropertyChangedEventArgs : EventArgs
	{


		/// <summary>
		/// The item that has changed.
		/// </summary>
		Object itemField;

		/// <summary>
		/// The name of the property that has changed.
		/// </summary>
		String propertyNameField;

		/// <summary>
		/// Initializes a new instance of the PropetyChangedEventArgs class.
		/// </summary>
		/// <param name="item">The item that has been changed.</param>
		/// <param name="propertyName">The property on the given item that has changed.</param>
		public ItemPropertyChangedEventArgs(Object item, String propertyName)
		{

			// Initialize the object.
			this.itemField = item;
			this.propertyNameField = propertyName;

		}

		/// <summary>
		/// Gets the item that has changed.
		/// </summary>
		public Object Item
		{
			get
			{
				return this.itemField;
			}
		}

		/// <summary>
		/// Gets the property name that has changed.
		/// </summary>
		public String PropertyName
		{
			get
			{
				return this.propertyNameField;
			}
		}

	}

}
