using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace MahApps.Metro.Controls.Dialogs
{
	public abstract class BaseMetroDialog : ContentControl
	{
		public static readonly DependencyProperty TitleProperty;

		public static readonly DependencyProperty DialogTopProperty;

		public static readonly DependencyProperty DialogBottomProperty;

		public static readonly DependencyProperty DialogTitleFontSizeProperty;

		public static readonly DependencyProperty DialogMessageFontSizeProperty;

		public MetroDialogSettings DialogSettings
		{
			get;
			private set;
		}

		public string Title
		{
			get
			{
				return (string)GetValue(TitleProperty);
			}
			set
			{
				SetValue(TitleProperty, value);
			}
		}

		public object DialogTop
		{
			get
			{
				return GetValue(DialogTopProperty);
			}
			set
			{
				SetValue(DialogTopProperty, value);
			}
		}

		public object DialogBottom
		{
			get
			{
				return GetValue(DialogBottomProperty);
			}
			set
			{
				SetValue(DialogBottomProperty, value);
			}
		}

		public double DialogTitleFontSize
		{
			get
			{
				return (double)GetValue(DialogTitleFontSizeProperty);
			}
			set
			{
				SetValue(DialogTitleFontSizeProperty, value);
			}
		}

		public double DialogMessageFontSize
		{
			get
			{
				return (double)GetValue(DialogMessageFontSizeProperty);
			}
			set
			{
				SetValue(DialogMessageFontSizeProperty, value);
			}
		}

		internal SizeChangedEventHandler SizeChangedHandler
		{
			get;
			set;
		}

		protected internal Window ParentDialogWindow
		{
			get;
			internal set;
		}

		protected internal MetroWindow OwningWindow
		{
			get;
			internal set;
		}

		static BaseMetroDialog()
		{
			TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(BaseMetroDialog), new PropertyMetadata((object)null));
			DialogTopProperty = DependencyProperty.Register("DialogTop", typeof(object), typeof(BaseMetroDialog), new PropertyMetadata(null));
			DialogBottomProperty = DependencyProperty.Register("DialogBottom", typeof(object), typeof(BaseMetroDialog), new PropertyMetadata(null));
			DialogTitleFontSizeProperty = DependencyProperty.Register("DialogTitleFontSize", typeof(double), typeof(BaseMetroDialog), new PropertyMetadata(26.0));
			DialogMessageFontSizeProperty = DependencyProperty.Register("DialogMessageFontSize", typeof(double), typeof(BaseMetroDialog), new PropertyMetadata(15.0));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseMetroDialog), new FrameworkPropertyMetadata(typeof(BaseMetroDialog)));
		}

		protected BaseMetroDialog(MetroWindow owningWindow, MetroDialogSettings settings)
		{
			Initialize(owningWindow, settings);
		}

		protected BaseMetroDialog()
			: this(null, new MetroDialogSettings())
		{
		}

		protected virtual MetroDialogSettings ConfigureSettings(MetroDialogSettings settings)
		{
			return settings;
		}

		private void Initialize(MetroWindow owningWindow, MetroDialogSettings settings)
		{
			OwningWindow = owningWindow;
			DialogSettings = ConfigureSettings(settings ?? owningWindow?.MetroDialogOptions ?? new MetroDialogSettings());
			if (DialogSettings?.CustomResourceDictionary != null)
			{
				base.Resources.MergedDictionaries.Add(DialogSettings.CustomResourceDictionary);
			}
			HandleThemeChange();
			base.Loaded += BaseMetroDialogLoaded;
			base.Unloaded += BaseMetroDialogUnloaded;
		}

		private void BaseMetroDialogLoaded(object sender, RoutedEventArgs e)
		{
			ThemeManager.IsThemeChanged -= ThemeManagerIsThemeChanged;
			ThemeManager.IsThemeChanged += ThemeManagerIsThemeChanged;
			OnLoaded();
		}

		private void BaseMetroDialogUnloaded(object sender, RoutedEventArgs e)
		{
			ThemeManager.IsThemeChanged -= ThemeManagerIsThemeChanged;
		}

		private void ThemeManagerIsThemeChanged(object sender, OnThemeChangedEventArgs e)
		{
			HandleThemeChange();
		}

		private static object TryGetResource(Accent accent, AppTheme theme, string key)
		{
			if (accent == null || theme == null)
			{
				return null;
			}
			object result = theme.Resources[key];
			object obj = accent.Resources[key];
			if (obj != null)
			{
				return obj;
			}
			return result;
		}

		internal void HandleThemeChange()
		{
			Tuple<AppTheme, Accent> tuple = DetectTheme(this);
			if (!DesignerProperties.GetIsInDesignMode(this) && tuple != null)
			{
				Accent item = tuple.Item2;
				AppTheme appTheme = tuple.Item1;
				if (DialogSettings != null)
				{
					switch (DialogSettings.ColorScheme)
					{
					case MetroDialogColorScheme.Theme:
						ThemeManager.ChangeAppStyle(base.Resources, item, appTheme);
						SetValue(Control.BackgroundProperty, TryGetResource(item, appTheme, "WhiteColorBrush"));
						SetValue(Control.ForegroundProperty, TryGetResource(item, appTheme, "BlackBrush"));
						break;
					case MetroDialogColorScheme.Inverted:
						appTheme = ThemeManager.GetInverseAppTheme(appTheme);
						if (appTheme == null)
						{
							throw new InvalidOperationException("The inverse dialog theme only works if the window theme abides the naming convention. See ThemeManager.GetInverseAppTheme for more infos");
						}
						ThemeManager.ChangeAppStyle(base.Resources, item, appTheme);
						SetValue(Control.BackgroundProperty, TryGetResource(item, appTheme, "WhiteColorBrush"));
						SetValue(Control.ForegroundProperty, TryGetResource(item, appTheme, "BlackBrush"));
						break;
					case MetroDialogColorScheme.Accented:
						ThemeManager.ChangeAppStyle(base.Resources, item, appTheme);
						SetValue(Control.BackgroundProperty, TryGetResource(item, appTheme, "HighlightBrush"));
						SetValue(Control.ForegroundProperty, TryGetResource(item, appTheme, "IdealForegroundColorBrush"));
						break;
					}
				}
				if (ParentDialogWindow != null)
				{
					ParentDialogWindow.SetValue(Control.BackgroundProperty, base.Background);
					object obj = TryGetResource(item, appTheme, "AccentColorBrush");
					if (obj != null)
					{
						ParentDialogWindow.SetValue(MetroWindow.GlowBrushProperty, obj);
					}
				}
			}
		}

		protected virtual void OnLoaded()
		{
		}

		private static Tuple<AppTheme, Accent> DetectTheme(BaseMetroDialog dialog)
		{
			if (dialog == null)
			{
				return null;
			}
			MetroWindow metroWindow = dialog.OwningWindow ?? dialog.TryFindParent<MetroWindow>();
			Tuple<AppTheme, Accent> tuple = (metroWindow != null) ? ThemeManager.DetectAppStyle(metroWindow) : null;
			if (((tuple != null) ? tuple.Item2 : null) != null)
			{
				return tuple;
			}
			if (Application.Current != null)
			{
				MetroWindow metroWindow2 = Application.Current.MainWindow as MetroWindow;
				tuple = ((metroWindow2 != null) ? ThemeManager.DetectAppStyle(metroWindow2) : null);
				if (((tuple != null) ? tuple.Item2 : null) != null)
				{
					return tuple;
				}
				tuple = ThemeManager.DetectAppStyle(Application.Current);
				if (((tuple != null) ? tuple.Item2 : null) != null)
				{
					return tuple;
				}
			}
			return null;
		}

		public Task WaitForLoadAsync()
		{
			base.Dispatcher.VerifyAccess();
			if (base.IsLoaded)
			{
				return new Task(delegate
				{
				});
			}
			if (!DialogSettings.AnimateShow)
			{
				base.Opacity = 1.0;
			}
			TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
			RoutedEventHandler handler = null;
			handler = delegate
			{
				base.Loaded -= handler;
				Focus();
				tcs.TrySetResult(null);
			};
			base.Loaded += handler;
			return tcs.Task;
		}

		public Task RequestCloseAsync()
		{
			if (OnRequestClose())
			{
				if (ParentDialogWindow == null)
				{
					return OwningWindow.HideMetroDialogAsync(this);
				}
				return _WaitForCloseAsync().ContinueWith(delegate
				{
					ParentDialogWindow.Dispatcher.Invoke(delegate
					{
						ParentDialogWindow.Close();
					});
				});
			}
			return Task.Factory.StartNew(delegate
			{
			});
		}

		protected internal virtual void OnShown()
		{
		}

		protected internal virtual void OnClose()
		{
			ParentDialogWindow?.Close();
		}

		protected internal virtual bool OnRequestClose()
		{
			return true;
		}

		public Task WaitUntilUnloadedAsync()
		{
			TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
			base.Unloaded += delegate
			{
				tcs.TrySetResult(null);
			};
			return tcs.Task;
		}

		public Task _WaitForCloseAsync()
		{
			TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
			if (DialogSettings.AnimateHide)
			{
				Storyboard closingStoryboard = TryFindResource("DialogCloseStoryboard") as Storyboard;
				if (closingStoryboard == null)
				{
					throw new InvalidOperationException("Unable to find the dialog closing storyboard. Did you forget to add BaseMetroDialog.xaml to your merged dictionaries?");
				}
				EventHandler handler = null;
				handler = delegate
				{
					closingStoryboard.Completed -= handler;
					tcs.TrySetResult(null);
				};
				closingStoryboard = closingStoryboard.Clone();
				closingStoryboard.Completed += handler;
				closingStoryboard.Begin(this);
			}
			else
			{
				base.Opacity = 0.0;
				tcs.TrySetResult(null);
			}
			return tcs.Task;
		}

		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new MetroDialogAutomationPeer(this);
		}
	}
}
