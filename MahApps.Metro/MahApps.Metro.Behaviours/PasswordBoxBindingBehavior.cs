using MahApps.Metro.Controls;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interactivity;

namespace MahApps.Metro.Behaviours
{
	public class PasswordBoxBindingBehavior : Behavior<PasswordBox>
	{
		public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordBoxBindingBehavior), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPasswordPropertyChanged));

		private static readonly DependencyProperty IsChangingProperty = DependencyProperty.RegisterAttached("IsChanging", typeof(bool), typeof(PasswordBoxBindingBehavior), new UIPropertyMetadata(false));

		private static readonly DependencyProperty SelectionProperty = DependencyProperty.RegisterAttached("Selection", typeof(TextSelection), typeof(PasswordBoxBindingBehavior), new UIPropertyMetadata((object)null));

		private static readonly DependencyProperty RevealedPasswordTextBoxProperty = DependencyProperty.RegisterAttached("RevealedPasswordTextBox", typeof(TextBox), typeof(PasswordBoxBindingBehavior), new UIPropertyMetadata((object)null));

		[Category("MahApps.Metro")]
		public static string GetPassword(DependencyObject dpo)
		{
			return (string)dpo.GetValue(PasswordProperty);
		}

		public static void SetPassword(DependencyObject dpo, string value)
		{
			dpo.SetValue(PasswordProperty, value);
		}

		private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			PasswordBox passwordBox = sender as PasswordBox;
			if (passwordBox != null)
			{
				passwordBox.PasswordChanged -= PasswordBoxPasswordChanged;
				if (!GetIsChanging(passwordBox))
				{
					passwordBox.Password = (string)e.NewValue;
				}
				passwordBox.PasswordChanged += PasswordBoxPasswordChanged;
			}
		}

		private static void PasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
		{
			PasswordBox passwordBox = (PasswordBox)sender;
			SetIsChanging(passwordBox, value: true);
			SetPassword(passwordBox, passwordBox.Password);
			SetIsChanging(passwordBox, value: false);
		}

		private static void SetRevealedPasswordCaretIndex(PasswordBox passwordBox)
		{
			TextBox revealedPasswordTextBox = GetRevealedPasswordTextBox(passwordBox);
			if (revealedPasswordTextBox != null)
			{
				int num = revealedPasswordTextBox.CaretIndex = GetPasswordBoxCaretPosition(passwordBox);
			}
		}

		private static int GetPasswordBoxCaretPosition(PasswordBox passwordBox)
		{
			TextSelection selection = GetSelection(passwordBox);
			object obj = (selection?.GetType().GetInterfaces().FirstOrDefault((Type i) => i.Name == "ITextRange"))?.GetProperty("Start")?.GetGetMethod()?.Invoke(selection, null);
			return (obj?.GetType().GetProperty("Offset", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj, null) as int?).GetValueOrDefault(0);
		}

		protected override void OnAttached()
		{
			this.OnAttached();
			base.get_AssociatedObject().PasswordChanged += PasswordBoxPasswordChanged;
			base.get_AssociatedObject().Loaded += PasswordBoxLoaded;
			TextSelection selection = GetSelection(base.get_AssociatedObject());
			if (selection != null)
			{
				selection.Changed += PasswordBoxSelectionChanged;
			}
		}

		private void PasswordBoxLoaded(object sender, RoutedEventArgs e)
		{
			SetPassword(base.get_AssociatedObject(), base.get_AssociatedObject().Password);
			TextBox textBox = base.get_AssociatedObject().FindChild<TextBox>("RevealedPassword");
			if (textBox != null)
			{
				TextSelection selection = GetSelection(base.get_AssociatedObject());
				if (selection == null)
				{
					selection = (base.get_AssociatedObject().GetType().GetProperty("Selection", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(base.get_AssociatedObject(), null) as TextSelection);
					SetSelection(base.get_AssociatedObject(), selection);
					if (selection != null)
					{
						SetRevealedPasswordTextBox(base.get_AssociatedObject(), textBox);
						SetRevealedPasswordCaretIndex(base.get_AssociatedObject());
						selection.Changed += PasswordBoxSelectionChanged;
					}
				}
			}
		}

		private void PasswordBoxSelectionChanged(object sender, EventArgs e)
		{
			SetRevealedPasswordCaretIndex(base.get_AssociatedObject());
		}

		protected override void OnDetaching()
		{
			if (base.get_AssociatedObject() != null)
			{
				TextSelection selection = GetSelection(base.get_AssociatedObject());
				if (selection != null)
				{
					selection.Changed -= PasswordBoxSelectionChanged;
				}
				base.get_AssociatedObject().Loaded -= PasswordBoxLoaded;
				base.get_AssociatedObject().PasswordChanged -= PasswordBoxPasswordChanged;
			}
			this.OnDetaching();
		}

		private static bool GetIsChanging(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsChangingProperty);
		}

		private static void SetIsChanging(DependencyObject obj, bool value)
		{
			obj.SetValue(IsChangingProperty, value);
		}

		private static TextSelection GetSelection(DependencyObject obj)
		{
			return (TextSelection)obj.GetValue(SelectionProperty);
		}

		private static void SetSelection(DependencyObject obj, TextSelection value)
		{
			obj.SetValue(SelectionProperty, value);
		}

		private static TextBox GetRevealedPasswordTextBox(DependencyObject obj)
		{
			return (TextBox)obj.GetValue(RevealedPasswordTextBoxProperty);
		}

		private static void SetRevealedPasswordTextBox(DependencyObject obj, TextBox value)
		{
			obj.SetValue(RevealedPasswordTextBoxProperty, value);
		}
	}
}
