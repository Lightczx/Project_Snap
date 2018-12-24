using MahApps.Metro.Controls;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using System.Windows.Threading;

namespace MahApps.Metro.Behaviours
{
	public class DatePickerTextBoxBehavior : Behavior<DatePickerTextBox>
	{
		protected override void OnAttached()
		{
			this.OnAttached();
			base.get_AssociatedObject().TextChanged += OnTextChanged;
			((DispatcherObject)this).BeginInvoke(SetHasTextProperty, DispatcherPriority.Loaded);
		}

		protected override void OnDetaching()
		{
			base.get_AssociatedObject().TextChanged -= OnTextChanged;
			this.OnDetaching();
		}

		private void OnTextChanged(object sender, TextChangedEventArgs e)
		{
			SetHasTextProperty();
		}

		private void SetHasTextProperty()
		{
			base.get_AssociatedObject().TemplatedParent?.SetValue(TextBoxHelper.HasTextProperty, base.get_AssociatedObject().Text.Length > 0);
		}
	}
}
