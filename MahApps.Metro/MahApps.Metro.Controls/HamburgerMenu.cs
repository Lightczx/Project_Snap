using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MahApps.Metro.Controls
{
	[TemplatePart(Name = "HamburgerButton", Type = typeof(Button))]
	[TemplatePart(Name = "ButtonsListView", Type = typeof(ListBox))]
	[TemplatePart(Name = "OptionsListView", Type = typeof(ListBox))]
	public class HamburgerMenu : ContentControl
	{
		private Button _hamburgerButton;

		private ListBox _buttonsListView;

		private ListBox _optionsListView;

		public static readonly DependencyProperty HamburgerWidthProperty = DependencyProperty.Register("HamburgerWidth", typeof(double), typeof(HamburgerMenu), new PropertyMetadata(48.0));

		public static readonly DependencyProperty HamburgerHeightProperty = DependencyProperty.Register("HamburgerHeight", typeof(double), typeof(HamburgerMenu), new PropertyMetadata(48.0));

		public static readonly DependencyProperty HamburgerMarginProperty = DependencyProperty.Register("HamburgerMargin", typeof(Thickness), typeof(HamburgerMenu), new PropertyMetadata(null));

		public static readonly DependencyProperty HamburgerVisibilityProperty = DependencyProperty.Register("HamburgerVisibility", typeof(Visibility), typeof(HamburgerMenu), new PropertyMetadata(Visibility.Visible));

		public static readonly DependencyProperty HamburgerMenuTemplateProperty = DependencyProperty.Register("HamburgerMenuTemplate", typeof(DataTemplate), typeof(HamburgerMenu), new PropertyMetadata(null));

		public static readonly DependencyProperty HamburgerMenuHeaderTemplateProperty = DependencyProperty.Register("HamburgerMenuHeaderTemplate", typeof(DataTemplate), typeof(HamburgerMenu), new PropertyMetadata(null));

		public static readonly DependencyProperty OptionsItemsSourceProperty = DependencyProperty.Register("OptionsItemsSource", typeof(object), typeof(HamburgerMenu), new PropertyMetadata(null));

		public static readonly DependencyProperty OptionsItemContainerStyleProperty = DependencyProperty.Register("OptionsItemContainerStyle", typeof(Style), typeof(HamburgerMenu), new PropertyMetadata(null));

		public static readonly DependencyProperty OptionsItemTemplateProperty = DependencyProperty.Register("OptionsItemTemplate", typeof(DataTemplate), typeof(HamburgerMenu), new PropertyMetadata(null));

		public static readonly DependencyProperty OptionsItemTemplateSelectorProperty = DependencyProperty.Register("OptionsItemTemplateSelector", typeof(DataTemplateSelector), typeof(HamburgerMenu), new PropertyMetadata(null));

		public static readonly DependencyProperty OptionsVisibilityProperty = DependencyProperty.Register("OptionsVisibility", typeof(Visibility), typeof(HamburgerMenu), new PropertyMetadata(Visibility.Visible));

		public static readonly DependencyProperty SelectedOptionsItemProperty = DependencyProperty.Register("SelectedOptionsItem", typeof(object), typeof(HamburgerMenu), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public static readonly DependencyProperty SelectedOptionsIndexProperty = DependencyProperty.Register("SelectedOptionsIndex", typeof(int), typeof(HamburgerMenu), new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal));

		public static readonly DependencyProperty OptionsItemCommandProperty = DependencyProperty.Register("OptionsItemCommand", typeof(ICommand), typeof(HamburgerMenu), new PropertyMetadata(null));

		public static readonly DependencyProperty OptionsItemCommandParameterProperty = DependencyProperty.Register("OptionsItemCommandParameter", typeof(object), typeof(HamburgerMenu), new PropertyMetadata(null));

		private ControlTemplate _defaultItemFocusVisualTemplate;

		public static readonly DependencyProperty OpenPaneLengthProperty = DependencyProperty.Register("OpenPaneLength", typeof(double), typeof(HamburgerMenu), new PropertyMetadata(240.0, OpenPaneLengthPropertyChangedCallback));

		public static readonly DependencyProperty PanePlacementProperty = DependencyProperty.Register("PanePlacement", typeof(SplitViewPanePlacement), typeof(HamburgerMenu), new PropertyMetadata(SplitViewPanePlacement.Left));

		public static readonly DependencyProperty DisplayModeProperty = DependencyProperty.Register("DisplayMode", typeof(SplitViewDisplayMode), typeof(HamburgerMenu), new PropertyMetadata(SplitViewDisplayMode.CompactInline));

		public static readonly DependencyProperty CompactPaneLengthProperty = DependencyProperty.Register("CompactPaneLength", typeof(double), typeof(HamburgerMenu), new PropertyMetadata(48.0, CompactPaneLengthPropertyChangedCallback));

		public static readonly DependencyProperty PaneBackgroundProperty = DependencyProperty.Register("PaneBackground", typeof(Brush), typeof(HamburgerMenu), new PropertyMetadata(null));

		public static readonly DependencyProperty PaneForegroundProperty = DependencyProperty.Register("PaneForeground", typeof(Brush), typeof(HamburgerMenu), new PropertyMetadata(null));

		public static readonly DependencyProperty IsPaneOpenProperty = DependencyProperty.Register("IsPaneOpen", typeof(bool), typeof(HamburgerMenu), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsPaneOpenPropertyChangedCallback));

		public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(object), typeof(HamburgerMenu), new PropertyMetadata(null));

		public static readonly DependencyProperty ItemContainerStyleProperty = DependencyProperty.Register("ItemContainerStyle", typeof(Style), typeof(HamburgerMenu), new PropertyMetadata(null));

		public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(HamburgerMenu), new PropertyMetadata(null));

		public static readonly DependencyProperty ItemTemplateSelectorProperty = DependencyProperty.Register("ItemTemplateSelector", typeof(DataTemplateSelector), typeof(HamburgerMenu), new PropertyMetadata(null));

		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(HamburgerMenu), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(HamburgerMenu), new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal));

		public static readonly DependencyProperty ContentTransitionProperty = DependencyProperty.Register("ContentTransition", typeof(TransitionType), typeof(HamburgerMenu), new FrameworkPropertyMetadata(TransitionType.Normal));

		public static readonly DependencyProperty ItemCommandProperty = DependencyProperty.Register("ItemCommand", typeof(ICommand), typeof(HamburgerMenu), new PropertyMetadata(null));

		public static readonly DependencyProperty ItemCommandParameterProperty = DependencyProperty.Register("ItemCommandParameter", typeof(object), typeof(HamburgerMenu), new PropertyMetadata(null));

		public static readonly DependencyProperty VerticalScrollBarOnLeftSideProperty = DependencyProperty.Register("VerticalScrollBarOnLeftSide", typeof(bool), typeof(HamburgerMenu), new PropertyMetadata(false));

		public static readonly DependencyProperty ShowSelectionIndicatorProperty = DependencyProperty.Register("ShowSelectionIndicator", typeof(bool), typeof(HamburgerMenu), new PropertyMetadata(false));

		public static readonly DependencyPropertyKey ItemFocusVisualStylePropertyKey = DependencyProperty.RegisterReadOnly("ItemFocusVisualStyle", typeof(Style), typeof(HamburgerMenu), new PropertyMetadata(null));

		public static readonly DependencyProperty ItemFocusVisualStyleProperty = ItemFocusVisualStylePropertyKey.DependencyProperty;

		public DataTemplate HamburgerMenuTemplate
		{
			get
			{
				return (DataTemplate)GetValue(HamburgerMenuTemplateProperty);
			}
			set
			{
				SetValue(HamburgerMenuTemplateProperty, value);
			}
		}

		public DataTemplate HamburgerMenuHeaderTemplate
		{
			get
			{
				return (DataTemplate)GetValue(HamburgerMenuHeaderTemplateProperty);
			}
			set
			{
				SetValue(HamburgerMenuHeaderTemplateProperty, value);
			}
		}

		public double HamburgerWidth
		{
			get
			{
				return (double)GetValue(HamburgerWidthProperty);
			}
			set
			{
				SetValue(HamburgerWidthProperty, value);
			}
		}

		public double HamburgerHeight
		{
			get
			{
				return (double)GetValue(HamburgerHeightProperty);
			}
			set
			{
				SetValue(HamburgerHeightProperty, value);
			}
		}

		public Thickness HamburgerMargin
		{
			get
			{
				return (Thickness)GetValue(HamburgerMarginProperty);
			}
			set
			{
				SetValue(HamburgerMarginProperty, value);
			}
		}

		public Visibility HamburgerVisibility
		{
			get
			{
				return (Visibility)GetValue(HamburgerVisibilityProperty);
			}
			set
			{
				SetValue(HamburgerVisibilityProperty, value);
			}
		}

		public object OptionsItemsSource
		{
			get
			{
				return GetValue(OptionsItemsSourceProperty);
			}
			set
			{
				SetValue(OptionsItemsSourceProperty, value);
			}
		}

		public Style OptionsItemContainerStyle
		{
			get
			{
				return (Style)GetValue(OptionsItemContainerStyleProperty);
			}
			set
			{
				SetValue(OptionsItemContainerStyleProperty, value);
			}
		}

		public DataTemplate OptionsItemTemplate
		{
			get
			{
				return (DataTemplate)GetValue(OptionsItemTemplateProperty);
			}
			set
			{
				SetValue(OptionsItemTemplateProperty, value);
			}
		}

		public DataTemplateSelector OptionsItemTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)GetValue(OptionsItemTemplateSelectorProperty);
			}
			set
			{
				SetValue(OptionsItemTemplateSelectorProperty, value);
			}
		}

		public ItemCollection OptionsItems
		{
			get
			{
				if (_optionsListView == null)
				{
					throw new Exception("OptionsListView is not defined yet. Please use OptionsItemsSource instead.");
				}
				return _optionsListView?.Items;
			}
		}

		public Visibility OptionsVisibility
		{
			get
			{
				return (Visibility)GetValue(OptionsVisibilityProperty);
			}
			set
			{
				SetValue(OptionsVisibilityProperty, value);
			}
		}

		public object SelectedOptionsItem
		{
			get
			{
				return GetValue(SelectedOptionsItemProperty);
			}
			set
			{
				SetValue(SelectedOptionsItemProperty, value);
			}
		}

		public int SelectedOptionsIndex
		{
			get
			{
				return (int)GetValue(SelectedOptionsIndexProperty);
			}
			set
			{
				SetValue(SelectedOptionsIndexProperty, value);
			}
		}

		public ICommand OptionsItemCommand
		{
			get
			{
				return (ICommand)GetValue(OptionsItemCommandProperty);
			}
			set
			{
				SetValue(OptionsItemCommandProperty, value);
			}
		}

		public object OptionsItemCommandParameter
		{
			get
			{
				return GetValue(OptionsItemCommandParameterProperty);
			}
			set
			{
				SetValue(OptionsItemCommandParameterProperty, value);
			}
		}

		public double OpenPaneLength
		{
			get
			{
				return (double)GetValue(OpenPaneLengthProperty);
			}
			set
			{
				SetValue(OpenPaneLengthProperty, value);
			}
		}

		public SplitViewPanePlacement PanePlacement
		{
			get
			{
				return (SplitViewPanePlacement)GetValue(PanePlacementProperty);
			}
			set
			{
				SetValue(PanePlacementProperty, value);
			}
		}

		public SplitViewDisplayMode DisplayMode
		{
			get
			{
				return (SplitViewDisplayMode)GetValue(DisplayModeProperty);
			}
			set
			{
				SetValue(DisplayModeProperty, value);
			}
		}

		public double CompactPaneLength
		{
			get
			{
				return (double)GetValue(CompactPaneLengthProperty);
			}
			set
			{
				SetValue(CompactPaneLengthProperty, value);
			}
		}

		public Brush PaneBackground
		{
			get
			{
				return (Brush)GetValue(PaneBackgroundProperty);
			}
			set
			{
				SetValue(PaneBackgroundProperty, value);
			}
		}

		public Brush PaneForeground
		{
			get
			{
				return (Brush)GetValue(PaneForegroundProperty);
			}
			set
			{
				SetValue(PaneForegroundProperty, value);
			}
		}

		public bool IsPaneOpen
		{
			get
			{
				return (bool)GetValue(IsPaneOpenProperty);
			}
			set
			{
				SetValue(IsPaneOpenProperty, value);
			}
		}

		public object ItemsSource
		{
			get
			{
				return GetValue(ItemsSourceProperty);
			}
			set
			{
				SetValue(ItemsSourceProperty, value);
			}
		}

		public Style ItemContainerStyle
		{
			get
			{
				return (Style)GetValue(ItemContainerStyleProperty);
			}
			set
			{
				SetValue(ItemContainerStyleProperty, value);
			}
		}

		public DataTemplate ItemTemplate
		{
			get
			{
				return (DataTemplate)GetValue(ItemTemplateProperty);
			}
			set
			{
				SetValue(ItemTemplateProperty, value);
			}
		}

		public DataTemplateSelector ItemTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty);
			}
			set
			{
				SetValue(ItemTemplateSelectorProperty, value);
			}
		}

		public ItemCollection Items
		{
			get
			{
				if (_buttonsListView == null)
				{
					throw new Exception("ButtonsListView is not defined yet. Please use ItemsSource instead.");
				}
				return _buttonsListView.Items;
			}
		}

		public object SelectedItem
		{
			get
			{
				return GetValue(SelectedItemProperty);
			}
			set
			{
				SetValue(SelectedItemProperty, value);
			}
		}

		public int SelectedIndex
		{
			get
			{
				return (int)GetValue(SelectedIndexProperty);
			}
			set
			{
				SetValue(SelectedIndexProperty, value);
			}
		}

		public TransitionType ContentTransition
		{
			get
			{
				return (TransitionType)GetValue(ContentTransitionProperty);
			}
			set
			{
				SetValue(ContentTransitionProperty, value);
			}
		}

		public ICommand ItemCommand
		{
			get
			{
				return (ICommand)GetValue(ItemCommandProperty);
			}
			set
			{
				SetValue(ItemCommandProperty, value);
			}
		}

		public object ItemCommandParameter
		{
			get
			{
				return GetValue(ItemCommandParameterProperty);
			}
			set
			{
				SetValue(ItemCommandParameterProperty, value);
			}
		}

		public bool VerticalScrollBarOnLeftSide
		{
			get
			{
				return (bool)GetValue(VerticalScrollBarOnLeftSideProperty);
			}
			set
			{
				SetValue(VerticalScrollBarOnLeftSideProperty, value);
			}
		}

		public bool ShowSelectionIndicator
		{
			get
			{
				return (bool)GetValue(ShowSelectionIndicatorProperty);
			}
			set
			{
				SetValue(ShowSelectionIndicatorProperty, value);
			}
		}

		public Style ItemFocusVisualStyle
		{
			get
			{
				return (Style)GetValue(ItemFocusVisualStyleProperty);
			}
			private set
			{
				SetValue(ItemFocusVisualStylePropertyKey, value);
			}
		}

		public event ItemClickEventHandler ItemClick;

		public event ItemClickEventHandler OptionsItemClick;

		public event EventHandler<HamburgerMenuItemInvokedEventArgs> ItemInvoked;

		public HamburgerMenu()
		{
			base.DefaultStyleKey = typeof(HamburgerMenu);
		}

		public override void OnApplyTemplate()
		{
			if (_hamburgerButton != null)
			{
				_hamburgerButton.Click -= HamburgerButton_Click;
			}
			if (_buttonsListView != null)
			{
				_buttonsListView.MouseUp -= ButtonsListView_ItemClick;
				_buttonsListView.SelectionChanged -= ButtonsListView_SelectionChanged;
			}
			if (_optionsListView != null)
			{
				_optionsListView.MouseUp -= OptionsListView_ItemClick;
				_optionsListView.SelectionChanged -= OptionsListView_SelectionChanged;
			}
			_hamburgerButton = (Button)GetTemplateChild("HamburgerButton");
			_buttonsListView = (ListBox)GetTemplateChild("ButtonsListView");
			_optionsListView = (ListBox)GetTemplateChild("OptionsListView");
			if (_hamburgerButton != null)
			{
				_hamburgerButton.Click += HamburgerButton_Click;
			}
			if (_buttonsListView != null)
			{
				_buttonsListView.MouseUp += ButtonsListView_ItemClick;
				_buttonsListView.SelectionChanged += ButtonsListView_SelectionChanged;
			}
			if (_optionsListView != null)
			{
				_optionsListView.MouseUp += OptionsListView_ItemClick;
				_optionsListView.SelectionChanged += OptionsListView_SelectionChanged;
			}
			ChangeItemFocusVisualStyle();
			base.Loaded -= HamburgerMenu_Loaded;
			base.Loaded += HamburgerMenu_Loaded;
			base.OnApplyTemplate();
		}

		private void HamburgerMenu_Loaded(object sender, RoutedEventArgs e)
		{
			if (GetValue(ContentControl.ContentProperty) == null)
			{
				object obj = _buttonsListView?.SelectedItem ?? _optionsListView?.SelectedItem;
				if (obj != null)
				{
					SetCurrentValue(ContentControl.ContentProperty, obj);
				}
			}
		}

		private void HamburgerButton_Click(object sender, RoutedEventArgs e)
		{
			IsPaneOpen = !IsPaneOpen;
		}

		private void OnItemClick()
		{
			if (_optionsListView != null)
			{
				_optionsListView.SelectedIndex = -1;
			}
			object selectedItem = _buttonsListView.SelectedItem;
			(selectedItem as HamburgerMenuItem)?.RaiseCommand();
			RaiseItemCommand();
			this.ItemClick?.Invoke(this, new ItemClickEventArgs(selectedItem));
			this.ItemInvoked?.Invoke(this, new HamburgerMenuItemInvokedEventArgs
			{
				InvokedItem = selectedItem,
				IsItemOptions = false
			});
		}

		private void OnOptionsItemClick()
		{
			if (_buttonsListView != null)
			{
				_buttonsListView.SelectedIndex = -1;
			}
			object selectedItem = _optionsListView.SelectedItem;
			(selectedItem as HamburgerMenuItem)?.RaiseCommand();
			RaiseOptionsItemCommand();
			this.OptionsItemClick?.Invoke(this, new ItemClickEventArgs(selectedItem));
			this.ItemInvoked?.Invoke(this, new HamburgerMenuItemInvokedEventArgs
			{
				InvokedItem = selectedItem,
				IsItemOptions = true
			});
		}

		private ListBoxItem GetClickedListBoxItem(ItemsControl itemsControl, DependencyObject dependencyObject)
		{
			if (itemsControl == null || dependencyObject == null)
			{
				return null;
			}
			return ItemsControl.ContainerFromElement(itemsControl, dependencyObject) as ListBoxItem;
		}

		private void ButtonsListView_ItemClick(object sender, MouseButtonEventArgs e)
		{
			if (GetClickedListBoxItem(sender as ItemsControl, e.OriginalSource as DependencyObject) != null)
			{
				OnItemClick();
			}
		}

		private void OptionsListView_ItemClick(object sender, MouseButtonEventArgs e)
		{
			if (GetClickedListBoxItem(sender as ItemsControl, e.OriginalSource as DependencyObject) != null)
			{
				OnOptionsItemClick();
			}
		}

		private void ButtonsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems != null && e.AddedItems.Count > 0 && (Keyboard.IsKeyToggled(Key.Space) || Keyboard.IsKeyToggled(Key.Up) || Keyboard.IsKeyToggled(Key.Prior) || Keyboard.IsKeyToggled(Key.Down) || Keyboard.IsKeyToggled(Key.Next)))
			{
				OnItemClick();
			}
		}

		private void OptionsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems != null && e.AddedItems.Count > 0 && (Keyboard.IsKeyToggled(Key.Space) || Keyboard.IsKeyToggled(Key.Up) || Keyboard.IsKeyToggled(Key.Prior) || Keyboard.IsKeyToggled(Key.Down) || Keyboard.IsKeyToggled(Key.Next)))
			{
				OnOptionsItemClick();
			}
		}

		public void RaiseOptionsItemCommand()
		{
			ICommand optionsItemCommand = OptionsItemCommand;
			object parameter = OptionsItemCommandParameter ?? this;
			if (optionsItemCommand != null && optionsItemCommand.CanExecute(parameter))
			{
				optionsItemCommand.Execute(parameter);
			}
		}

		private static void OpenPaneLengthPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			if (args.NewValue != args.OldValue)
			{
				(dependencyObject as HamburgerMenu)?.ChangeItemFocusVisualStyle();
			}
		}

		private static void CompactPaneLengthPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			if (args.NewValue != args.OldValue)
			{
				(dependencyObject as HamburgerMenu)?.ChangeItemFocusVisualStyle();
			}
		}

		private static void IsPaneOpenPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			if (args.NewValue != args.OldValue)
			{
				(dependencyObject as HamburgerMenu)?.ChangeItemFocusVisualStyle();
			}
		}

		public void RaiseItemCommand()
		{
			ICommand itemCommand = ItemCommand;
			object parameter = ItemCommandParameter ?? this;
			if (itemCommand != null && itemCommand.CanExecute(parameter))
			{
				itemCommand.Execute(parameter);
			}
		}

		private void ChangeItemFocusVisualStyle()
		{
			_defaultItemFocusVisualTemplate = (_defaultItemFocusVisualTemplate ?? (TryFindResource("HamburgerMenuItemFocusVisualTemplate") as ControlTemplate));
			if (_defaultItemFocusVisualTemplate != null)
			{
				Style style = new Style(typeof(Control));
				style.Setters.Add(new Setter(Control.TemplateProperty, _defaultItemFocusVisualTemplate));
				style.Setters.Add(new Setter(FrameworkElement.WidthProperty, IsPaneOpen ? OpenPaneLength : CompactPaneLength));
				style.Setters.Add(new Setter(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left));
				style.Seal();
				SetValue(ItemFocusVisualStylePropertyKey, style);
			}
		}
	}
}
