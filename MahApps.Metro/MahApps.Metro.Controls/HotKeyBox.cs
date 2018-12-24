using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;

namespace MahApps.Metro.Controls
{
	[TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
	public class HotKeyBox : Control
	{
		private const string PART_TextBox = "PART_TextBox";

		public static readonly DependencyProperty HotKeyProperty;

		public static readonly DependencyProperty AreModifierKeysRequiredProperty;

		[Obsolete("This property will be deleted in the next release. Instead use TextBoxHelper.Watermark attached property.")]
		public static readonly DependencyProperty WatermarkProperty;

		private static readonly DependencyPropertyKey TextPropertyKey;

		public static readonly DependencyProperty TextProperty;

		private TextBox _textBox;

		public HotKey HotKey
		{
			get
			{
				return (HotKey)GetValue(HotKeyProperty);
			}
			set
			{
				SetValue(HotKeyProperty, value);
			}
		}

		public bool AreModifierKeysRequired
		{
			get
			{
				return (bool)GetValue(AreModifierKeysRequiredProperty);
			}
			set
			{
				SetValue(AreModifierKeysRequiredProperty, value);
			}
		}

		[Obsolete("This property will be deleted in the next release. Instead use TextBoxHelper.Watermark attached property.")]
		public string Watermark
		{
			get
			{
				return (string)GetValue(WatermarkProperty);
			}
			set
			{
				SetValue(WatermarkProperty, value);
			}
		}

		public string Text
		{
			get
			{
				return (string)GetValue(TextProperty);
			}
			private set
			{
				SetValue(TextPropertyKey, value);
			}
		}

		private static void OnHotKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((HotKeyBox)d).UpdateText();
		}

		static HotKeyBox()
		{
			HotKeyProperty = DependencyProperty.Register("HotKey", typeof(HotKey), typeof(HotKeyBox), new FrameworkPropertyMetadata(null, OnHotKeyChanged)
			{
				BindsTwoWayByDefault = true
			});
			AreModifierKeysRequiredProperty = DependencyProperty.Register("AreModifierKeysRequired", typeof(bool), typeof(HotKeyBox), new PropertyMetadata(false));
			WatermarkProperty = DependencyProperty.Register("Watermark", typeof(string), typeof(HotKeyBox), new PropertyMetadata((object)null));
			TextPropertyKey = DependencyProperty.RegisterReadOnly("Text", typeof(string), typeof(HotKeyBox), new PropertyMetadata((object)null));
			TextProperty = TextPropertyKey.DependencyProperty;
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HotKeyBox), new FrameworkPropertyMetadata(typeof(HotKeyBox)));
			EventManager.RegisterClassHandler(typeof(HotKeyBox), UIElement.GotFocusEvent, new RoutedEventHandler(OnGotFocus));
		}

		private static void OnGotFocus(object sender, RoutedEventArgs e)
		{
			if (!e.Handled)
			{
				HotKeyBox hotKeyBox = (HotKeyBox)sender;
				if (hotKeyBox.Focusable && e.OriginalSource == hotKeyBox)
				{
					TraversalRequest request = new TraversalRequest(((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) ? FocusNavigationDirection.Previous : FocusNavigationDirection.Next);
					UIElement uIElement = Keyboard.FocusedElement as UIElement;
					if (uIElement != null)
					{
						uIElement.MoveFocus(request);
					}
					else
					{
						hotKeyBox.Focus();
					}
					e.Handled = true;
				}
			}
		}

		public override void OnApplyTemplate()
		{
			if (_textBox != null)
			{
				_textBox.PreviewKeyDown -= TextBoxOnPreviewKeyDown2;
				_textBox.GotFocus -= TextBoxOnGotFocus;
				_textBox.LostFocus -= TextBoxOnLostFocus;
				_textBox.TextChanged -= TextBoxOnTextChanged;
			}
			base.OnApplyTemplate();
			_textBox = (base.Template.FindName("PART_TextBox", this) as TextBox);
			if (_textBox != null)
			{
				_textBox.PreviewKeyDown += TextBoxOnPreviewKeyDown2;
				_textBox.GotFocus += TextBoxOnGotFocus;
				_textBox.LostFocus += TextBoxOnLostFocus;
				_textBox.TextChanged += TextBoxOnTextChanged;
				UpdateText();
			}
		}

		private void TextBoxOnTextChanged(object sender, TextChangedEventArgs args)
		{
			_textBox.SelectionStart = _textBox.Text.Length;
		}

		private void TextBoxOnGotFocus(object sender, RoutedEventArgs routedEventArgs)
		{
			ComponentDispatcher.ThreadPreprocessMessage += this.ComponentDispatcherOnThreadPreprocessMessage;
		}

		private void ComponentDispatcherOnThreadPreprocessMessage(ref MSG msg, ref bool handled)
		{
			if (msg.message == 786)
			{
				handled = true;
			}
		}

		private void TextBoxOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
		{
			ComponentDispatcher.ThreadPreprocessMessage -= this.ComponentDispatcherOnThreadPreprocessMessage;
		}

		private void TextBoxOnPreviewKeyDown2(object sender, KeyEventArgs e)
		{
			Key key = (e.Key == Key.System) ? e.SystemKey : e.Key;
			if (key != Key.Tab && (uint)(key - 70) > 1u && (uint)(key - 116) > 5u)
			{
				e.Handled = true;
				ModifierKeys currentModifierKeys = GetCurrentModifierKeys();
				if (currentModifierKeys == ModifierKeys.None && key == Key.Back)
				{
					HotKey = null;
				}
				else if (currentModifierKeys != 0 || !AreModifierKeysRequired)
				{
					HotKey = new HotKey(key, currentModifierKeys);
				}
				UpdateText();
			}
		}

		private static ModifierKeys GetCurrentModifierKeys()
		{
			ModifierKeys modifierKeys = ModifierKeys.None;
			if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
			{
				modifierKeys |= ModifierKeys.Control;
			}
			if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
			{
				modifierKeys |= ModifierKeys.Alt;
			}
			if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
			{
				modifierKeys |= ModifierKeys.Shift;
			}
			if (Keyboard.IsKeyDown(Key.LWin) || Keyboard.IsKeyDown(Key.RWin))
			{
				modifierKeys |= ModifierKeys.Windows;
			}
			return modifierKeys;
		}

		private void UpdateText()
		{
			HotKey hotKey = HotKey;
			Text = ((hotKey == null || hotKey.Key == Key.None) ? string.Empty : hotKey.ToString());
		}
	}
}
