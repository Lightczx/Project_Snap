using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MahApps.Metro.Controls
{
	public class PasswordBoxHelper
	{
		public static readonly DependencyProperty CapsLockIconProperty = DependencyProperty.RegisterAttached("CapsLockIcon", typeof(object), typeof(PasswordBoxHelper), new PropertyMetadata("!", ShowCapslockWarningChanged));

		public static readonly DependencyProperty CapsLockWarningToolTipProperty = DependencyProperty.RegisterAttached("CapsLockWarningToolTip", typeof(object), typeof(PasswordBoxHelper), new PropertyMetadata("Caps lock is on"));

		public static readonly DependencyProperty RevealButtonContentProperty = DependencyProperty.RegisterAttached("RevealButtonContent", typeof(object), typeof(PasswordBoxHelper), new FrameworkPropertyMetadata(null));

		public static readonly DependencyProperty RevealButtonContentTemplateProperty = DependencyProperty.RegisterAttached("RevealButtonContentTemplate", typeof(DataTemplate), typeof(PasswordBoxHelper), new FrameworkPropertyMetadata((object)null));

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(PasswordBox))]
		public static object GetCapsLockIcon(PasswordBox element)
		{
			return element.GetValue(CapsLockIconProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(PasswordBox))]
		public static void SetCapsLockIcon(PasswordBox element, object value)
		{
			element.SetValue(CapsLockIconProperty, value);
		}

		private static void ShowCapslockWarningChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue != e.OldValue)
			{
				PasswordBox passwordBox = (PasswordBox)d;
				passwordBox.KeyDown -= RefreshCapslockStatus;
				passwordBox.GotFocus -= RefreshCapslockStatus;
				passwordBox.PreviewGotKeyboardFocus -= RefreshCapslockStatus;
				passwordBox.LostFocus -= HandlePasswordBoxLostFocus;
				if (e.NewValue != null)
				{
					passwordBox.KeyDown += RefreshCapslockStatus;
					passwordBox.GotFocus += RefreshCapslockStatus;
					passwordBox.PreviewGotKeyboardFocus += RefreshCapslockStatus;
					passwordBox.LostFocus += HandlePasswordBoxLostFocus;
				}
			}
		}

		private static void RefreshCapslockStatus(object sender, RoutedEventArgs e)
		{
			FrameworkElement frameworkElement = FindCapsLockIndicator((Control)sender);
			if (frameworkElement != null)
			{
				frameworkElement.Visibility = ((!Keyboard.IsKeyToggled(Key.Capital)) ? Visibility.Collapsed : Visibility.Visible);
			}
		}

		private static void HandlePasswordBoxLostFocus(object sender, RoutedEventArgs e)
		{
			FrameworkElement frameworkElement = FindCapsLockIndicator((Control)sender);
			if (frameworkElement != null)
			{
				frameworkElement.Visibility = Visibility.Collapsed;
			}
		}

		private static FrameworkElement FindCapsLockIndicator(Control pb)
		{
			return pb?.Template?.FindName("PART_CapsLockIndicator", pb) as FrameworkElement;
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(PasswordBox))]
		public static object GetCapsLockWarningToolTip(PasswordBox element)
		{
			return element.GetValue(CapsLockWarningToolTipProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(PasswordBox))]
		public static void SetCapsLockWarningToolTip(PasswordBox element, object value)
		{
			element.SetValue(CapsLockWarningToolTipProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(PasswordBox))]
		public static object GetRevealButtonContent(DependencyObject d)
		{
			return d.GetValue(RevealButtonContentProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(PasswordBox))]
		public static void SetRevealButtonContent(DependencyObject obj, object value)
		{
			obj.SetValue(RevealButtonContentProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(PasswordBox))]
		public static DataTemplate GetRevealButtonContentTemplate(DependencyObject d)
		{
			return (DataTemplate)d.GetValue(RevealButtonContentTemplateProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(PasswordBox))]
		public static void SetRevealButtonContentTemplate(DependencyObject obj, DataTemplate value)
		{
			obj.SetValue(RevealButtonContentTemplateProperty, value);
		}
	}
}
