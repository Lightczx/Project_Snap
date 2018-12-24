using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace MahApps.Metro.Controls
{
	[ContentProperty("ItemsSource")]
	[TemplatePart(Name = "PART_Button", Type = typeof(Button))]
	[TemplatePart(Name = "PART_Image", Type = typeof(Image))]
	[TemplatePart(Name = "PART_ButtonContent", Type = typeof(ContentControl))]
	[TemplatePart(Name = "PART_Menu", Type = typeof(ContextMenu))]
	public class DropDownButton : ItemsControl
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

		public static readonly DependencyProperty ContentProperty;

		public static readonly DependencyProperty ContentTemplateProperty;

		public static readonly DependencyProperty ContentTemplateSelectorProperty;

		public static readonly DependencyProperty ContentStringFormatProperty;

		public static readonly DependencyProperty ButtonStyleProperty;

		public static readonly DependencyProperty MenuStyleProperty;

		public static readonly DependencyProperty ArrowBrushProperty;

		public static readonly DependencyProperty ArrowMouseOverBrushProperty;

		public static readonly DependencyProperty ArrowPressedBrushProperty;

		public static readonly DependencyProperty ArrowVisibilityProperty;

		private Button clickButton;

		private ContextMenu menu;

		public object Content
		{
			get
			{
				return GetValue(ContentProperty);
			}
			set
			{
				SetValue(ContentProperty, value);
			}
		}

		[Bindable(true)]
		public DataTemplate ContentTemplate
		{
			get
			{
				return (DataTemplate)GetValue(ContentTemplateProperty);
			}
			set
			{
				SetValue(ContentTemplateProperty, value);
			}
		}

		[Bindable(true)]
		public DataTemplateSelector ContentTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)GetValue(ContentTemplateSelectorProperty);
			}
			set
			{
				SetValue(ContentTemplateSelectorProperty, value);
			}
		}

		[Bindable(true)]
		public string ContentStringFormat
		{
			get
			{
				return (string)GetValue(ContentStringFormatProperty);
			}
			set
			{
				SetValue(ContentStringFormatProperty, value);
			}
		}

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

		public Style MenuStyle
		{
			get
			{
				return (Style)GetValue(MenuStyleProperty);
			}
			set
			{
				SetValue(MenuStyleProperty, value);
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

		public Visibility ArrowVisibility
		{
			get
			{
				return (Visibility)GetValue(ArrowVisibilityProperty);
			}
			set
			{
				SetValue(ArrowVisibilityProperty, value);
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

		private static void IsExpandedPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			DropDownButton obj = (DropDownButton)dependencyObject;
			obj.SetContextMenuPlacementTarget(obj.menu);
		}

		protected virtual void SetContextMenuPlacementTarget(ContextMenu contextMenu)
		{
			if (clickButton != null)
			{
				contextMenu.PlacementTarget = clickButton;
			}
		}

		static DropDownButton()
		{
			ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DropDownButton));
			IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(DropDownButton), new FrameworkPropertyMetadata(IsExpandedPropertyChangedCallback));
			ExtraTagProperty = DependencyProperty.Register("ExtraTag", typeof(object), typeof(DropDownButton));
			OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(DropDownButton), new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure));
			IconProperty = DependencyProperty.Register("Icon", typeof(object), typeof(DropDownButton));
			IconTemplateProperty = DependencyProperty.Register("IconTemplate", typeof(DataTemplate), typeof(DropDownButton));
			CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(DropDownButton));
			CommandTargetProperty = DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(DropDownButton));
			CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(DropDownButton));
			ContentProperty = DependencyProperty.Register("Content", typeof(object), typeof(DropDownButton));
			ContentTemplateProperty = DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(DropDownButton), new FrameworkPropertyMetadata((object)null));
			ContentTemplateSelectorProperty = DependencyProperty.Register("ContentTemplateSelector", typeof(DataTemplateSelector), typeof(DropDownButton), new FrameworkPropertyMetadata((object)null));
			ContentStringFormatProperty = DependencyProperty.Register("ContentStringFormat", typeof(string), typeof(DropDownButton), new FrameworkPropertyMetadata((object)null));
			ButtonStyleProperty = DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(DropDownButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));
			MenuStyleProperty = DependencyProperty.Register("MenuStyle", typeof(Style), typeof(DropDownButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));
			ArrowBrushProperty = DependencyProperty.Register("ArrowBrush", typeof(Brush), typeof(DropDownButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
			ArrowMouseOverBrushProperty = DependencyProperty.Register("ArrowMouseOverBrush", typeof(Brush), typeof(DropDownButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
			ArrowPressedBrushProperty = DependencyProperty.Register("ArrowPressedBrush", typeof(Brush), typeof(DropDownButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
			ArrowVisibilityProperty = DependencyProperty.Register("ArrowVisibility", typeof(Visibility), typeof(DropDownButton), new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DropDownButton), new FrameworkPropertyMetadata(typeof(DropDownButton)));
		}

		private void ButtonClick(object sender, RoutedEventArgs e)
		{
			SetCurrentValue(IsExpandedProperty, true);
			e.RoutedEvent = ClickEvent;
			RaiseEvent(e);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			clickButton = EnforceInstance<Button>("PART_Button");
			menu = EnforceInstance<ContextMenu>("PART_Menu");
			InitializeVisualElementsContainer();
			if (menu != null && base.Items != null && base.ItemsSource == null)
			{
				foreach (object item in (IEnumerable)base.Items)
				{
					TryRemoveVisualFromOldTree(item);
					menu.Items.Add(item);
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
			if (menu != null && base.ItemsSource == null && menu.ItemsSource == null)
			{
				switch (e.Action)
				{
				case NotifyCollectionChangedAction.Add:
					if (e.NewItems != null)
					{
						foreach (object newItem in e.NewItems)
						{
							TryRemoveVisualFromOldTree(newItem);
							menu.Items.Add(newItem);
						}
					}
					break;
				case NotifyCollectionChangedAction.Remove:
					if (e.OldItems != null)
					{
						foreach (object oldItem in e.OldItems)
						{
							menu.Items.Remove(oldItem);
						}
					}
					break;
				case NotifyCollectionChangedAction.Replace:
				case NotifyCollectionChangedAction.Move:
					if (e.OldItems != null)
					{
						foreach (object oldItem2 in e.OldItems)
						{
							menu.Items.Remove(oldItem2);
						}
					}
					if (e.NewItems != null)
					{
						foreach (object newItem2 in e.NewItems)
						{
							TryRemoveVisualFromOldTree(newItem2);
							menu.Items.Add(newItem2);
						}
					}
					break;
				case NotifyCollectionChangedAction.Reset:
					if (base.Items != null)
					{
						menu.Items.Clear();
						foreach (object item in (IEnumerable)base.Items)
						{
							TryRemoveVisualFromOldTree(item);
							menu.Items.Add(item);
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
			base.MouseRightButtonUp -= DropDownButtonMouseRightButtonUp;
			clickButton.Click -= ButtonClick;
			base.MouseRightButtonUp += DropDownButtonMouseRightButtonUp;
			clickButton.Click += ButtonClick;
		}

		private void DropDownButtonMouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
		}
	}
}
