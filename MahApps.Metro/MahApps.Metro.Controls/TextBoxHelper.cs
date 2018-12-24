using MahApps.Metro.Behaviours;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace MahApps.Metro.Controls
{
	public class TextBoxHelper
	{
		public static readonly DependencyProperty IsMonitoringProperty = DependencyProperty.RegisterAttached("IsMonitoring", typeof(bool), typeof(TextBoxHelper), new UIPropertyMetadata(false, OnIsMonitoringChanged));

		public static readonly DependencyProperty WatermarkProperty = DependencyProperty.RegisterAttached("Watermark", typeof(string), typeof(TextBoxHelper), new UIPropertyMetadata(string.Empty));

		public static readonly DependencyProperty WatermarkAlignmentProperty = DependencyProperty.RegisterAttached("WatermarkAlignment", typeof(TextAlignment), typeof(TextBoxHelper), new FrameworkPropertyMetadata(TextAlignment.Left, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty WatermarkTrimmingProperty = DependencyProperty.RegisterAttached("WatermarkTrimming", typeof(TextTrimming), typeof(TextBoxHelper), new FrameworkPropertyMetadata(TextTrimming.None, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

		public static readonly DependencyProperty WatermarkWrappingProperty = DependencyProperty.RegisterAttached("WatermarkWrapping", typeof(TextWrapping), typeof(TextBoxHelper), new FrameworkPropertyMetadata(TextWrapping.NoWrap, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

		public static readonly DependencyProperty UseFloatingWatermarkProperty = DependencyProperty.RegisterAttached("UseFloatingWatermark", typeof(bool), typeof(TextBoxHelper), new FrameworkPropertyMetadata(false, ButtonCommandOrClearTextChanged));

		public static readonly DependencyProperty TextLengthProperty = DependencyProperty.RegisterAttached("TextLength", typeof(int), typeof(TextBoxHelper), new UIPropertyMetadata(0));

		public static readonly DependencyProperty ClearTextButtonProperty = DependencyProperty.RegisterAttached("ClearTextButton", typeof(bool), typeof(TextBoxHelper), new FrameworkPropertyMetadata(false, ButtonCommandOrClearTextChanged));

		public static readonly DependencyProperty TextButtonProperty = DependencyProperty.RegisterAttached("TextButton", typeof(bool), typeof(TextBoxHelper), new FrameworkPropertyMetadata(false, ButtonCommandOrClearTextChanged));

		public static readonly DependencyProperty ButtonsAlignmentProperty = DependencyProperty.RegisterAttached("ButtonsAlignment", typeof(ButtonsAlignment), typeof(TextBoxHelper), new FrameworkPropertyMetadata(ButtonsAlignment.Right, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

		public static readonly DependencyProperty IsClearTextButtonBehaviorEnabledProperty = DependencyProperty.RegisterAttached("IsClearTextButtonBehaviorEnabled", typeof(bool), typeof(TextBoxHelper), new FrameworkPropertyMetadata(false, IsClearTextButtonBehaviorEnabledChanged));

		public static readonly DependencyProperty ButtonWidthProperty = DependencyProperty.RegisterAttached("ButtonWidth", typeof(double), typeof(TextBoxHelper), new FrameworkPropertyMetadata(22.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty ButtonCommandProperty = DependencyProperty.RegisterAttached("ButtonCommand", typeof(ICommand), typeof(TextBoxHelper), new FrameworkPropertyMetadata(null, ButtonCommandOrClearTextChanged));

		public static readonly DependencyProperty ButtonCommandParameterProperty = DependencyProperty.RegisterAttached("ButtonCommandParameter", typeof(object), typeof(TextBoxHelper), new FrameworkPropertyMetadata(null));

		public static readonly DependencyProperty ButtonContentProperty = DependencyProperty.RegisterAttached("ButtonContent", typeof(object), typeof(TextBoxHelper), new FrameworkPropertyMetadata("r"));

		public static readonly DependencyProperty ButtonContentTemplateProperty = DependencyProperty.RegisterAttached("ButtonContentTemplate", typeof(DataTemplate), typeof(TextBoxHelper), new FrameworkPropertyMetadata((object)null));

		public static readonly DependencyProperty ButtonTemplateProperty = DependencyProperty.RegisterAttached("ButtonTemplate", typeof(ControlTemplate), typeof(TextBoxHelper), new FrameworkPropertyMetadata(null));

		public static readonly DependencyProperty ButtonFontFamilyProperty = DependencyProperty.RegisterAttached("ButtonFontFamily", typeof(FontFamily), typeof(TextBoxHelper), new FrameworkPropertyMetadata(new FontFamilyConverter().ConvertFromString("Marlett")));

		public static readonly DependencyProperty ButtonFontSizeProperty = DependencyProperty.RegisterAttached("ButtonFontSize", typeof(double), typeof(TextBoxHelper), new FrameworkPropertyMetadata(SystemFonts.MessageFontSize));

		public static readonly DependencyProperty SelectAllOnFocusProperty = DependencyProperty.RegisterAttached("SelectAllOnFocus", typeof(bool), typeof(TextBoxHelper), new FrameworkPropertyMetadata(false));

		public static readonly DependencyProperty IsWaitingForDataProperty = DependencyProperty.RegisterAttached("IsWaitingForData", typeof(bool), typeof(TextBoxHelper), new UIPropertyMetadata(false));

		public static readonly DependencyProperty HasTextProperty = DependencyProperty.RegisterAttached("HasText", typeof(bool), typeof(TextBoxHelper), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender));

		public static readonly DependencyProperty IsSpellCheckContextMenuEnabledProperty = DependencyProperty.RegisterAttached("IsSpellCheckContextMenuEnabled", typeof(bool), typeof(TextBoxHelper), new FrameworkPropertyMetadata(false, IsSpellCheckContextMenuEnabledChanged));

		public static readonly DependencyProperty AutoWatermarkProperty = DependencyProperty.RegisterAttached("AutoWatermark", typeof(bool), typeof(TextBoxHelper), new PropertyMetadata(false, OnAutoWatermarkChanged));

		private static readonly Dictionary<Type, DependencyProperty> AutoWatermarkPropertyMapping = new Dictionary<Type, DependencyProperty>
		{
			{
				typeof(TextBox),
				TextBox.TextProperty
			},
			{
				typeof(ComboBox),
				Selector.SelectedItemProperty
			},
			{
				typeof(NumericUpDown),
				NumericUpDown.ValueProperty
			},
			{
				typeof(HotKeyBox),
				HotKeyBox.HotKeyProperty
			},
			{
				typeof(DatePicker),
				DatePicker.SelectedDateProperty
			},
			{
				typeof(TimePicker),
				TimePickerBase.SelectedTimeProperty
			},
			{
				typeof(DateTimePicker),
				DateTimePicker.SelectedDateProperty
			}
		};

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
		public static bool GetIsSpellCheckContextMenuEnabled(UIElement element)
		{
			return (bool)element.GetValue(IsSpellCheckContextMenuEnabledProperty);
		}

		[AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
		public static void SetIsSpellCheckContextMenuEnabled(UIElement element, bool value)
		{
			element.SetValue(IsSpellCheckContextMenuEnabledProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TextBox))]
		[AttachedPropertyBrowsableForType(typeof(ComboBox))]
		[AttachedPropertyBrowsableForType(typeof(DatePicker))]
		[AttachedPropertyBrowsableForType(typeof(TimePickerBase))]
		[AttachedPropertyBrowsableForType(typeof(NumericUpDown))]
		[AttachedPropertyBrowsableForType(typeof(HotKeyBox))]
		public static bool GetAutoWatermark(DependencyObject element)
		{
			return (bool)element.GetValue(AutoWatermarkProperty);
		}

		public static void SetAutoWatermark(DependencyObject element, bool value)
		{
			element.SetValue(AutoWatermarkProperty, value);
		}

		private static void OnAutoWatermarkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = d as FrameworkElement;
			bool? flag = e.NewValue as bool?;
			if (frameworkElement != null)
			{
				if (flag.GetValueOrDefault())
				{
					if (frameworkElement.IsLoaded)
					{
						OnControlWithAutoWatermarkSupportLoaded(frameworkElement, new RoutedEventArgs());
					}
					else
					{
						frameworkElement.Loaded += OnControlWithAutoWatermarkSupportLoaded;
					}
				}
				else
				{
					frameworkElement.Loaded -= OnControlWithAutoWatermarkSupportLoaded;
				}
			}
		}

		private static void OnControlWithAutoWatermarkSupportLoaded(object o, RoutedEventArgs routedEventArgs)
		{
			FrameworkElement frameworkElement = (FrameworkElement)o;
			frameworkElement.Loaded -= OnControlWithAutoWatermarkSupportLoaded;
			if (!AutoWatermarkPropertyMapping.TryGetValue(frameworkElement.GetType(), out DependencyProperty value))
			{
				throw new NotSupportedException(string.Format("{0} is not supported for {1}", "AutoWatermarkProperty", frameworkElement.GetType()));
			}
			PropertyInfo propertyInfo = ResolvePropertyFromBindingExpression(frameworkElement.GetBindingExpression(value));
			if (propertyInfo != null)
			{
				DisplayAttribute customAttribute = propertyInfo.GetCustomAttribute<DisplayAttribute>();
				if (customAttribute != null)
				{
					frameworkElement.SetCurrentValue(WatermarkProperty, customAttribute.GetPrompt());
				}
			}
		}

		private static PropertyInfo ResolvePropertyFromBindingExpression(BindingExpression bindingExpression)
		{
			if (bindingExpression != null)
			{
				if (bindingExpression.Status == BindingStatus.PathError)
				{
					return null;
				}
				string resolvedSourcePropertyName = bindingExpression.ResolvedSourcePropertyName;
				if (!string.IsNullOrEmpty(resolvedSourcePropertyName))
				{
					Type type = bindingExpression.ResolvedSource?.GetType();
					if (type != null)
					{
						return type.GetProperty(resolvedSourcePropertyName, BindingFlags.Instance | BindingFlags.Public);
					}
				}
			}
			return null;
		}

		private static void IsSpellCheckContextMenuEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBoxBase textBoxBase = d as TextBoxBase;
			if (textBoxBase == null)
			{
				throw new InvalidOperationException("The property 'IsSpellCheckContextMenuEnabled' may only be set on TextBoxBase elements.");
			}
			if (e.OldValue != e.NewValue)
			{
				textBoxBase.SetCurrentValue(SpellCheck.IsEnabledProperty, (bool)e.NewValue);
				if ((bool)e.NewValue)
				{
					textBoxBase.ContextMenuOpening += TextBoxBaseContextMenuOpening;
					textBoxBase.LostFocus += TextBoxBaseLostFocus;
					textBoxBase.ContextMenuClosing += TextBoxBaseContextMenuClosing;
				}
				else
				{
					textBoxBase.ContextMenuOpening -= TextBoxBaseContextMenuOpening;
					textBoxBase.LostFocus -= TextBoxBaseLostFocus;
					textBoxBase.ContextMenuClosing -= TextBoxBaseContextMenuClosing;
				}
			}
		}

		private static void TextBoxBaseLostFocus(object sender, RoutedEventArgs e)
		{
			RemoveSpellCheckMenuItems((sender as FrameworkElement)?.ContextMenu);
		}

		private static void TextBoxBaseContextMenuClosing(object sender, ContextMenuEventArgs e)
		{
			RemoveSpellCheckMenuItems((sender as FrameworkElement)?.ContextMenu);
		}

		private static void TextBoxBaseContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			TextBoxBase textBoxBase = (TextBoxBase)sender;
			ContextMenu contextMenu = textBoxBase.ContextMenu;
			if (contextMenu != null)
			{
				RemoveSpellCheckMenuItems(contextMenu);
				if (SpellCheck.GetIsEnabled(textBoxBase))
				{
					int num = 0;
					TextBox textBox = textBoxBase as TextBox;
					RichTextBox richTextBox = textBoxBase as RichTextBox;
					SpellingError spellingError = (textBox != null) ? textBox.GetSpellingError(textBox.CaretIndex) : richTextBox?.GetSpellingError(richTextBox.CaretPosition);
					if (spellingError != null)
					{
						Style style = contextMenu.TryFindResource(Spelling.SuggestionMenuItemStyleKey) as Style;
						List<string> list = spellingError.Suggestions.ToList();
						if (list.Any())
						{
							foreach (string item in list)
							{
								MenuItem insertItem = new MenuItem
								{
									Command = EditingCommands.CorrectSpellingError,
									CommandParameter = item,
									CommandTarget = textBoxBase,
									Style = style,
									Tag = typeof(Spelling)
								};
								contextMenu.Items.Insert(num++, insertItem);
							}
						}
						else
						{
							contextMenu.Items.Insert(num++, new MenuItem
							{
								Style = (contextMenu.TryFindResource(Spelling.NoSuggestionsMenuItemStyleKey) as Style),
								Tag = typeof(Spelling)
							});
						}
						contextMenu.Items.Insert(num++, new Separator
						{
							Style = (contextMenu.TryFindResource(Spelling.SeparatorStyleKey) as Style),
							Tag = typeof(Spelling)
						});
						MenuItem insertItem2 = new MenuItem
						{
							Command = EditingCommands.IgnoreSpellingError,
							CommandTarget = textBoxBase,
							Style = (contextMenu.TryFindResource(Spelling.IgnoreAllMenuItemStyleKey) as Style),
							Tag = typeof(Spelling)
						};
						contextMenu.Items.Insert(num++, insertItem2);
						contextMenu.Items.Insert(num, new Separator
						{
							Style = (contextMenu.TryFindResource(Spelling.SeparatorStyleKey) as Style),
							Tag = typeof(Spelling)
						});
					}
				}
			}
		}

		private static void RemoveSpellCheckMenuItems(ContextMenu contextMenu)
		{
			if (contextMenu != null)
			{
				foreach (FrameworkElement item in (from item in contextMenu.Items.OfType<FrameworkElement>()
				where item.Tag == typeof(Spelling)
				select item).ToList())
				{
					contextMenu.Items.Remove(item);
				}
			}
		}

		public static void SetIsWaitingForData(DependencyObject obj, bool value)
		{
			obj.SetValue(IsWaitingForDataProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
		[AttachedPropertyBrowsableForType(typeof(PasswordBox))]
		public static bool GetIsWaitingForData(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsWaitingForDataProperty);
		}

		public static void SetSelectAllOnFocus(DependencyObject obj, bool value)
		{
			obj.SetValue(SelectAllOnFocusProperty, value);
		}

		public static bool GetSelectAllOnFocus(DependencyObject obj)
		{
			return (bool)obj.GetValue(SelectAllOnFocusProperty);
		}

		public static void SetIsMonitoring(DependencyObject obj, bool value)
		{
			obj.SetValue(IsMonitoringProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
		[AttachedPropertyBrowsableForType(typeof(PasswordBox))]
		[AttachedPropertyBrowsableForType(typeof(ComboBox))]
		[AttachedPropertyBrowsableForType(typeof(DatePicker))]
		[AttachedPropertyBrowsableForType(typeof(TimePickerBase))]
		[AttachedPropertyBrowsableForType(typeof(NumericUpDown))]
		[AttachedPropertyBrowsableForType(typeof(HotKeyBox))]
		public static string GetWatermark(DependencyObject obj)
		{
			return (string)obj.GetValue(WatermarkProperty);
		}

		public static void SetWatermark(DependencyObject obj, string value)
		{
			obj.SetValue(WatermarkProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
		[AttachedPropertyBrowsableForType(typeof(PasswordBox))]
		[AttachedPropertyBrowsableForType(typeof(ComboBox))]
		[AttachedPropertyBrowsableForType(typeof(DatePicker))]
		[AttachedPropertyBrowsableForType(typeof(TimePickerBase))]
		[AttachedPropertyBrowsableForType(typeof(NumericUpDown))]
		[AttachedPropertyBrowsableForType(typeof(HotKeyBox))]
		public static TextAlignment GetWatermarkAlignment(DependencyObject obj)
		{
			return (TextAlignment)obj.GetValue(WatermarkAlignmentProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
		[AttachedPropertyBrowsableForType(typeof(PasswordBox))]
		[AttachedPropertyBrowsableForType(typeof(ComboBox))]
		[AttachedPropertyBrowsableForType(typeof(DatePicker))]
		[AttachedPropertyBrowsableForType(typeof(TimePickerBase))]
		[AttachedPropertyBrowsableForType(typeof(NumericUpDown))]
		[AttachedPropertyBrowsableForType(typeof(HotKeyBox))]
		public static void SetWatermarkAlignment(DependencyObject obj, TextAlignment value)
		{
			obj.SetValue(WatermarkAlignmentProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
		[AttachedPropertyBrowsableForType(typeof(PasswordBox))]
		[AttachedPropertyBrowsableForType(typeof(ComboBox))]
		[AttachedPropertyBrowsableForType(typeof(DatePicker))]
		[AttachedPropertyBrowsableForType(typeof(TimePickerBase))]
		[AttachedPropertyBrowsableForType(typeof(NumericUpDown))]
		[AttachedPropertyBrowsableForType(typeof(HotKeyBox))]
		public static TextTrimming GetWatermarkTrimming(DependencyObject obj)
		{
			return (TextTrimming)obj.GetValue(WatermarkTrimmingProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
		[AttachedPropertyBrowsableForType(typeof(PasswordBox))]
		[AttachedPropertyBrowsableForType(typeof(ComboBox))]
		[AttachedPropertyBrowsableForType(typeof(DatePicker))]
		[AttachedPropertyBrowsableForType(typeof(TimePickerBase))]
		[AttachedPropertyBrowsableForType(typeof(NumericUpDown))]
		[AttachedPropertyBrowsableForType(typeof(HotKeyBox))]
		public static void SetWatermarkTrimming(DependencyObject obj, TextTrimming value)
		{
			obj.SetValue(WatermarkTrimmingProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
		public static TextWrapping GetWatermarkWrapping(DependencyObject obj)
		{
			return (TextWrapping)obj.GetValue(WatermarkWrappingProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
		public static void SetWatermarkWrapping(DependencyObject obj, TextWrapping value)
		{
			obj.SetValue(WatermarkWrappingProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
		[AttachedPropertyBrowsableForType(typeof(PasswordBox))]
		[AttachedPropertyBrowsableForType(typeof(ComboBox))]
		[AttachedPropertyBrowsableForType(typeof(NumericUpDown))]
		[AttachedPropertyBrowsableForType(typeof(HotKeyBox))]
		public static bool GetUseFloatingWatermark(DependencyObject obj)
		{
			return (bool)obj.GetValue(UseFloatingWatermarkProperty);
		}

		public static void SetUseFloatingWatermark(DependencyObject obj, bool value)
		{
			obj.SetValue(UseFloatingWatermarkProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
		[AttachedPropertyBrowsableForType(typeof(PasswordBox))]
		[AttachedPropertyBrowsableForType(typeof(ComboBox))]
		[AttachedPropertyBrowsableForType(typeof(DatePicker))]
		[AttachedPropertyBrowsableForType(typeof(NumericUpDown))]
		public static bool GetHasText(DependencyObject obj)
		{
			return (bool)obj.GetValue(HasTextProperty);
		}

		public static void SetHasText(DependencyObject obj, bool value)
		{
			obj.SetValue(HasTextProperty, value);
		}

		private static void OnIsMonitoringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is TextBox)
			{
				TextBox txtBox = (TextBox)d;
				if ((bool)e.NewValue)
				{
					txtBox.BeginInvoke(delegate
					{
						TextChanged(txtBox, new TextChangedEventArgs(TextBoxBase.TextChangedEvent, UndoAction.None));
					});
					txtBox.TextChanged += TextChanged;
					txtBox.GotFocus += TextBoxGotFocus;
				}
				else
				{
					txtBox.TextChanged -= TextChanged;
					txtBox.GotFocus -= TextBoxGotFocus;
				}
			}
			else if (d is PasswordBox)
			{
				PasswordBox passBox = (PasswordBox)d;
				if ((bool)e.NewValue)
				{
					passBox.BeginInvoke(delegate
					{
						PasswordChanged(passBox, new RoutedEventArgs(PasswordBox.PasswordChangedEvent, passBox));
					});
					passBox.PasswordChanged += PasswordChanged;
					passBox.GotFocus += PasswordGotFocus;
				}
				else
				{
					passBox.PasswordChanged -= PasswordChanged;
					passBox.GotFocus -= PasswordGotFocus;
				}
			}
			else if (d is NumericUpDown)
			{
				NumericUpDown numericUpDown = (NumericUpDown)d;
				if ((bool)e.NewValue)
				{
					numericUpDown.BeginInvoke(delegate
					{
						OnNumericUpDownValueChaged(numericUpDown, new RoutedEventArgs(NumericUpDown.ValueChangedEvent, numericUpDown));
					});
					numericUpDown.ValueChanged += OnNumericUpDownValueChaged;
				}
				else
				{
					numericUpDown.ValueChanged -= OnNumericUpDownValueChaged;
				}
			}
			else if (d is TimePickerBase)
			{
				TimePickerBase timePickerBase = (TimePickerBase)d;
				if ((bool)e.NewValue)
				{
					timePickerBase.SelectedTimeChanged += OnTimePickerBaseSelectedTimeChanged;
				}
				else
				{
					timePickerBase.SelectedTimeChanged -= OnTimePickerBaseSelectedTimeChanged;
				}
			}
			else if (d is DatePicker)
			{
				DatePicker datePicker = (DatePicker)d;
				if ((bool)e.NewValue)
				{
					datePicker.SelectedDateChanged += OnDatePickerBaseSelectedDateChanged;
				}
				else
				{
					datePicker.SelectedDateChanged -= OnDatePickerBaseSelectedDateChanged;
				}
			}
		}

		private static void SetTextLength<TDependencyObject>(TDependencyObject sender, Func<TDependencyObject, int> funcTextLength) where TDependencyObject : DependencyObject
		{
			if (sender != null)
			{
				int num = funcTextLength(sender);
				sender.SetValue(TextLengthProperty, num);
				sender.SetValue(HasTextProperty, num >= 1);
			}
		}

		private static void TextChanged(object sender, RoutedEventArgs e)
		{
			SetTextLength(sender as TextBox, (TextBox textBox) => textBox.Text.Length);
		}

		private static void OnNumericUpDownValueChaged(object sender, RoutedEventArgs e)
		{
			SetTextLength(sender as NumericUpDown, delegate(NumericUpDown numericUpDown)
			{
				if (!numericUpDown.Value.HasValue)
				{
					return 0;
				}
				return 1;
			});
		}

		private static void PasswordChanged(object sender, RoutedEventArgs e)
		{
			SetTextLength(sender as PasswordBox, (PasswordBox passwordBox) => passwordBox.Password.Length);
		}

		private static void OnDatePickerBaseSelectedDateChanged(object sender, RoutedEventArgs e)
		{
			SetTextLength(sender as DatePicker, delegate(DatePicker timePickerBase)
			{
				if (!timePickerBase.SelectedDate.HasValue)
				{
					return 0;
				}
				return 1;
			});
		}

		private static void OnTimePickerBaseSelectedTimeChanged(object sender, RoutedEventArgs e)
		{
			SetTextLength(sender as TimePickerBase, delegate(TimePickerBase timePickerBase)
			{
				if (!timePickerBase.SelectedTime.HasValue)
				{
					return 0;
				}
				return 1;
			});
		}

		private static void TextBoxGotFocus(object sender, RoutedEventArgs e)
		{
			ControlGotFocus(sender as TextBox, delegate(TextBox textBox)
			{
				textBox.SelectAll();
			});
		}

		private static void NumericUpDownGotFocus(object sender, RoutedEventArgs e)
		{
			ControlGotFocus(sender as NumericUpDown, delegate(NumericUpDown numericUpDown)
			{
				numericUpDown.SelectAll();
			});
		}

		private static void PasswordGotFocus(object sender, RoutedEventArgs e)
		{
			ControlGotFocus(sender as PasswordBox, delegate(PasswordBox passwordBox)
			{
				passwordBox.SelectAll();
			});
		}

		private static void ControlGotFocus<TDependencyObject>(TDependencyObject sender, Action<TDependencyObject> action) where TDependencyObject : DependencyObject
		{
			if (sender != null && GetSelectAllOnFocus(sender))
			{
				sender.Dispatcher.BeginInvoke(action, sender);
			}
		}

		[Category("MahApps.Metro")]
		public static bool GetClearTextButton(DependencyObject d)
		{
			return (bool)d.GetValue(ClearTextButtonProperty);
		}

		public static void SetClearTextButton(DependencyObject obj, bool value)
		{
			obj.SetValue(ClearTextButtonProperty, value);
		}

		[Category("MahApps.Metro")]
		public static bool GetTextButton(DependencyObject d)
		{
			return (bool)d.GetValue(TextButtonProperty);
		}

		public static void SetTextButton(DependencyObject obj, bool value)
		{
			obj.SetValue(TextButtonProperty, value);
		}

		[Category("MahApps.Metro")]
		public static ButtonsAlignment GetButtonsAlignment(DependencyObject d)
		{
			return (ButtonsAlignment)d.GetValue(ButtonsAlignmentProperty);
		}

		public static void SetButtonsAlignment(DependencyObject obj, ButtonsAlignment value)
		{
			obj.SetValue(ButtonsAlignmentProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ButtonBase))]
		public static bool GetIsClearTextButtonBehaviorEnabled(Button d)
		{
			return (bool)d.GetValue(IsClearTextButtonBehaviorEnabledProperty);
		}

		[AttachedPropertyBrowsableForType(typeof(ButtonBase))]
		public static void SetIsClearTextButtonBehaviorEnabled(Button obj, bool value)
		{
			obj.SetValue(IsClearTextButtonBehaviorEnabledProperty, value);
		}

		[Category("MahApps.Metro")]
		public static double GetButtonWidth(DependencyObject obj)
		{
			return (double)obj.GetValue(ButtonWidthProperty);
		}

		public static void SetButtonWidth(DependencyObject obj, double value)
		{
			obj.SetValue(ButtonWidthProperty, value);
		}

		[Category("MahApps.Metro")]
		public static ICommand GetButtonCommand(DependencyObject d)
		{
			return (ICommand)d.GetValue(ButtonCommandProperty);
		}

		public static void SetButtonCommand(DependencyObject obj, ICommand value)
		{
			obj.SetValue(ButtonCommandProperty, value);
		}

		[Category("MahApps.Metro")]
		public static object GetButtonCommandParameter(DependencyObject d)
		{
			return d.GetValue(ButtonCommandParameterProperty);
		}

		public static void SetButtonCommandParameter(DependencyObject obj, object value)
		{
			obj.SetValue(ButtonCommandParameterProperty, value);
		}

		[Category("MahApps.Metro")]
		public static object GetButtonContent(DependencyObject d)
		{
			return d.GetValue(ButtonContentProperty);
		}

		public static void SetButtonContent(DependencyObject obj, object value)
		{
			obj.SetValue(ButtonContentProperty, value);
		}

		[Category("MahApps.Metro")]
		public static DataTemplate GetButtonContentTemplate(DependencyObject d)
		{
			return (DataTemplate)d.GetValue(ButtonContentTemplateProperty);
		}

		public static void SetButtonContentTemplate(DependencyObject obj, DataTemplate value)
		{
			obj.SetValue(ButtonContentTemplateProperty, value);
		}

		[Category("MahApps.Metro")]
		public static ControlTemplate GetButtonTemplate(DependencyObject d)
		{
			return (ControlTemplate)d.GetValue(ButtonTemplateProperty);
		}

		public static void SetButtonTemplate(DependencyObject obj, ControlTemplate value)
		{
			obj.SetValue(ButtonTemplateProperty, value);
		}

		[Category("MahApps.Metro")]
		public static FontFamily GetButtonFontFamily(DependencyObject d)
		{
			return (FontFamily)d.GetValue(ButtonFontFamilyProperty);
		}

		public static void SetButtonFontFamily(DependencyObject obj, FontFamily value)
		{
			obj.SetValue(ButtonFontFamilyProperty, value);
		}

		[Category("MahApps.Metro")]
		public static double GetButtonFontSize(DependencyObject d)
		{
			return (double)d.GetValue(ButtonFontSizeProperty);
		}

		public static void SetButtonFontSize(DependencyObject obj, double value)
		{
			obj.SetValue(ButtonFontSizeProperty, value);
		}

		private static void IsClearTextButtonBehaviorEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Button button = d as Button;
			if (e.OldValue != e.NewValue && button != null)
			{
				button.Click -= ButtonClicked;
				if ((bool)e.NewValue)
				{
					button.Click += ButtonClicked;
				}
			}
		}

		public static void ButtonClicked(object sender, RoutedEventArgs e)
		{
			DependencyObject dependencyObject = ((Button)sender).GetAncestors().FirstOrDefault(delegate(DependencyObject a)
			{
				if (!(a is TextBox) && !(a is PasswordBox))
				{
					return a is ComboBox;
				}
				return true;
			});
			ICommand buttonCommand = GetButtonCommand(dependencyObject);
			object parameter = GetButtonCommandParameter(dependencyObject) ?? dependencyObject;
			if (buttonCommand != null && buttonCommand.CanExecute(parameter))
			{
				buttonCommand.Execute(parameter);
			}
			if (GetClearTextButton(dependencyObject))
			{
				if (dependencyObject is TextBox)
				{
					((TextBox)dependencyObject).Clear();
					((TextBox)dependencyObject).GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
				}
				else if (dependencyObject is PasswordBox)
				{
					((PasswordBox)dependencyObject).Clear();
					((PasswordBox)dependencyObject).GetBindingExpression(PasswordBoxBindingBehavior.PasswordProperty)?.UpdateSource();
				}
				else if (dependencyObject is ComboBox)
				{
					if (((ComboBox)dependencyObject).IsEditable)
					{
						((ComboBox)dependencyObject).Text = string.Empty;
						((ComboBox)dependencyObject).GetBindingExpression(ComboBox.TextProperty)?.UpdateSource();
					}
					((ComboBox)dependencyObject).SelectedItem = null;
					((ComboBox)dependencyObject).GetBindingExpression(Selector.SelectedItemProperty)?.UpdateSource();
				}
			}
		}

		private static void ButtonCommandOrClearTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBox textBox = d as TextBox;
			if (textBox != null)
			{
				textBox.Loaded -= TextChanged;
				textBox.Loaded += TextChanged;
				if (textBox.IsLoaded)
				{
					TextChanged(textBox, new RoutedEventArgs());
				}
			}
			PasswordBox passwordBox = d as PasswordBox;
			if (passwordBox != null)
			{
				passwordBox.Loaded -= PasswordChanged;
				passwordBox.Loaded += PasswordChanged;
				if (passwordBox.IsLoaded)
				{
					PasswordChanged(passwordBox, new RoutedEventArgs());
				}
			}
			ComboBox comboBox = d as ComboBox;
			if (comboBox != null)
			{
				comboBox.Loaded -= ComboBoxLoaded;
				comboBox.Loaded += ComboBoxLoaded;
				if (comboBox.IsLoaded)
				{
					ComboBoxLoaded(comboBox, new RoutedEventArgs());
				}
			}
		}

		private static void ComboBoxLoaded(object sender, RoutedEventArgs e)
		{
			ComboBox comboBox = sender as ComboBox;
			comboBox?.SetValue(HasTextProperty, !string.IsNullOrWhiteSpace(comboBox.Text) || comboBox.SelectedItem != null);
		}
	}
}
