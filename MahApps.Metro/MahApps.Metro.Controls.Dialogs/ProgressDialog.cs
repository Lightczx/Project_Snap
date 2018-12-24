using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace MahApps.Metro.Controls.Dialogs
{
	public class ProgressDialog : BaseMetroDialog, IComponentConnector
	{
		public static readonly DependencyProperty ProgressBarForegroundProperty = DependencyProperty.Register("ProgressBarForeground", typeof(Brush), typeof(ProgressDialog), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

		public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(ProgressDialog), new PropertyMetadata((object)null));

		public static readonly DependencyProperty IsCancelableProperty = DependencyProperty.Register("IsCancelable", typeof(bool), typeof(ProgressDialog), new PropertyMetadata(false, delegate(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			((ProgressDialog)s).PART_NegativeButton.Visibility = ((!(bool)e.NewValue) ? Visibility.Hidden : Visibility.Visible);
		}));

		public static readonly DependencyProperty NegativeButtonTextProperty = DependencyProperty.Register("NegativeButtonText", typeof(string), typeof(ProgressDialog), new PropertyMetadata("Cancel"));

		internal Button PART_NegativeButton;

		internal MetroProgressBar PART_ProgressBar;

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

		public bool IsCancelable
		{
			get
			{
				return (bool)GetValue(IsCancelableProperty);
			}
			set
			{
				SetValue(IsCancelableProperty, value);
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

		public Brush ProgressBarForeground
		{
			get
			{
				return (Brush)GetValue(ProgressBarForegroundProperty);
			}
			set
			{
				SetValue(ProgressBarForegroundProperty, value);
			}
		}

		internal CancellationToken CancellationToken => base.DialogSettings.CancellationToken;

		internal double Minimum
		{
			get
			{
				return PART_ProgressBar.Minimum;
			}
			set
			{
				PART_ProgressBar.Minimum = value;
			}
		}

		internal double Maximum
		{
			get
			{
				return PART_ProgressBar.Maximum;
			}
			set
			{
				PART_ProgressBar.Maximum = value;
			}
		}

		internal double ProgressValue
		{
			get
			{
				return PART_ProgressBar.Value;
			}
			set
			{
				PART_ProgressBar.IsIndeterminate = false;
				PART_ProgressBar.Value = value;
				PART_ProgressBar.ApplyTemplate();
			}
		}

		internal ProgressDialog()
			: this(null)
		{
		}

		internal ProgressDialog(MetroWindow parentWindow)
			: this(parentWindow, null)
		{
		}

		internal ProgressDialog(MetroWindow parentWindow, MetroDialogSettings settings)
			: base(parentWindow, settings)
		{
			InitializeComponent();
		}

		protected override void OnLoaded()
		{
			NegativeButtonText = base.DialogSettings.NegativeButtonText;
			SetResourceReference(ProgressBarForegroundProperty, (base.DialogSettings.ColorScheme == MetroDialogColorScheme.Theme) ? "AccentColorBrush" : "BlackBrush");
		}

		internal void SetIndeterminate()
		{
			PART_ProgressBar.IsIndeterminate = true;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/MahApps.Metro;component/themes/dialogs/progressdialog.xaml", UriKind.Relative);
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
				PART_NegativeButton = (Button)target;
				break;
			case 2:
				PART_ProgressBar = (MetroProgressBar)target;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
}
