using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MahApps.Metro.Controls
{
	public static class TreeHelper
	{
		public static T TryFindParent<T>(this DependencyObject child) where T : DependencyObject
		{
			DependencyObject parentObject = child.GetParentObject();
			if (parentObject == null)
			{
				return null;
			}
			return (parentObject as T) ?? parentObject.TryFindParent<T>();
		}

		public static IEnumerable<DependencyObject> GetAncestors(this DependencyObject child)
		{
			for (DependencyObject parent = VisualTreeHelper.GetParent(child); parent != null; parent = VisualTreeHelper.GetParent(parent))
			{
				yield return parent;
			}
		}

		public static T FindChild<T>(this DependencyObject parent, string childName) where T : DependencyObject
		{
			if (parent == null)
			{
				return null;
			}
			T val = null;
			int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
			for (int i = 0; i < childrenCount; i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(parent, i);
				if (child as T == null)
				{
					val = child.FindChild<T>(childName);
					if (val != null)
					{
						break;
					}
				}
				else
				{
					if (string.IsNullOrEmpty(childName))
					{
						val = (T)child;
						break;
					}
					IFrameworkInputElement frameworkInputElement = child as IFrameworkInputElement;
					if (frameworkInputElement != null && frameworkInputElement.Name == childName)
					{
						val = (T)child;
						break;
					}
					val = child.FindChild<T>(childName);
					if (val != null)
					{
						break;
					}
				}
			}
			return val;
		}

		public static DependencyObject GetParentObject(this DependencyObject child)
		{
			if (child == null)
			{
				return null;
			}
			ContentElement contentElement = child as ContentElement;
			if (contentElement != null)
			{
				DependencyObject parent = ContentOperations.GetParent(contentElement);
				if (parent != null)
				{
					return parent;
				}
				return (contentElement as FrameworkContentElement)?.Parent;
			}
			DependencyObject parent2 = VisualTreeHelper.GetParent(child);
			if (parent2 != null)
			{
				return parent2;
			}
			FrameworkElement frameworkElement = child as FrameworkElement;
			if (frameworkElement != null)
			{
				DependencyObject parent3 = frameworkElement.Parent;
				if (parent3 != null)
				{
					return parent3;
				}
			}
			return null;
		}

		public static IEnumerable<T> FindChildren<T>(this DependencyObject source, bool forceUsingTheVisualTreeHelper = false) where T : DependencyObject
		{
			if (source != null)
			{
				IEnumerable<DependencyObject> childObjects = source.GetChildObjects(forceUsingTheVisualTreeHelper);
				foreach (DependencyObject item in childObjects)
				{
					if (item != null && item is T)
					{
						yield return (T)item;
					}
					foreach (T item2 in item.FindChildren<T>(forceUsingTheVisualTreeHelper))
					{
						yield return item2;
					}
				}
			}
		}

		public static IEnumerable<DependencyObject> GetChildObjects(this DependencyObject parent, bool forceUsingTheVisualTreeHelper = false)
		{
			if (parent != null)
			{
				if (!forceUsingTheVisualTreeHelper && (parent is ContentElement || parent is FrameworkElement))
				{
					foreach (object child in LogicalTreeHelper.GetChildren(parent))
					{
						if (child is DependencyObject)
						{
							yield return (DependencyObject)child;
						}
					}
				}
				else if (parent is Visual || parent is Visual3D)
				{
					int count = VisualTreeHelper.GetChildrenCount(parent);
					for (int i = 0; i < count; i++)
					{
						yield return VisualTreeHelper.GetChild(parent, i);
					}
				}
			}
		}

		public static T TryFindFromPoint<T>(UIElement reference, Point point) where T : DependencyObject
		{
			DependencyObject dependencyObject = reference.InputHitTest(point) as DependencyObject;
			if (dependencyObject == null)
			{
				return null;
			}
			if (dependencyObject is T)
			{
				return (T)dependencyObject;
			}
			return dependencyObject.TryFindParent<T>();
		}
	}
}
