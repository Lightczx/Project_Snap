using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MahApps.Metro.Controls
{
	public static class TabControlHelper
	{
		public static readonly DependencyProperty CloseButtonEnabledProperty = DependencyProperty.RegisterAttached("CloseButtonEnabled", typeof(bool), typeof(TabControlHelper), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty CloseTabCommandProperty = DependencyProperty.RegisterAttached("CloseTabCommand", typeof(ICommand), typeof(TabControlHelper), new PropertyMetadata(null));

		public static readonly DependencyProperty CloseTabCommandParameterProperty = DependencyProperty.RegisterAttached("CloseTabCommandParameter", typeof(object), typeof(TabControlHelper), new PropertyMetadata(null));

		[Obsolete("This property will be deleted in the next release. You should now use the Underlined attached property.")]
		public static readonly DependencyProperty IsUnderlinedProperty = DependencyProperty.RegisterAttached("IsUnderlined", typeof(bool), typeof(TabControlHelper), new PropertyMetadata(false, delegate(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			UIElement uIElement = o as UIElement;
			if (uIElement != null && e.OldValue != e.NewValue && e.NewValue is bool)
			{
				SetUnderlined(uIElement, ((bool)e.NewValue) ? UnderlinedType.TabItems : UnderlinedType.None);
			}
		}));

		public static readonly DependencyProperty UnderlinedProperty = DependencyProperty.RegisterAttached("Underlined", typeof(UnderlinedType), typeof(TabControlHelper), new PropertyMetadata(UnderlinedType.None));

		public static readonly DependencyProperty UnderlineBrushProperty = DependencyProperty.RegisterAttached("UnderlineBrush", typeof(Brush), typeof(TabControlHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty UnderlineSelectedBrushProperty = DependencyProperty.RegisterAttached("UnderlineSelectedBrush", typeof(Brush), typeof(TabControlHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty UnderlineMouseOverBrushProperty = DependencyProperty.RegisterAttached("UnderlineMouseOverBrush", typeof(Brush), typeof(TabControlHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty UnderlineMouseOverSelectedBrushProperty = DependencyProperty.RegisterAttached("UnderlineMouseOverSelectedBrush", typeof(Brush), typeof(TabControlHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty TransitionProperty = DependencyProperty.RegisterAttached("Transition", typeof(TransitionType), typeof(TabControlHelper), new FrameworkPropertyMetadata(TransitionType.Default, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));

		public static void ClearStyle(this TabItem tabItem)
		{
			if (tabItem != null)
			{
				tabItem.Template = null;
				tabItem.Style = null;
			}
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TabItem))]
		public static bool GetCloseButtonEnabled(UIElement element)
		{
			return (bool)element.GetValue(CloseButtonEnabledProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TabItem))]
		public static void SetCloseButtonEnabled(UIElement element, bool value)
		{
			element.SetValue(CloseButtonEnabledProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TabItem))]
		public static ICommand GetCloseTabCommand(UIElement element)
		{
			return (ICommand)element.GetValue(CloseTabCommandProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TabItem))]
		public static void SetCloseTabCommand(UIElement element, ICommand value)
		{
			element.SetValue(CloseTabCommandProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TabItem))]
		public static object GetCloseTabCommandParameter(UIElement element)
		{
			return element.GetValue(CloseTabCommandParameterProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TabItem))]
		public static void SetCloseTabCommandParameter(UIElement element, object value)
		{
			element.SetValue(CloseTabCommandParameterProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TabControl))]
		[Obsolete("This property will be deleted in the next release. You should now use the Underlined attached property.")]
		public static bool GetIsUnderlined(UIElement element)
		{
			return (bool)element.GetValue(IsUnderlinedProperty);
		}

		[Obsolete("This property will be deleted in the next release. You should now use the Underlined attached property.")]
		public static void SetIsUnderlined(UIElement element, bool value)
		{
			element.SetValue(IsUnderlinedProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TabControl))]
		public static UnderlinedType GetUnderlined(UIElement element)
		{
			return (UnderlinedType)element.GetValue(UnderlinedProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TabControl))]
		public static void SetUnderlined(UIElement element, UnderlinedType value)
		{
			element.SetValue(UnderlinedProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TabControl))]
		[AttachedPropertyBrowsableForType(typeof(TabItem))]
		public static Brush GetUnderlineBrush(UIElement element)
		{
			return (Brush)element.GetValue(UnderlineBrushProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TabControl))]
		[AttachedPropertyBrowsableForType(typeof(TabItem))]
		public static void SetUnderlineBrush(UIElement element, Brush value)
		{
			element.SetValue(UnderlineBrushProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TabControl))]
		[AttachedPropertyBrowsableForType(typeof(TabItem))]
		public static Brush GetUnderlineSelectedBrush(UIElement element)
		{
			return (Brush)element.GetValue(UnderlineSelectedBrushProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TabControl))]
		[AttachedPropertyBrowsableForType(typeof(TabItem))]
		public static void SetUnderlineSelectedBrush(UIElement element, Brush value)
		{
			element.SetValue(UnderlineSelectedBrushProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TabControl))]
		[AttachedPropertyBrowsableForType(typeof(TabItem))]
		public static Brush GetUnderlineMouseOverBrush(UIElement element)
		{
			return (Brush)element.GetValue(UnderlineMouseOverBrushProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TabControl))]
		[AttachedPropertyBrowsableForType(typeof(TabItem))]
		public static void SetUnderlineMouseOverBrush(UIElement element, Brush value)
		{
			element.SetValue(UnderlineMouseOverBrushProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TabControl))]
		[AttachedPropertyBrowsableForType(typeof(TabItem))]
		public static Brush GetUnderlineMouseOverSelectedBrush(UIElement element)
		{
			return (Brush)element.GetValue(UnderlineMouseOverSelectedBrushProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TabControl))]
		[AttachedPropertyBrowsableForType(typeof(TabItem))]
		public static void SetUnderlineMouseOverSelectedBrush(UIElement element, Brush value)
		{
			element.SetValue(UnderlineMouseOverSelectedBrushProperty, value);
		}

		[Category("MahApps.Metro")]
		public static TransitionType GetTransition(DependencyObject obj)
		{
			return (TransitionType)obj.GetValue(TransitionProperty);
		}

		public static void SetTransition(DependencyObject obj, TransitionType value)
		{
			obj.SetValue(TransitionProperty, value);
		}
	}
}
