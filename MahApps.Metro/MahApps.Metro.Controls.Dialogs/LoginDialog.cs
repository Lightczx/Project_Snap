using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace MahApps.Metro.Controls.Dialogs
{
	public class LoginDialog : BaseMetroDialog, IComponentConnector
	{
		public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(LoginDialog), new PropertyMetadata((object)null));

		public static readonly DependencyProperty UsernameProperty = DependencyProperty.Register("Username", typeof(string), typeof(LoginDialog), new PropertyMetadata((object)null));

		public static readonly DependencyProperty UsernameWatermarkProperty = DependencyProperty.Register("UsernameWatermark", typeof(string), typeof(LoginDialog), new PropertyMetadata((object)null));

		public static readonly DependencyProperty UsernameCharacterCasingProperty = DependencyProperty.Register("UsernameCharacterCasing", typeof(CharacterCasing), typeof(LoginDialog), new PropertyMetadata(CharacterCasing.Normal));

		public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(string), typeof(LoginDialog), new PropertyMetadata((object)null));

		public static readonly DependencyProperty PasswordWatermarkProperty = DependencyProperty.Register("PasswordWatermark", typeof(string), typeof(LoginDialog), new PropertyMetadata((object)null));

		public static readonly DependencyProperty AffirmativeButtonTextProperty = DependencyProperty.Register("AffirmativeButtonText", typeof(string), typeof(LoginDialog), new PropertyMetadata("OK"));

		public static readonly DependencyProperty NegativeButtonTextProperty = DependencyProperty.Register("NegativeButtonText", typeof(string), typeof(LoginDialog), new PropertyMetadata("Cancel"));

		public static readonly DependencyProperty NegativeButtonButtonVisibilityProperty = DependencyProperty.Register("NegativeButtonButtonVisibility", typeof(Visibility), typeof(LoginDialog), new PropertyMetadata(Visibility.Collapsed));

		public static readonly DependencyProperty ShouldHideUsernameProperty = DependencyProperty.Register("ShouldHideUsername", typeof(bool), typeof(LoginDialog), new PropertyMetadata(false));

		public static readonly DependencyProperty RememberCheckBoxVisibilityProperty = DependencyProperty.Register("RememberCheckBoxVisibility", typeof(Visibility), typeof(LoginDialog), new PropertyMetadata(Visibility.Collapsed));

		public static readonly DependencyProperty RememberCheckBoxTextProperty = DependencyProperty.Register("RememberCheckBoxText", typeof(string), typeof(LoginDialog), new PropertyMetadata("Remember"));

		public static readonly DependencyProperty RememberCheckBoxCheckedProperty = DependencyProperty.Register("RememberCheckBoxChecked", typeof(bool), typeof(LoginDialog), new PropertyMetadata(false));

		internal TextBox PART_TextBox;

		internal PasswordBox PART_TextBox2;

		internal CheckBox PART_RememberCheckBox;

		internal Button PART_AffirmativeButton;

		internal Button PART_NegativeButton;

		private bool _contentLoaded;

		public string Message
		{
			get
			{
				return (string)GetValue(MessageProperty);
			}
			set
			{
				SetValue(MessageProperty, value);
			}
		}

		public string Username
		{
			get
			{
				return (string)GetValue(UsernameProperty);
			}
			set
			{
				SetValue(UsernameProperty, value);
			}
		}

		public string Password
		{
			get
			{
				return (string)GetValue(PasswordProperty);
			}
			set
			{
				SetValue(PasswordProperty, value);
			}
		}

		public string UsernameWatermark
		{
			get
			{
				return (string)GetValue(UsernameWatermarkProperty);
			}
			set
			{
				SetValue(UsernameWatermarkProperty, value);
			}
		}

		public CharacterCasing UsernameCharacterCasing
		{
			get
			{
				return (CharacterCasing)GetValue(UsernameCharacterCasingProperty);
			}
			set
			{
				SetValue(UsernameCharacterCasingProperty, value);
			}
		}

		public string PasswordWatermark
		{
			get
			{
				return (string)GetValue(PasswordWatermarkProperty);
			}
			set
			{
				SetValue(PasswordWatermarkProperty, value);
			}
		}

		public string AffirmativeButtonText
		{
			get
			{
				return (string)GetValue(AffirmativeButtonTextProperty);
			}
			set
			{
				SetValue(AffirmativeButtonTextProperty, value);
			}
		}

		public string NegativeButtonText
		{
			get
			{
				return (string)GetValue(NegativeButtonTextProperty);
			}
			set
			{
				SetValue(NegativeButtonTextProperty, value);
			}
		}

		public Visibility NegativeButtonButtonVisibility
		{
			get
			{
				return (Visibility)GetValue(NegativeButtonButtonVisibilityProperty);
			}
			set
			{
				SetValue(NegativeButtonButtonVisibilityProperty, value);
			}
		}

		public bool ShouldHideUsername
		{
			get
			{
				return (bool)GetValue(ShouldHideUsernameProperty);
			}
			set
			{
				SetValue(ShouldHideUsernameProperty, value);
			}
		}

		public Visibility RememberCheckBoxVisibility
		{
			get
			{
				return (Visibility)GetValue(RememberCheckBoxVisibilityProperty);
			}
			set
			{
				SetValue(RememberCheckBoxVisibilityProperty, value);
			}
		}

		public string RememberCheckBoxText
		{
			get
			{
				return (string)GetValue(RememberCheckBoxTextProperty);
			}
			set
			{
				SetValue(RememberCheckBoxTextProperty, value);
			}
		}

		public bool RememberCheckBoxChecked
		{
			get
			{
				return (bool)GetValue(RememberCheckBoxCheckedProperty);
			}
			set
			{
				SetValue(RememberCheckBoxCheckedProperty, value);
			}
		}

		internal LoginDialog()
			: this(null)
		{
		}

		internal LoginDialog(MetroWindow parentWindow)
			: this(parentWindow, null)
		{
		}

		internal LoginDialog(MetroWindow parentWindow, LoginDialogSettings settings)
			: base(parentWindow, settings)
		{
			InitializeComponent();
			Username = settings.InitialUsername;
			Password = settings.InitialPassword;
			UsernameCharacterCasing = settings.UsernameCharacterCasing;
			UsernameWatermark = settings.UsernameWatermark;
			PasswordWatermark = settings.PasswordWatermark;
			NegativeButtonButtonVisibility = settings.NegativeButtonVisibility;
			ShouldHideUsername = settings.ShouldHideUsername;
			RememberCheckBoxVisibility = settings.RememberCheckBoxVisibility;
			RememberCheckBoxText = settings.RememberCheckBoxText;
		}

		internal Task<LoginDialogData> WaitForButtonPressAsync()
		{
			base.Dispatcher.BeginInvoke((Action)delegate
			{
				Focus();
				if (string.IsNullOrEmpty(PART_TextBox.Text) && !ShouldHideUsername)
				{
					PART_TextBox.Focus();
				}
				else
				{
					PART_TextBox2.Focus();
				}
			});
			TaskCompletionSource<LoginDialogData> tcs = new TaskCompletionSource<LoginDialogData>();
			RoutedEventHandler negativeHandler = null;
			KeyEventHandler negativeKeyHandler = null;
			RoutedEventHandler affirmativeHandler = null;
			KeyEventHandler affirmativeKeyHandler = null;
			KeyEventHandler escapeKeyHandler = null;
			Action cleanUpHandlers = null;
			CancellationTokenRegistration cancellationTokenRegistration = base.DialogSettings.CancellationToken.Register(delegate
			{
				cleanUpHandlers();
				tcs.TrySetResult(null);
			});
			cleanUpHandlers = delegate
			{
				PART_TextBox.KeyDown -= affirmativeKeyHandler;
				PART_TextBox2.KeyDown -= affirmativeKeyHandler;
				base.KeyDown -= escapeKeyHandler;
				PART_NegativeButton.Click -= negativeHandler;
				PART_AffirmativeButton.Click -= affirmativeHandler;
				PART_NegativeButton.KeyDown -= negativeKeyHandler;
				PART_AffirmativeButton.KeyDown -= affirmativeKeyHandler;
				cancellationTokenRegistration.Dispose();
			};
			escapeKeyHandler = delegate(object sender, KeyEventArgs e)
			{
				if (e.Key == Key.Escape)
				{
					cleanUpHandlers();
					tcs.TrySetResult(null);
				}
			};
			negativeKeyHandler = delegate(object sender, KeyEventArgs e)
			{
				if (e.Key == Key.Return)
				{
					cleanUpHandlers();
					tcs.TrySetResult(null);
				}
			};
			affirmativeKeyHandler = delegate(object sender, KeyEventArgs e)
			{
				if (e.Key == Key.Return)
				{
					cleanUpHandlers();
					tcs.TrySetResult(new LoginDialogData
					{
						Username = Username,
						SecurePassword = PART_TextBox2.SecurePassword,
						ShouldRemember = RememberCheckBoxChecked
					});
				}
			};
			negativeHandler = delegate(object sender, RoutedEventArgs e)
			{
				cleanUpHandlers();
				tcs.TrySetResult(null);
				e.Handled = true;
			};
			affirmativeHandler = delegate(object sender, RoutedEventArgs e)
			{
				cleanUpHandlers();
				tcs.TrySetResult(new LoginDialogData
				{
					Username = Username,
					SecurePassword = PART_TextBox2.SecurePassword,
					ShouldRemember = RememberCheckBoxChecked
				});
				e.Handled = true;
			};
			PART_NegativeButton.KeyDown += negativeKeyHandler;
			PART_AffirmativeButton.KeyDown += affirmativeKeyHandler;
			PART_TextBox.KeyDown += affirmativeKeyHandler;
			PART_TextBox2.KeyDown += affirmativeKeyHandler;
			base.KeyDown += escapeKeyHandler;
			PART_NegativeButton.Click += negativeHandler;
			PART_AffirmativeButton.Click += affirmativeHandler;
			return tcs.Task;
		}

		protected override void OnLoaded()
		{
			LoginDialogSettings loginDialogSettings = base.DialogSettings as LoginDialogSettings;
			if (loginDialogSettings != null && loginDialogSettings.EnablePasswordPreview)
			{
				Style style = FindResource("Win8MetroPasswordBox") as Style;
				if (style != null)
				{
					PART_TextBox2.Style = style;
					PART_TextBox2.ApplyTemplate();
				}
			}
			AffirmativeButtonText = base.DialogSettings.AffirmativeButtonText;
			NegativeButtonText = base.DialogSettings.NegativeButtonText;
			MetroDialogColorScheme colorScheme = base.DialogSettings.ColorScheme;
			if (colorScheme == MetroDialogColorScheme.Accented)
			{
				PART_NegativeButton.SetResourceReference(FrameworkElement.StyleProperty, "AccentedDialogHighlightedSquareButton");
				PART_TextBox.SetResourceReference(Control.ForegroundProperty, "BlackColorBrush");
				PART_TextBox2.SetResourceReference(Control.ForegroundProperty, "BlackColorBrush");
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/MahApps.Metro;component/themes/dialogs/logindialog.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				PART_TextBox = (TextBox)target;
				break;
			case 2:
				PART_TextBox2 = (PasswordBox)target;
				break;
			case 3:
				PART_RememberCheckBox = (CheckBox)target;
				break;
			case 4:
				PART_AffirmativeButton = (Button)target;
				break;
			case 5:
				PART_NegativeButton = (Button)target;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
}
