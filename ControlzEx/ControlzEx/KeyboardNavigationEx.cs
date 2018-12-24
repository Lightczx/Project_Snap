using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ControlzEx
{
	public sealed class KeyboardNavigationEx
	{
		private static KeyboardNavigationEx _instance;

		private readonly PropertyInfo _alwaysShowFocusVisual;

		private readonly MethodInfo _showFocusVisual;

		public static readonly DependencyProperty AlwaysShowFocusVisualProperty;

		internal static KeyboardNavigationEx Instance => _instance ?? (_instance = new KeyboardNavigationEx());

		internal bool AlwaysShowFocusVisualInternal
		{
			get
			{
				return (bool)_alwaysShowFocusVisual.GetValue(null, null);
			}
			set
			{
				_alwaysShowFocusVisual.SetValue(null, value, null);
			}
		}

		static KeyboardNavigationEx()
		{
			AlwaysShowFocusVisualProperty = DependencyProperty.RegisterAttached("AlwaysShowFocusVisual", typeof(bool), typeof(KeyboardNavigationEx), new FrameworkPropertyMetadata(false, AlwaysShowFocusVisualPropertyChangedCallback));
		}

		private KeyboardNavigationEx()
		{
			Type typeFromHandle = typeof(KeyboardNavigation);
			_alwaysShowFocusVisual = typeFromHandle.GetProperty("AlwaysShowFocusVisual", BindingFlags.Static | BindingFlags.NonPublic);
			_showFocusVisual = typeFromHandle.GetMethod("ShowFocusVisual", BindingFlags.Static | BindingFlags.NonPublic);
		}

		internal void ShowFocusVisualInternal()
		{
			_showFocusVisual.Invoke(null, null);
		}

		public static void Focus(UIElement element)
		{
			element?.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
			{
				KeyboardNavigationEx instance = Instance;
				bool alwaysShowFocusVisualInternal = instance.AlwaysShowFocusVisualInternal;
				instance.AlwaysShowFocusVisualInternal = true;
				try
				{
					Keyboard.Focus(element);
					instance.ShowFocusVisualInternal();
				}
				finally
				{
					instance.AlwaysShowFocusVisualInternal = alwaysShowFocusVisualInternal;
				}
			});
		}

		private static void AlwaysShowFocusVisualPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			UIElement uIElement = dependencyObject as UIElement;
			if (uIElement != null && args.NewValue != args.OldValue)
			{
				uIElement.GotFocus -= FrameworkElementGotFocus;
				if ((bool)args.NewValue)
				{
					uIElement.GotFocus += FrameworkElementGotFocus;
				}
			}
		}

		private static void FrameworkElementGotFocus(object sender, RoutedEventArgs e)
		{
			Focus(sender as UIElement);
		}

		[AttachedPropertyBrowsableForType(typeof(UIElement))]
		public static bool GetAlwaysShowFocusVisual(UIElement element)
		{
			return (bool)element.GetValue(AlwaysShowFocusVisualProperty);
		}

		public static void SetAlwaysShowFocusVisual(UIElement element, bool value)
		{
			element.SetValue(AlwaysShowFocusVisualProperty, value);
		}
	}
}
