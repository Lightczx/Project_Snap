using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace MahApps.Metro.Controls
{
	public class DataGridNumericUpDownColumn : DataGridBoundColumn
	{
		private static Style _defaultEditingElementStyle;

		private static Style _defaultElementStyle;

		public static readonly DependencyProperty StringFormatProperty;

		public static readonly DependencyProperty MinimumProperty;

		public static readonly DependencyProperty MaximumProperty;

		public static readonly DependencyProperty IntervalProperty;

		public static readonly DependencyProperty HideUpDownButtonsProperty;

		public static readonly DependencyProperty UpDownButtonsWidthProperty;

		public static readonly DependencyProperty FontFamilyProperty;

		public static readonly DependencyProperty FontSizeProperty;

		public static readonly DependencyProperty FontStyleProperty;

		public static readonly DependencyProperty FontWeightProperty;

		public static readonly DependencyProperty ForegroundProperty;

		public static Style DefaultEditingElementStyle
		{
			get
			{
				if (_defaultEditingElementStyle == null)
				{
					Style style = new Style(typeof(NumericUpDown));
					style.Setters.Add(new Setter(Control.BorderThicknessProperty, new Thickness(0.0)));
					style.Setters.Add(new Setter(Control.PaddingProperty, new Thickness(0.0)));
					style.Setters.Add(new Setter(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Top));
					style.Setters.Add(new Setter(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled));
					style.Setters.Add(new Setter(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled));
					style.Setters.Add(new Setter(Control.VerticalContentAlignmentProperty, VerticalAlignment.Center));
					style.Setters.Add(new Setter(FrameworkElement.MinHeightProperty, 0.0));
					style.Seal();
					_defaultEditingElementStyle = style;
				}
				return _defaultEditingElementStyle;
			}
		}

		public static Style DefaultElementStyle
		{
			get
			{
				if (_defaultElementStyle == null)
				{
					Style style = new Style(typeof(NumericUpDown));
					style.Setters.Add(new Setter(Control.BorderThicknessProperty, new Thickness(0.0)));
					style.Setters.Add(new Setter(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Top));
					style.Setters.Add(new Setter(UIElement.IsHitTestVisibleProperty, false));
					style.Setters.Add(new Setter(UIElement.FocusableProperty, false));
					style.Setters.Add(new Setter(NumericUpDown.HideUpDownButtonsProperty, true));
					style.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.Transparent));
					style.Setters.Add(new Setter(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled));
					style.Setters.Add(new Setter(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled));
					style.Setters.Add(new Setter(Control.VerticalContentAlignmentProperty, VerticalAlignment.Center));
					style.Setters.Add(new Setter(FrameworkElement.MinHeightProperty, 0.0));
					style.Setters.Add(new Setter(ControlsHelper.DisabledVisualElementVisibilityProperty, Visibility.Collapsed));
					style.Seal();
					_defaultElementStyle = style;
				}
				return _defaultElementStyle;
			}
		}

		public string StringFormat
		{
			get
			{
				return (string)GetValue(StringFormatProperty);
			}
			set
			{
				SetValue(StringFormatProperty, value);
			}
		}

		public double Minimum
		{
			get
			{
				return (double)GetValue(MinimumProperty);
			}
			set
			{
				SetValue(MinimumProperty, value);
			}
		}

		public double Maximum
		{
			get
			{
				return (double)GetValue(MaximumProperty);
			}
			set
			{
				SetValue(MaximumProperty, value);
			}
		}

		public double Interval
		{
			get
			{
				return (double)GetValue(IntervalProperty);
			}
			set
			{
				SetValue(IntervalProperty, value);
			}
		}

		public bool HideUpDownButtons
		{
			get
			{
				return (bool)GetValue(HideUpDownButtonsProperty);
			}
			set
			{
				SetValue(HideUpDownButtonsProperty, value);
			}
		}

		public double UpDownButtonsWidth
		{
			get
			{
				return (double)GetValue(UpDownButtonsWidthProperty);
			}
			set
			{
				SetValue(UpDownButtonsWidthProperty, value);
			}
		}

		public FontFamily FontFamily
		{
			get
			{
				return (FontFamily)GetValue(FontFamilyProperty);
			}
			set
			{
				SetValue(FontFamilyProperty, value);
			}
		}

		[TypeConverter(typeof(FontSizeConverter))]
		[Localizability(LocalizationCategory.None)]
		public double FontSize
		{
			get
			{
				return (double)GetValue(FontSizeProperty);
			}
			set
			{
				SetValue(FontSizeProperty, value);
			}
		}

		public FontStyle FontStyle
		{
			get
			{
				return (FontStyle)GetValue(FontStyleProperty);
			}
			set
			{
				SetValue(FontStyleProperty, value);
			}
		}

		public FontWeight FontWeight
		{
			get
			{
				return (FontWeight)GetValue(FontWeightProperty);
			}
			set
			{
				SetValue(FontWeightProperty, value);
			}
		}

		public Brush Foreground
		{
			get
			{
				return (Brush)GetValue(ForegroundProperty);
			}
			set
			{
				SetValue(ForegroundProperty, value);
			}
		}

		static DataGridNumericUpDownColumn()
		{
			StringFormatProperty = NumericUpDown.StringFormatProperty.AddOwner(typeof(DataGridNumericUpDownColumn), new FrameworkPropertyMetadata((string)NumericUpDown.StringFormatProperty.DefaultMetadata.DefaultValue, FrameworkPropertyMetadataOptions.Inherits, NotifyPropertyChangeForRefreshContent));
			MinimumProperty = NumericUpDown.MinimumProperty.AddOwner(typeof(DataGridNumericUpDownColumn), new FrameworkPropertyMetadata((double)NumericUpDown.MinimumProperty.DefaultMetadata.DefaultValue, FrameworkPropertyMetadataOptions.Inherits, NotifyPropertyChangeForRefreshContent));
			MaximumProperty = NumericUpDown.MaximumProperty.AddOwner(typeof(DataGridNumericUpDownColumn), new FrameworkPropertyMetadata((double)NumericUpDown.MaximumProperty.DefaultMetadata.DefaultValue, FrameworkPropertyMetadataOptions.Inherits, NotifyPropertyChangeForRefreshContent));
			IntervalProperty = NumericUpDown.IntervalProperty.AddOwner(typeof(DataGridNumericUpDownColumn), new FrameworkPropertyMetadata((double)NumericUpDown.IntervalProperty.DefaultMetadata.DefaultValue, FrameworkPropertyMetadataOptions.Inherits, NotifyPropertyChangeForRefreshContent));
			HideUpDownButtonsProperty = NumericUpDown.HideUpDownButtonsProperty.AddOwner(typeof(DataGridNumericUpDownColumn), new FrameworkPropertyMetadata((bool)NumericUpDown.HideUpDownButtonsProperty.DefaultMetadata.DefaultValue, FrameworkPropertyMetadataOptions.Inherits, NotifyPropertyChangeForRefreshContent));
			UpDownButtonsWidthProperty = NumericUpDown.UpDownButtonsWidthProperty.AddOwner(typeof(DataGridNumericUpDownColumn), new FrameworkPropertyMetadata((double)NumericUpDown.UpDownButtonsWidthProperty.DefaultMetadata.DefaultValue, FrameworkPropertyMetadataOptions.Inherits, NotifyPropertyChangeForRefreshContent));
			FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof(DataGridNumericUpDownColumn), new FrameworkPropertyMetadata(SystemFonts.MessageFontFamily, FrameworkPropertyMetadataOptions.Inherits, NotifyPropertyChangeForRefreshContent));
			FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(DataGridNumericUpDownColumn), new FrameworkPropertyMetadata(SystemFonts.MessageFontSize, FrameworkPropertyMetadataOptions.Inherits, NotifyPropertyChangeForRefreshContent));
			FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof(DataGridNumericUpDownColumn), new FrameworkPropertyMetadata(SystemFonts.MessageFontStyle, FrameworkPropertyMetadataOptions.Inherits, NotifyPropertyChangeForRefreshContent));
			FontWeightProperty = TextElement.FontWeightProperty.AddOwner(typeof(DataGridNumericUpDownColumn), new FrameworkPropertyMetadata(SystemFonts.MessageFontWeight, FrameworkPropertyMetadataOptions.Inherits, NotifyPropertyChangeForRefreshContent));
			ForegroundProperty = TextElement.ForegroundProperty.AddOwner(typeof(DataGridNumericUpDownColumn), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush, FrameworkPropertyMetadataOptions.Inherits, NotifyPropertyChangeForRefreshContent));
			DataGridBoundColumn.ElementStyleProperty.OverrideMetadata(typeof(DataGridNumericUpDownColumn), new FrameworkPropertyMetadata(DefaultElementStyle));
			DataGridBoundColumn.EditingElementStyleProperty.OverrideMetadata(typeof(DataGridNumericUpDownColumn), new FrameworkPropertyMetadata(DefaultEditingElementStyle));
		}

		private static void ApplyBinding(BindingBase binding, DependencyObject target, DependencyProperty property)
		{
			if (binding != null)
			{
				BindingOperations.SetBinding(target, property, binding);
			}
			else
			{
				BindingOperations.ClearBinding(target, property);
			}
		}

		private void ApplyStyle(bool isEditing, bool defaultToElementStyle, FrameworkElement element)
		{
			Style style = PickStyle(isEditing, defaultToElementStyle);
			if (style != null)
			{
				element.Style = style;
			}
		}

		protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
		{
			return GenerateNumericUpDown(isEditing: true, cell);
		}

		protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
		{
			NumericUpDown numericUpDown = GenerateNumericUpDown(isEditing: false, cell);
			numericUpDown.HideUpDownButtons = true;
			return numericUpDown;
		}

		private NumericUpDown GenerateNumericUpDown(bool isEditing, DataGridCell cell)
		{
			NumericUpDown numericUpDown = (cell != null) ? (cell.Content as NumericUpDown) : null;
			if (numericUpDown == null)
			{
				numericUpDown = new NumericUpDown();
			}
			SyncColumnProperty(this, numericUpDown, FontFamilyProperty, TextElement.FontFamilyProperty);
			SyncColumnProperty(this, numericUpDown, FontSizeProperty, TextElement.FontSizeProperty);
			SyncColumnProperty(this, numericUpDown, FontStyleProperty, TextElement.FontStyleProperty);
			SyncColumnProperty(this, numericUpDown, FontWeightProperty, TextElement.FontWeightProperty);
			SyncColumnProperty(this, numericUpDown, StringFormatProperty, NumericUpDown.StringFormatProperty);
			SyncColumnProperty(this, numericUpDown, MinimumProperty, NumericUpDown.MinimumProperty);
			SyncColumnProperty(this, numericUpDown, MaximumProperty, NumericUpDown.MaximumProperty);
			SyncColumnProperty(this, numericUpDown, IntervalProperty, NumericUpDown.IntervalProperty);
			SyncColumnProperty(this, numericUpDown, HideUpDownButtonsProperty, NumericUpDown.HideUpDownButtonsProperty);
			SyncColumnProperty(this, numericUpDown, UpDownButtonsWidthProperty, NumericUpDown.UpDownButtonsWidthProperty);
			if (isEditing)
			{
				SyncColumnProperty(this, numericUpDown, ForegroundProperty, TextElement.ForegroundProperty);
			}
			else if (!SyncColumnProperty(this, numericUpDown, ForegroundProperty, TextElement.ForegroundProperty))
			{
				ApplyBinding(new Binding(Control.ForegroundProperty.Name)
				{
					Source = cell,
					Mode = BindingMode.OneWay
				}, numericUpDown, TextElement.ForegroundProperty);
			}
			ApplyStyle(isEditing, defaultToElementStyle: true, numericUpDown);
			ApplyBinding(Binding, numericUpDown, NumericUpDown.ValueProperty);
			numericUpDown.InterceptArrowKeys = true;
			numericUpDown.InterceptMouseWheel = true;
			numericUpDown.Speedup = true;
			return numericUpDown;
		}

		protected override object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs)
		{
			NumericUpDown numericUpDown = editingElement as NumericUpDown;
			if (numericUpDown != null)
			{
				numericUpDown.Focus();
				numericUpDown.SelectAll();
				return numericUpDown.Value;
			}
			return null;
		}

		internal static bool SyncColumnProperty(DependencyObject column, DependencyObject content, DependencyProperty columnProperty, DependencyProperty contentProperty)
		{
			if (IsDefaultValue(column, columnProperty))
			{
				content.ClearValue(contentProperty);
				return false;
			}
			content.SetValue(contentProperty, column.GetValue(columnProperty));
			return true;
		}

		private static bool IsDefaultValue(DependencyObject d, DependencyProperty dp)
		{
			return DependencyPropertyHelper.GetValueSource(d, dp).BaseValueSource == BaseValueSource.Default;
		}

		private Style PickStyle(bool isEditing, bool defaultToElementStyle)
		{
			Style style = isEditing ? base.EditingElementStyle : base.ElementStyle;
			if (isEditing && defaultToElementStyle && style == null)
			{
				style = base.ElementStyle;
			}
			return style;
		}

		private static void NotifyPropertyChangeForRefreshContent(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGridNumericUpDownColumn)d).NotifyPropertyChanged(e.Property.Name);
		}

		protected override void RefreshCellContent(FrameworkElement element, string propertyName)
		{
			NumericUpDown numericUpDown = (element as DataGridCell)?.Content as NumericUpDown;
			if (numericUpDown != null)
			{
				switch (propertyName)
				{
				case "FontFamily":
					SyncColumnProperty(this, numericUpDown, FontFamilyProperty, TextElement.FontFamilyProperty);
					break;
				case "FontSize":
					SyncColumnProperty(this, numericUpDown, FontSizeProperty, TextElement.FontSizeProperty);
					break;
				case "FontStyle":
					SyncColumnProperty(this, numericUpDown, FontStyleProperty, TextElement.FontStyleProperty);
					break;
				case "FontWeight":
					SyncColumnProperty(this, numericUpDown, FontWeightProperty, TextElement.FontWeightProperty);
					break;
				case "StringFormat":
					SyncColumnProperty(this, numericUpDown, StringFormatProperty, NumericUpDown.StringFormatProperty);
					break;
				case "Minimum":
					SyncColumnProperty(this, numericUpDown, MinimumProperty, NumericUpDown.MinimumProperty);
					break;
				case "Maximum":
					SyncColumnProperty(this, numericUpDown, MaximumProperty, NumericUpDown.MaximumProperty);
					break;
				case "Interval":
					SyncColumnProperty(this, numericUpDown, IntervalProperty, NumericUpDown.IntervalProperty);
					break;
				case "HideUpDownButtons":
					SyncColumnProperty(this, numericUpDown, HideUpDownButtonsProperty, NumericUpDown.HideUpDownButtonsProperty);
					break;
				case "UpDownButtonsWidth":
					SyncColumnProperty(this, numericUpDown, UpDownButtonsWidthProperty, NumericUpDown.UpDownButtonsWidthProperty);
					break;
				}
			}
			base.RefreshCellContent(element, propertyName);
		}
	}
}
