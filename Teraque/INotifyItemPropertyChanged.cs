namespace Teraque
{

	using System;

	/// <summary>
	/// Notifies clients that a property value has changed on an item in the collection.
	/// </summary>
	public interface INotifyItemPropertyChanged
	{

		/// <summary>
		/// Occurs when a property value changes on an item in the collection.
		/// </summary>
		event EventHandler<ItemPropertyChangedEventArgs> ItemPropertyChanged;

	}

}
