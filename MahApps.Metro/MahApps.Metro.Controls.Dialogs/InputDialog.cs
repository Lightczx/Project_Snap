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
	public class InputDialog : BaseMetroDialog, IComponentConnector
	{
		public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(InputDialog), new PropertyMetadata((object)null));

		public static readonly DependencyProperty InputProperty = DependencyProperty.Register("Input", typeof(string), typeof(InputDialog), new PropertyMetadata((object)null));

		public static readonly DependencyProperty AffirmativeButtonTextProperty = DependencyProperty.Register("AffirmativeButtonText", typeof(string), typeof(InputDialog), new PropertyMetadata("OK"));

		public static readonly DependencyProperty NegativeButtonTextProperty = DependencyProperty.Register("NegativeButtonText", typeof(string), typeof(InputDialog), new PropertyMetadata("Cancel"));

		internal TextBox PART_TextBox;

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

		public string Input
		{
			get
			{
				return (string)GetValue(InputProperty);
			}
			set
			{
				SetValue(InputProperty, value);
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

		internal InputDialog()
			: this(null)
		{
		}

		internal InputDialog(MetroWindow parentWindow)
			: this(parentWindow, null)
		{
		}

		internal InputDialog(MetroWindow parentWindow, MetroDialogSettings settings)
			: base(parentWindow, settings)
		{
			InitializeComponent();
		}

		internal Task<string> WaitForButtonPressAsync()
		{
			base.Dispatcher.BeginInvoke((Action)delegate
			{
				Focus();
				PART_TextBox.Focus();
			});
			TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
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
					tcs.TrySetResult(Input);
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
				tcs.TrySetResult(Input);
				e.Handled = true;
			};
			PART_NegativeButton.KeyDown += negativeKeyHandler;
			PART_AffirmativeButton.KeyDown += affirmativeKeyHandler;
			PART_TextBox.KeyDown += affirmativeKeyHandler;
			base.KeyDown += escapeKeyHandler;
			PART_NegativeButton.Click += negativeHandler;
			PART_AffirmativeButton.Click += affirmativeHandler;
			return tcs.Task;
		}

		protected override void OnLoaded()
		{
			AffirmativeButtonText = base.DialogSettings.AffirmativeButtonText;
			NegativeButtonText = base.DialogSettings.NegativeButtonText;
			MetroDialogColorScheme colorScheme = base.DialogSettings.ColorScheme;
			if (colorScheme == MetroDialogColorScheme.Accented)
			{
				PART_NegativeButton.SetResourceReference(FrameworkElement.StyleProperty, "AccentedDialogHighlightedSquareButton");
				PART_TextBox.SetResourceReference(Control.ForegroundProperty, "BlackColorBrush");
				PART_TextBox.SetResourceReference(ControlsHelper.FocusBorderBrushProperty, "TextBoxFocusBorderBrush");
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/MahApps.Metro;component/themes/dialogs/inputdialog.xaml", UriKind.Relative);
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
				PART_AffirmativeButton = (Button)target;
				break;
			case 3:
				PART_NegativeButton = (Button)target;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
}
