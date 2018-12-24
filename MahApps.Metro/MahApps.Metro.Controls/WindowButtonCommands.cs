using ControlzEx.Native;
using ControlzEx.Windows.Shell;
using System;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MahApps.Metro.Controls
{
	[TemplatePart(Name = "PART_Min", Type = typeof(Button))]
	[TemplatePart(Name = "PART_Max", Type = typeof(Button))]
	[TemplatePart(Name = "PART_Close", Type = typeof(Button))]
	public class WindowButtonCommands : ContentControl, INotifyPropertyChanged
	{
		public delegate void ClosingWindowEventHandler(object sender, ClosingWindowEventHandlerArgs args);

		public static readonly DependencyProperty LightMinButtonStyleProperty;

		public static readonly DependencyProperty LightMaxButtonStyleProperty;

		public static readonly DependencyProperty LightCloseButtonStyleProperty;

		public static readonly DependencyProperty DarkMinButtonStyleProperty;

		public static readonly DependencyProperty DarkMaxButtonStyleProperty;

		public static readonly DependencyProperty DarkCloseButtonStyleProperty;

		public static readonly DependencyProperty ThemeProperty;

		public static readonly DependencyProperty MinimizeProperty;

		public static readonly DependencyProperty MaximizeProperty;

		public static readonly DependencyProperty CloseProperty;

		public static readonly DependencyProperty RestoreProperty;

		private Button min;

		private Button max;

		private Button close;

		private SafeLibraryHandle user32;

		private MetroWindow _parentWindow;

		public Style LightMinButtonStyle
		{
			get
			{
				return (Style)GetValue(LightMinButtonStyleProperty);
			}
			set
			{
				SetValue(LightMinButtonStyleProperty, value);
			}
		}

		public Style LightMaxButtonStyle
		{
			get
			{
				return (Style)GetValue(LightMaxButtonStyleProperty);
			}
			set
			{
				SetValue(LightMaxButtonStyleProperty, value);
			}
		}

		public Style LightCloseButtonStyle
		{
			get
			{
				return (Style)GetValue(LightCloseButtonStyleProperty);
			}
			set
			{
				SetValue(LightCloseButtonStyleProperty, value);
			}
		}

		public Style DarkMinButtonStyle
		{
			get
			{
				return (Style)GetValue(DarkMinButtonStyleProperty);
			}
			set
			{
				SetValue(DarkMinButtonStyleProperty, value);
			}
		}

		public Style DarkMaxButtonStyle
		{
			get
			{
				return (Style)GetValue(DarkMaxButtonStyleProperty);
			}
			set
			{
				SetValue(DarkMaxButtonStyleProperty, value);
			}
		}

		public Style DarkCloseButtonStyle
		{
			get
			{
				return (Style)GetValue(DarkCloseButtonStyleProperty);
			}
			set
			{
				SetValue(DarkCloseButtonStyleProperty, value);
			}
		}

		public Theme Theme
		{
			get
			{
				return (Theme)GetValue(ThemeProperty);
			}
			set
			{
				SetValue(ThemeProperty, value);
			}
		}

		public string Minimize
		{
			get
			{
				return (string)GetValue(MinimizeProperty);
			}
			set
			{
				SetValue(MinimizeProperty, value);
			}
		}

		public string Maximize
		{
			get
			{
				return (string)GetValue(MaximizeProperty);
			}
			set
			{
				SetValue(MaximizeProperty, value);
			}
		}

		public string Close
		{
			get
			{
				return (string)GetValue(CloseProperty);
			}
			set
			{
				SetValue(CloseProperty, value);
			}
		}

		public string Restore
		{
			get
			{
				return (string)GetValue(RestoreProperty);
			}
			set
			{
				SetValue(RestoreProperty, value);
			}
		}

		public MetroWindow ParentWindow
		{
			get
			{
				return _parentWindow;
			}
			set
			{
				if (!object.Equals(_parentWindow, value))
				{
					_parentWindow = value;
					RaisePropertyChanged("ParentWindow");
				}
			}
		}

		public event ClosingWindowEventHandler ClosingWindow;

		public event PropertyChangedEventHandler PropertyChanged;

		private static void OnThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue != e.OldValue)
			{
				((WindowButtonCommands)d).ApplyTheme();
			}
		}

		static WindowButtonCommands()
		{
			LightMinButtonStyleProperty = DependencyProperty.Register("LightMinButtonStyle", typeof(Style), typeof(WindowButtonCommands), new PropertyMetadata(null, OnThemeChanged));
			LightMaxButtonStyleProperty = DependencyProperty.Register("LightMaxButtonStyle", typeof(Style), typeof(WindowButtonCommands), new PropertyMetadata(null, OnThemeChanged));
			LightCloseButtonStyleProperty = DependencyProperty.Register("LightCloseButtonStyle", typeof(Style), typeof(WindowButtonCommands), new PropertyMetadata(null, OnThemeChanged));
			DarkMinButtonStyleProperty = DependencyProperty.Register("DarkMinButtonStyle", typeof(Style), typeof(WindowButtonCommands), new PropertyMetadata(null, OnThemeChanged));
			DarkMaxButtonStyleProperty = DependencyProperty.Register("DarkMaxButtonStyle", typeof(Style), typeof(WindowButtonCommands), new PropertyMetadata(null, OnThemeChanged));
			DarkCloseButtonStyleProperty = DependencyProperty.Register("DarkCloseButtonStyle", typeof(Style), typeof(WindowButtonCommands), new PropertyMetadata(null, OnThemeChanged));
			ThemeProperty = DependencyProperty.Register("Theme", typeof(Theme), typeof(WindowButtonCommands), new PropertyMetadata(Theme.Light, OnThemeChanged));
			MinimizeProperty = DependencyProperty.Register("Minimize", typeof(string), typeof(WindowButtonCommands), new PropertyMetadata(null));
			MaximizeProperty = DependencyProperty.Register("Maximize", typeof(string), typeof(WindowButtonCommands), new PropertyMetadata(null));
			CloseProperty = DependencyProperty.Register("Close", typeof(string), typeof(WindowButtonCommands), new PropertyMetadata(null));
			RestoreProperty = DependencyProperty.Register("Restore", typeof(string), typeof(WindowButtonCommands), new PropertyMetadata(null));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowButtonCommands), new FrameworkPropertyMetadata(typeof(WindowButtonCommands)));
		}

		public WindowButtonCommands()
		{
			base.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Action)delegate
			{
				if (string.IsNullOrWhiteSpace(Minimize))
				{
					SetCurrentValue(MinimizeProperty, GetCaption(900));
				}
				if (string.IsNullOrWhiteSpace(Maximize))
				{
					SetCurrentValue(MaximizeProperty, GetCaption(901));
				}
				if (string.IsNullOrWhiteSpace(Close))
				{
					SetCurrentValue(CloseProperty, GetCaption(905));
				}
				if (string.IsNullOrWhiteSpace(Restore))
				{
					SetCurrentValue(RestoreProperty, GetCaption(903));
				}
			});
		}

		private string GetCaption(int id)
		{
			if (user32 == null)
			{
				user32 = UnsafeNativeMethods.LoadLibrary(Environment.SystemDirectory + "\\User32.dll");
			}
			StringBuilder stringBuilder = new StringBuilder(256);
			UnsafeNativeMethods.LoadString(user32, (uint)id, stringBuilder, stringBuilder.Capacity);
			return stringBuilder.ToString().Replace("&", "");
		}

		public void ApplyTheme()
		{
			if (close != null)
			{
				if (ParentWindow?.WindowCloseButtonStyle != null)
				{
					close.Style = ParentWindow.WindowCloseButtonStyle;
				}
				else
				{
					close.Style = ((Theme == Theme.Light) ? LightCloseButtonStyle : DarkCloseButtonStyle);
				}
			}
			if (max != null)
			{
				if (ParentWindow?.WindowMaxButtonStyle != null)
				{
					max.Style = ParentWindow.WindowMaxButtonStyle;
				}
				else
				{
					max.Style = ((Theme == Theme.Light) ? LightMaxButtonStyle : DarkMaxButtonStyle);
				}
			}
			if (min != null)
			{
				if (ParentWindow?.WindowMinButtonStyle != null)
				{
					min.Style = ParentWindow.WindowMinButtonStyle;
				}
				else
				{
					min.Style = ((Theme == Theme.Light) ? LightMinButtonStyle : DarkMinButtonStyle);
				}
			}
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			close = (base.Template.FindName("PART_Close", this) as Button);
			if (close != null)
			{
				close.Click += CloseClick;
			}
			max = (base.Template.FindName("PART_Max", this) as Button);
			if (max != null)
			{
				max.Click += MaximizeClick;
			}
			min = (base.Template.FindName("PART_Min", this) as Button);
			if (min != null)
			{
				min.Click += MinimizeClick;
			}
			ApplyTheme();
		}

		protected void OnClosingWindow(ClosingWindowEventHandlerArgs args)
		{
			this.ClosingWindow?.Invoke(this, args);
		}

		private void MinimizeClick(object sender, RoutedEventArgs e)
		{
			if (ParentWindow != null)
			{
				ControlzEx.Windows.Shell.SystemCommands.MinimizeWindow(ParentWindow);
			}
		}

		private void MaximizeClick(object sender, RoutedEventArgs e)
		{
			if (ParentWindow != null)
			{
				if (ParentWindow.WindowState == WindowState.Maximized)
				{
					ControlzEx.Windows.Shell.SystemCommands.RestoreWindow(ParentWindow);
				}
				else
				{
					ControlzEx.Windows.Shell.SystemCommands.MaximizeWindow(ParentWindow);
				}
			}
		}

		private void CloseClick(object sender, RoutedEventArgs e)
		{
			ClosingWindowEventHandlerArgs closingWindowEventHandlerArgs = new ClosingWindowEventHandlerArgs();
			OnClosingWindow(closingWindowEventHandlerArgs);
			if (!closingWindowEventHandlerArgs.Cancelled && ParentWindow != null)
			{
				ParentWindow.Close();
			}
		}

		protected virtual void RaisePropertyChanged(string propertyName = null)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
