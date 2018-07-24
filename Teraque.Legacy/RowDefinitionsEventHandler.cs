﻿namespace Teraque
{


    /// <summary>
	/// Used to invoke events that require a column definition.
	/// </summary>
	/// <param name="reportColumn"></param>
	public delegate void RowDefinitionsEventHandler(object sender, RowDefinitionsEventArgs columnsChangedEventArgs);

}
