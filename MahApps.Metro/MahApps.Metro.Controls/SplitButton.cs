using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace MahApps.Metro.Controls
{
	[ContentProperty("ItemsSource")]
	[DefaultEvent("SelectionChanged")]
	[TemplatePart(Name = "PART_Container", Type = typeof(Grid))]
	[TemplatePart(Name = "PART_Button", Type = typeof(Button))]
	[TemplatePart(Name = "PART_ButtonContent", Type = typeof(ContentControl))]
	[TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
	[TemplatePart(Name = "PART_Expander", Type = typeof(Button))]
	[TemplatePart(Name = "PART_ListBox", Type = typeof(ListBox))]
	public class SplitButton : Selector
	{
		public static readonly RoutedEvent ClickEvent;

		public static readonly DependencyProperty IsExpandedProperty;

		public static readonly DependencyProperty ExtraTagProperty;

		public static readonly DependencyProperty OrientationProperty;

		public static readonly DependencyProperty IconProperty;

		public static readonly DependencyProperty IconTemplateProperty;

		public static readonly DependencyProperty CommandProperty;

		public static readonly DependencyProperty CommandTargetProperty;

		public static readonly DependencyProperty CommandParameterProperty;

		public static readonly DependencyProperty ButtonStyleProperty;

		public static readonly DependencyProperty ButtonArrowStyleProperty;

		public static readonly DependencyProperty ListBoxStyleProperty;

		public static readonly DependencyProperty ArrowBrushProperty;

		public static readonly DependencyProperty ArrowMouseOverBrushProperty;

		public static readonly DependencyProperty ArrowPressedBrushProperty;

		private Button _clickButton;

		private Button _expander;

		private ListBox _listBox;

		private Popup _popup;

		public object CommandParameter
		{
			get
			{
				return GetValue(CommandParameterProperty);
			}
			set
			{
				SetValue(CommandParameterProperty, value);
			}
		}

		public IInputElement CommandTarget
		{
			get
			{
				return (IInputElement)GetValue(CommandTargetProperty);
			}
			set
			{
				SetValue(CommandTargetProperty, value);
			}
		}

		public ICommand Command
		{
			get
			{
				return (ICommand)GetValue(CommandProperty);
			}
			set
			{
				SetValue(CommandProperty, value);
			}
		}

		public bool IsExpanded
		{
			get
			{
				return (bool)GetValue(IsExpandedProperty);
			}
			set
			{
				SetValue(IsExpandedProperty, value);
			}
		}

		public object ExtraTag
		{
			get
			{
				return GetValue(ExtraTagProperty);
			}
			set
			{
				SetValue(ExtraTagProperty, value);
			}
		}

		public Orientation Orientation
		{
			get
			{
				return (Orientation)GetValue(OrientationProperty);
			}
			set
			{
				SetValue(OrientationProperty, value);
			}
		}

		[Bindable(true)]
		public object Icon
		{
			get
			{
				return GetValue(IconProperty);
			}
			set
			{
				SetValue(IconProperty, value);
			}
		}

		[Bindable(true)]
		public DataTemplate IconTemplate
		{
			get
			{
				return (DataTemplate)GetValue(IconTemplateProperty);
			}
			set
			{
				SetValue(IconTemplateProperty, value);
			}
		}

		public Style ButtonStyle
		{
			get
			{
				return (Style)GetValue(ButtonStyleProperty);
			}
			set
			{
				SetValue(ButtonStyleProperty, value);
			}
		}

		public Style ButtonArrowStyle
		{
			get
			{
				return (Style)GetValue(ButtonArrowStyleProperty);
			}
			set
			{
				SetValue(ButtonArrowStyleProperty, value);
			}
		}

		public Style ListBoxStyle
		{
			get
			{
				return (Style)GetValue(ListBoxStyleProperty);
			}
			set
			{
				SetValue(ListBoxStyleProperty, value);
			}
		}

		public Brush ArrowBrush
		{
			get
			{
				return (Brush)GetValue(ArrowBrushProperty);
			}
			set
			{
				SetValue(ArrowBrushProperty, value);
			}
		}

		public Brush ArrowMouseOverBrush
		{
			get
			{
				return (Brush)GetValue(ArrowMouseOverBrushProperty);
			}
			set
			{
				SetValue(ArrowMouseOverBrushProperty, value);
			}
		}

		public Brush ArrowPressedBrush
		{
			get
			{
				return (Brush)GetValue(ArrowPressedBrushProperty);
			}
			set
			{
				SetValue(ArrowPressedBrushProperty, value);
			}
		}

		public event RoutedEventHandler Click
		{
			add
			{
				AddHandler(ClickEvent, value);
			}
			remove
			{
				RemoveHandler(ClickEvent, value);
			}
		}

		static SplitButton()
		{
			ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SplitButton));
			IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(SplitButton));
			ExtraTagProperty = DependencyProperty.Register("ExtraTag", typeof(object), typeof(SplitButton));
			OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(SplitButton), new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure));
			IconProperty = DependencyProperty.Register("Icon", typeof(object), typeof(SplitButton));
			IconTemplateProperty = DependencyProperty.Register("IconTemplate", typeof(DataTemplate), typeof(SplitButton));
			CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(SplitButton));
			CommandTargetProperty = DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(SplitButton));
			CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(SplitButton));
			ButtonStyleProperty = DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(SplitButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));
			ButtonArrowStyleProperty = DependencyProperty.Register("ButtonArrowStyle", typeof(Style), typeof(SplitButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));
			ListBoxStyleProperty = DependencyProperty.Register("ListBoxStyle", typeof(Style), typeof(SplitButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));
			ArrowBrushProperty = DependencyProperty.Register("ArrowBrush", typeof(Brush), typeof(SplitButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
			ArrowMouseOverBrushProperty = DependencyProperty.Register("ArrowMouseOverBrush", typeof(Brush), typeof(SplitButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
			ArrowPressedBrushProperty = DependencyProperty.Register("ArrowPressedBrush", typeof(Brush), typeof(SplitButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitButton), new FrameworkPropertyMetadata(typeof(SplitButton)));
		}

		public SplitButton()
		{
			Mouse.AddPreviewMouseDownOutsideCapturedElementHandler(this, OutsideCapturedElementHandler);
		}

		private void ButtonClick(object sender, RoutedEventArgs e)
		{
			e.RoutedEvent = ClickEvent;
			RaiseEvent(e);
			SetCurrentValue(IsExpandedProperty, false);
		}

		private void ListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SetCurrentValue(IsExpandedProperty, false);
			e.Handled = true;
		}

		private void ExpanderClick(object sender, RoutedEventArgs e)
		{
			SetCurrentValue(IsExpandedProperty, !IsExpanded);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			_clickButton = EnforceInstance<Button>("PART_Button");
			_expander = EnforceInstance<Button>("PART_Expander");
			_listBox = EnforceInstance<ListBox>("PART_ListBox");
			_popup = EnforceInstance<Popup>("PART_Popup");
			InitializeVisualElementsContainer();
			if (_listBox != null && base.Items != null && base.ItemsSource == null)
			{
				foreach (object item in (IEnumerable)base.Items)
				{
					TryRemoveVisualFromOldTree(item);
					_listBox.Items.Add(item);
				}
			}
		}

		private void TryRemoveVisualFromOldTree(object newItem)
		{
			Visual visual = newItem as Visual;
			if (visual != null)
			{
				FrameworkElement objB = (LogicalTreeHelper.GetParent(visual) as FrameworkElement) ?? (VisualTreeHelper.GetParent(visual) as FrameworkElement);
				if (object.Equals(this, objB))
				{
					RemoveLogicalChild(visual);
					RemoveVisualChild(visual);
				}
			}
		}

		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);
			if (_listBox != null && base.ItemsSource == null && _listBox.ItemsSource == null)
			{
				switch (e.Action)
				{
				case NotifyCollectionChangedAction.Add:
					if (e.NewItems != null)
					{
						foreach (object newItem in e.NewItems)
						{
							TryRemoveVisualFromOldTree(newItem);
							_listBox.Items.Add(newItem);
						}
					}
					break;
				case NotifyCollectionChangedAction.Remove:
					if (e.OldItems != null)
					{
						foreach (object oldItem in e.OldItems)
						{
							_listBox.Items.Remove(oldItem);
						}
					}
					break;
				case NotifyCollectionChangedAction.Replace:
				case NotifyCollectionChangedAction.Move:
					if (e.OldItems != null)
					{
						foreach (object oldItem2 in e.OldItems)
						{
							_listBox.Items.Remove(oldItem2);
						}
					}
					if (e.NewItems != null)
					{
						foreach (object newItem2 in e.NewItems)
						{
							TryRemoveVisualFromOldTree(newItem2);
							_listBox.Items.Add(newItem2);
						}
					}
					break;
				case NotifyCollectionChangedAction.Reset:
					if (base.Items != null)
					{
						_listBox.Items.Clear();
						foreach (object item in (IEnumerable)base.Items)
						{
							TryRemoveVisualFromOldTree(item);
							_listBox.Items.Add(item);
						}
					}
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		private T EnforceInstance<T>(string partName) where T : FrameworkElement, new()
		{
			return (GetTemplateChild(partName) as T) ?? new T();
		}

		private void InitializeVisualElementsContainer()
		{
			_expander.Click -= ExpanderClick;
			_clickButton.Click -= ButtonClick;
			_listBox.SelectionChanged -= ListBoxSelectionChanged;
			_listBox.PreviewMouseLeftButtonDown -= ListBoxPreviewMouseLeftButtonDown;
			_popup.Opened -= PopupOpened;
			_popup.Closed -= PopupClosed;
			_expander.Click += ExpanderClick;
			_clickButton.Click += ButtonClick;
			_listBox.SelectionChanged += ListBoxSelectionChanged;
			_listBox.PreviewMouseLeftButtonDown += ListBoxPreviewMouseLeftButtonDown;
			_popup.Opened += PopupOpened;
			_popup.Closed += PopupClosed;
		}

		private void ListBoxPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			DependencyObject dependencyObject = e.OriginalSource as DependencyObject;
			if (dependencyObject != null && ItemsControl.ContainerFromElement(_listBox, dependencyObject) is ListBoxItem)
			{
				SetCurrentValue(IsExpandedProperty, false);
			}
		}

		private void PopupClosed(object sender, EventArgs e)
		{
			SetCurrentValue(IsExpandedProperty, false);
			ReleaseMouseCapture();
			Mouse.RemoveLostMouseCaptureHandler(_popup, LostMouseCaptureHandler);
			if (base.IsKeyboardFocusWithin)
			{
				_expander?.Focus();
			}
		}

		private void PopupOpened(object sender, EventArgs e)
		{
			Mouse.Capture(this, CaptureMode.SubTree);
			Mouse.AddLostMouseCaptureHandler(_popup, LostMouseCaptureHandler);
		}

		private void LostMouseCaptureHandler(object sender, MouseEventArgs e)
		{
			if (IsExpanded)
			{
				Mouse.Capture(this, CaptureMode.SubTree);
			}
		}

		private void OutsideCapturedElementHandler(object sender, MouseButtonEventArgs e)
		{
			PopupClosed(sender, e);
		}

		protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnIsKeyboardFocusWithinChanged(e);
			if (!(bool)e.NewValue)
			{
				SetCurrentValue(IsExpandedProperty, false);
			}
		}
	}
}
