namespace Teraque.Windows
{

	using System;
	using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using System.Windows.Media;

	/// <summary>
	/// Additional Utility functions for the Visual Tree.
	/// </summary>
	public static class VisualTreeExtensions
	{

		/// <summary>
		/// Finds the ancestor window of the given type.
		/// </summary>
		/// <typeparam name="TType">The type of window to find.</typeparam>
		/// <param name="dependencyObject">The starting point for the search.</param>
		/// <returns>The visual ancestor of the given dependency object with the given type.</returns>
        public static TType FindAncestor<TType>(DependencyObject dependencyObject)
			where TType : class
		{

			DependencyObject genericTarget = dependencyObject;
            TType typedTarget = null;

			// This will walk up the visual tree until an ancestor with the given type is found or until the root window is found.
			if (dependencyObject != null)
			{
				do
				{

					// Get the generic parent using the VisualTreeHelper.
					DependencyObject parent = genericTarget;
					genericTarget = VisualTreeHelper.GetParent(genericTarget);

					// If the generic parent matches the desired type, then we've found the object of our search.
					typedTarget = genericTarget as TType;
                    if (typedTarget != null)
                        break;

					// If the generic parent isn't of the desired type, then we'll see if the Parent window of our visual parent has the desired type.  If not we
					// will take our parent and keep on searching.  Note that this effort is more exaustive than the 'FindAncestor' binding instruction as it uses
					// the FrameworkElement's parents instead of relying on a pure visual tree search.  Content controls are especially bad for this.
					if (genericTarget == null)
					{
						FrameworkElement frameworkElement = parent as FrameworkElement;
						if (frameworkElement != null)
						{
							genericTarget = frameworkElement.Parent;
							typedTarget = genericTarget as TType;
							if (typedTarget != null)
								break;
						}
					}

				} while (genericTarget != null);
			}

			// This is the ancestor of the desired type, or null if there is no ancestor of that type.
			return typedTarget;

		}

	}

}
