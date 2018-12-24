using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace MahApps.Metro.Controls
{
	[ContentProperty("OverlayContent")]
	public class MetroNavigationWindow : MetroWindow, IUriContext, IComponentConnector
	{
		public static readonly DependencyProperty OverlayContentProperty = DependencyProperty.Register("OverlayContent", typeof(object), typeof(MetroNavigationWindow));

		public static readonly DependencyProperty PageContentProperty = DependencyProperty.Register("PageContent", typeof(object), typeof(MetroNavigationWindow));

		internal Button PART_BackButton;

		internal Button PART_ForwardButton;

		internal Label PART_Title;

		internal Frame PART_Frame;

		private bool _contentLoaded;

		public object OverlayContent
		{
			get
			{
				return GetValue(OverlayContentProperty);
			}
			set
			{
				SetValue(OverlayContentProperty, value);
			}
		}

		public object PageContent
		{
			get
			{
				return GetValue(PageContentProperty);
			}
			private set
			{
				SetValue(PageContentProperty, value);
			}
		}

		public IEnumerable ForwardStack => PART_Frame.ForwardStack;

		public IEnumerable BackStack => PART_Frame.BackStack;

		public NavigationService NavigationService => PART_Frame.NavigationService;

		public bool CanGoBack => PART_Frame.CanGoBack;

		public bool CanGoForward => PART_Frame.CanGoForward;

		Uri IUriContext.BaseUri
		{
			get;
			set;
		}

		public Uri Source
		{
			get
			{
				return PART_Frame.Source;
			}
			set
			{
				PART_Frame.Source = value;
			}
		}

		public event FragmentNavigationEventHandler FragmentNavigation;

		public event NavigatingCancelEventHandler Navigating;

		public event NavigationFailedEventHandler NavigationFailed;

		public event NavigationProgressEventHandler NavigationProgress;

		public event NavigationStoppedEventHandler NavigationStopped;

		public event NavigatedEventHandler Navigated;

		public event LoadCompletedEventHandler LoadCompleted;

		public MetroNavigationWindow()
		{
			InitializeComponent();
			base.Loaded += MetroNavigationWindow_Loaded;
			base.Closing += MetroNavigationWindow_Closing;
		}

		private void MetroNavigationWindow_Loaded(object sender, RoutedEventArgs e)
		{
			PART_Frame.Navigated += PART_Frame_Navigated;
			PART_Frame.Navigating += PART_Frame_Navigating;
			PART_Frame.NavigationFailed += PART_Frame_NavigationFailed;
			PART_Frame.NavigationProgress += PART_Frame_NavigationProgress;
			PART_Frame.NavigationStopped += PART_Frame_NavigationStopped;
			PART_Frame.LoadCompleted += PART_Frame_LoadCompleted;
			PART_Frame.FragmentNavigation += PART_Frame_FragmentNavigation;
			PART_BackButton.Click += PART_BackButton_Click;
			PART_ForwardButton.Click += PART_ForwardButton_Click;
		}

		[DebuggerNonUserCode]
		private void PART_ForwardButton_Click(object sender, RoutedEventArgs e)
		{
			if (CanGoForward)
			{
				GoForward();
			}
		}

		[DebuggerNonUserCode]
		private void PART_Frame_FragmentNavigation(object sender, FragmentNavigationEventArgs e)
		{
			if (this.FragmentNavigation != null)
			{
				this.FragmentNavigation(this, e);
			}
		}

		[DebuggerNonUserCode]
		private void PART_Frame_LoadCompleted(object sender, NavigationEventArgs e)
		{
			if (this.LoadCompleted != null)
			{
				this.LoadCompleted(this, e);
			}
		}

		[DebuggerNonUserCode]
		private void PART_Frame_NavigationStopped(object sender, NavigationEventArgs e)
		{
			if (this.NavigationStopped != null)
			{
				this.NavigationStopped(this, e);
			}
		}

		[DebuggerNonUserCode]
		private void PART_Frame_NavigationProgress(object sender, NavigationProgressEventArgs e)
		{
			if (this.NavigationProgress != null)
			{
				this.NavigationProgress(this, e);
			}
		}

		[DebuggerNonUserCode]
		private void PART_Frame_NavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			if (this.NavigationFailed != null)
			{
				this.NavigationFailed(this, e);
			}
		}

		[DebuggerNonUserCode]
		private void PART_Frame_Navigating(object sender, NavigatingCancelEventArgs e)
		{
			if (this.Navigating != null)
			{
				this.Navigating(this, e);
			}
		}

		[DebuggerNonUserCode]
		private void PART_BackButton_Click(object sender, RoutedEventArgs e)
		{
			if (CanGoBack)
			{
				GoBack();
			}
		}

		[DebuggerNonUserCode]
		private void MetroNavigationWindow_Closing(object sender, CancelEventArgs e)
		{
			PART_Frame.FragmentNavigation -= PART_Frame_FragmentNavigation;
			PART_Frame.Navigating -= PART_Frame_Navigating;
			PART_Frame.NavigationFailed -= PART_Frame_NavigationFailed;
			PART_Frame.NavigationProgress -= PART_Frame_NavigationProgress;
			PART_Frame.NavigationStopped -= PART_Frame_NavigationStopped;
			PART_Frame.LoadCompleted -= PART_Frame_LoadCompleted;
			PART_Frame.Navigated -= PART_Frame_Navigated;
			PART_ForwardButton.Click -= PART_ForwardButton_Click;
			PART_BackButton.Click -= PART_BackButton_Click;
			base.Loaded -= MetroNavigationWindow_Loaded;
			base.Closing -= MetroNavigationWindow_Closing;
		}

		[DebuggerNonUserCode]
		private void PART_Frame_Navigated(object sender, NavigationEventArgs e)
		{
			PART_Title.Content = ((Page)PART_Frame.Content).Title;
			((IUriContext)this).BaseUri = e.Uri;
			PageContent = PART_Frame.Content;
			PART_BackButton.IsEnabled = CanGoBack;
			PART_ForwardButton.IsEnabled = CanGoForward;
			if (this.Navigated != null)
			{
				this.Navigated(this, e);
			}
		}

		[DebuggerNonUserCode]
		public void AddBackEntry(CustomContentState state)
		{
			PART_Frame.AddBackEntry(state);
		}

		[DebuggerNonUserCode]
		public JournalEntry RemoveBackEntry()
		{
			return PART_Frame.RemoveBackEntry();
		}

		[DebuggerNonUserCode]
		public void GoBack()
		{
			PART_Frame.GoBack();
		}

		[DebuggerNonUserCode]
		public void GoForward()
		{
			PART_Frame.GoForward();
		}

		[DebuggerNonUserCode]
		public bool Navigate(object content)
		{
			return PART_Frame.Navigate(content);
		}

		[DebuggerNonUserCode]
		public bool Navigate(Uri source)
		{
			return PART_Frame.Navigate(source);
		}

		[DebuggerNonUserCode]
		public bool Navigate(object content, object extraData)
		{
			return PART_Frame.Navigate(content, extraData);
		}

		[DebuggerNonUserCode]
		public bool Navigate(Uri source, object extraData)
		{
			return PART_Frame.Navigate(source, extraData);
		}

		[DebuggerNonUserCode]
		public void StopLoading()
		{
			PART_Frame.StopLoading();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/MahApps.Metro;component/controls/metronavigationwindow.xaml", UriKind.Relative);
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
				PART_BackButton = (Button)target;
				break;
			case 2:
				PART_ForwardButton = (Button)target;
				break;
			case 3:
				PART_Title = (Label)target;
				break;
			case 4:
				PART_Frame = (Frame)target;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
}
