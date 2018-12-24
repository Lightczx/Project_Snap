using ControlzEx;
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
	public class MessageDialog : BaseMetroDialog, IComponentConnector
	{
		public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(MessageDialog), new PropertyMetadata((object)null));

		public static readonly DependencyProperty AffirmativeButtonTextProperty = DependencyProperty.Register("AffirmativeButtonText", typeof(string), typeof(MessageDialog), new PropertyMetadata("OK"));

		public static readonly DependencyProperty NegativeButtonTextProperty = DependencyProperty.Register("NegativeButtonText", typeof(string), typeof(MessageDialog), new PropertyMetadata("Cancel"));

		public static readonly DependencyProperty FirstAuxiliaryButtonTextProperty = DependencyProperty.Register("FirstAuxiliaryButtonText", typeof(string), typeof(MessageDialog), new PropertyMetadata("Cancel"));

		public static readonly DependencyProperty SecondAuxiliaryButtonTextProperty = DependencyProperty.Register("SecondAuxiliaryButtonText", typeof(string), typeof(MessageDialog), new PropertyMetadata("Cancel"));

		public static readonly DependencyProperty ButtonStyleProperty = DependencyProperty.Register("ButtonStyle", typeof(MessageDialogStyle), typeof(MessageDialog), new PropertyMetadata(MessageDialogStyle.Affirmative, ButtonStylePropertyChangedCallback));

		internal ScrollViewer PART_MessageScrollViewer;

		internal TextBlock PART_MessageTextBlock;

		internal Button PART_AffirmativeButton;

		internal Button PART_NegativeButton;

		internal Button PART_FirstAuxiliaryButton;

		internal Button PART_SecondAuxiliaryButton;

		private bool _contentLoaded;

		public MessageDialogStyle ButtonStyle
		{
			get
			{
				return (MessageDialogStyle)GetValue(ButtonStyleProperty);
			}
			set
			{
				SetValue(ButtonStyleProperty, value);
			}
		}

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

		public string FirstAuxiliaryButtonText
		{
			get
			{
				return (string)GetValue(FirstAuxiliaryButtonTextProperty);
			}
			set
			{
				SetValue(FirstAuxiliaryButtonTextProperty, value);
			}
		}

		public string SecondAuxiliaryButtonText
		{
			get
			{
				return (string)GetValue(SecondAuxiliaryButtonTextProperty);
			}
			set
			{
				SetValue(SecondAuxiliaryButtonTextProperty, value);
			}
		}

		internal MessageDialog()
			: this(null)
		{
		}

		internal MessageDialog(MetroWindow parentWindow)
			: this(parentWindow, null)
		{
		}

		internal MessageDialog(MetroWindow parentWindow, MetroDialogSettings settings)
			: base(parentWindow, settings)
		{
			InitializeComponent();
			PART_MessageScrollViewer.Height = base.DialogSettings.MaximumBodyHeight;
		}

		internal Task<MessageDialogResult> WaitForButtonPressAsync()
		{
			base.Dispatcher.BeginInvoke((Action)delegate
			{
				Focus();
				MessageDialogResult messageDialogResult = base.DialogSettings.DefaultButtonFocus;
				if (!IsApplicable(messageDialogResult))
				{
					messageDialogResult = ((ButtonStyle == MessageDialogStyle.Affirmative) ? MessageDialogResult.Affirmative : MessageDialogResult.Negative);
				}
				switch (messageDialogResult)
				{
				case MessageDialogResult.Affirmative:
					PART_AffirmativeButton.SetResourceReference(FrameworkElement.StyleProperty, "AccentedDialogSquareButton");
					KeyboardNavigationEx.Focus(PART_AffirmativeButton);
					break;
				case MessageDialogResult.Negative:
					PART_NegativeButton.SetResourceReference(FrameworkElement.StyleProperty, "AccentedDialogSquareButton");
					KeyboardNavigationEx.Focus(PART_NegativeButton);
					break;
				case MessageDialogResult.FirstAuxiliary:
					PART_FirstAuxiliaryButton.SetResourceReference(FrameworkElement.StyleProperty, "AccentedDialogSquareButton");
					KeyboardNavigationEx.Focus(PART_FirstAuxiliaryButton);
					break;
				case MessageDialogResult.SecondAuxiliary:
					PART_SecondAuxiliaryButton.SetResourceReference(FrameworkElement.StyleProperty, "AccentedDialogSquareButton");
					KeyboardNavigationEx.Focus(PART_SecondAuxiliaryButton);
					break;
				}
			});
			TaskCompletionSource<MessageDialogResult> tcs = new TaskCompletionSource<MessageDialogResult>();
			RoutedEventHandler negativeHandler = null;
			KeyEventHandler negativeKeyHandler = null;
			RoutedEventHandler affirmativeHandler = null;
			KeyEventHandler affirmativeKeyHandler = null;
			RoutedEventHandler firstAuxHandler = null;
			KeyEventHandler firstAuxKeyHandler = null;
			RoutedEventHandler secondAuxHandler = null;
			KeyEventHandler secondAuxKeyHandler = null;
			KeyEventHandler escapeKeyHandler = null;
			Action cleanUpHandlers = null;
			CancellationTokenRegistration cancellationTokenRegistration = base.DialogSettings.CancellationToken.Register(delegate
			{
				cleanUpHandlers?.Invoke();
				tcs.TrySetResult((ButtonStyle == MessageDialogStyle.Affirmative) ? MessageDialogResult.Affirmative : MessageDialogResult.Negative);
			});
			cleanUpHandlers = delegate
			{
				PART_NegativeButton.Click -= negativeHandler;
				PART_AffirmativeButton.Click -= affirmativeHandler;
				PART_FirstAuxiliaryButton.Click -= firstAuxHandler;
				PART_SecondAuxiliaryButton.Click -= secondAuxHandler;
				PART_NegativeButton.KeyDown -= negativeKeyHandler;
				PART_AffirmativeButton.KeyDown -= affirmativeKeyHandler;
				PART_FirstAuxiliaryButton.KeyDown -= firstAuxKeyHandler;
				PART_SecondAuxiliaryButton.KeyDown -= secondAuxKeyHandler;
				base.KeyDown -= escapeKeyHandler;
				cancellationTokenRegistration.Dispose();
			};
			negativeKeyHandler = delegate(object sender, KeyEventArgs e)
			{
				if (e.Key == Key.Return)
				{
					cleanUpHandlers();
					tcs.TrySetResult(MessageDialogResult.Negative);
				}
			};
			affirmativeKeyHandler = delegate(object sender, KeyEventArgs e)
			{
				if (e.Key == Key.Return)
				{
					cleanUpHandlers();
					tcs.TrySetResult(MessageDialogResult.Affirmative);
				}
			};
			firstAuxKeyHandler = delegate(object sender, KeyEventArgs e)
			{
				if (e.Key == Key.Return)
				{
					cleanUpHandlers();
					tcs.TrySetResult(MessageDialogResult.FirstAuxiliary);
				}
			};
			secondAuxKeyHandler = delegate(object sender, KeyEventArgs e)
			{
				if (e.Key == Key.Return)
				{
					cleanUpHandlers();
					tcs.TrySetResult(MessageDialogResult.SecondAuxiliary);
				}
			};
			negativeHandler = delegate(object sender, RoutedEventArgs e)
			{
				cleanUpHandlers();
				tcs.TrySetResult(MessageDialogResult.Negative);
				e.Handled = true;
			};
			affirmativeHandler = delegate(object sender, RoutedEventArgs e)
			{
				cleanUpHandlers();
				tcs.TrySetResult(MessageDialogResult.Affirmative);
				e.Handled = true;
			};
			firstAuxHandler = delegate(object sender, RoutedEventArgs e)
			{
				cleanUpHandlers();
				tcs.TrySetResult(MessageDialogResult.FirstAuxiliary);
				e.Handled = true;
			};
			secondAuxHandler = delegate(object sender, RoutedEventArgs e)
			{
				cleanUpHandlers();
				tcs.TrySetResult(MessageDialogResult.SecondAuxiliary);
				e.Handled = true;
			};
			escapeKeyHandler = delegate(object sender, KeyEventArgs e)
			{
				if (e.Key == Key.Escape)
				{
					cleanUpHandlers();
					tcs.TrySetResult((MessageDialogResult)(((int?)base.DialogSettings.DialogResultOnCancel) ?? ((ButtonStyle == MessageDialogStyle.Affirmative) ? 1 : 0)));
				}
				else if (e.Key == Key.Return)
				{
					cleanUpHandlers();
					tcs.TrySetResult(MessageDialogResult.Affirmative);
				}
			};
			PART_NegativeButton.KeyDown += negativeKeyHandler;
			PART_AffirmativeButton.KeyDown += affirmativeKeyHandler;
			PART_FirstAuxiliaryButton.KeyDown += firstAuxKeyHandler;
			PART_SecondAuxiliaryButton.KeyDown += secondAuxKeyHandler;
			PART_NegativeButton.Click += negativeHandler;
			PART_AffirmativeButton.Click += affirmativeHandler;
			PART_FirstAuxiliaryButton.Click += firstAuxHandler;
			PART_SecondAuxiliaryButton.Click += secondAuxHandler;
			base.KeyDown += escapeKeyHandler;
			return tcs.Task;
		}

		private static void ButtonStylePropertyChangedCallback(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			SetButtonState((MessageDialog)s);
		}

		private static void SetButtonState(MessageDialog md)
		{
			if (md.PART_AffirmativeButton != null)
			{
				MessageDialogStyle buttonStyle = md.ButtonStyle;
				if (buttonStyle != 0)
				{
					if ((uint)(buttonStyle - 1) <= 2u)
					{
						md.PART_AffirmativeButton.Visibility = Visibility.Visible;
						md.PART_NegativeButton.Visibility = Visibility.Visible;
						if (md.ButtonStyle == MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary || md.ButtonStyle == MessageDialogStyle.AffirmativeAndNegativeAndDoubleAuxiliary)
						{
							md.PART_FirstAuxiliaryButton.Visibility = Visibility.Visible;
						}
						if (md.ButtonStyle == MessageDialogStyle.AffirmativeAndNegativeAndDoubleAuxiliary)
						{
							md.PART_SecondAuxiliaryButton.Visibility = Visibility.Visible;
						}
					}
				}
				else
				{
					md.PART_AffirmativeButton.Visibility = Visibility.Visible;
					md.PART_NegativeButton.Visibility = Visibility.Collapsed;
					md.PART_FirstAuxiliaryButton.Visibility = Visibility.Collapsed;
					md.PART_SecondAuxiliaryButton.Visibility = Visibility.Collapsed;
				}
				md.AffirmativeButtonText = md.DialogSettings.AffirmativeButtonText;
				md.NegativeButtonText = md.DialogSettings.NegativeButtonText;
				md.FirstAuxiliaryButtonText = md.DialogSettings.FirstAuxiliaryButtonText;
				md.SecondAuxiliaryButtonText = md.DialogSettings.SecondAuxiliaryButtonText;
				MetroDialogColorScheme colorScheme = md.DialogSettings.ColorScheme;
				if (colorScheme == MetroDialogColorScheme.Accented)
				{
					md.PART_AffirmativeButton.SetResourceReference(FrameworkElement.StyleProperty, "AccentedDialogHighlightedSquareButton");
					md.PART_NegativeButton.SetResourceReference(FrameworkElement.StyleProperty, "AccentedDialogHighlightedSquareButton");
					md.PART_FirstAuxiliaryButton.SetResourceReference(FrameworkElement.StyleProperty, "AccentedDialogHighlightedSquareButton");
					md.PART_SecondAuxiliaryButton.SetResourceReference(FrameworkElement.StyleProperty, "AccentedDialogHighlightedSquareButton");
				}
			}
		}

		protected override void OnLoaded()
		{
			SetButtonState(this);
		}

		private void OnKeyCopyExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			Clipboard.SetDataObject(Message);
		}

		private bool IsApplicable(MessageDialogResult value)
		{
			switch (value)
			{
			case MessageDialogResult.Affirmative:
				return PART_AffirmativeButton.IsVisible;
			case MessageDialogResult.Negative:
				return PART_NegativeButton.IsVisible;
			case MessageDialogResult.FirstAuxiliary:
				return PART_FirstAuxiliaryButton.IsVisible;
			case MessageDialogResult.SecondAuxiliary:
				return PART_SecondAuxiliaryButton.IsVisible;
			default:
				return false;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/MahApps.Metro;component/themes/dialogs/messagedialog.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CommandBinding)target).Executed += OnKeyCopyExecuted;
				break;
			case 2:
				PART_MessageScrollViewer = (ScrollViewer)target;
				break;
			case 3:
				PART_MessageTextBlock = (TextBlock)target;
				break;
			case 4:
				PART_AffirmativeButton = (Button)target;
				break;
			case 5:
				PART_NegativeButton = (Button)target;
				break;
			case 6:
				PART_FirstAuxiliaryButton = (Button)target;
				break;
			case 7:
				PART_SecondAuxiliaryButton = (Button)target;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
}
