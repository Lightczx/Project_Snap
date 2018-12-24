using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Interactivity;

namespace MahApps.Metro.Actions
{
	[Obsolete("This TargetedTriggerAction will be deleted in the next release.")]
	public class SetFlyoutOpenAction : TargetedTriggerAction<FrameworkElement>
	{
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(bool), typeof(SetFlyoutOpenAction), new PropertyMetadata(false));

		public bool Value
		{
			get
			{
				return (bool)base.GetValue(ValueProperty);
			}
			set
			{
				base.SetValue(ValueProperty, (object)value);
			}
		}

		protected override void Invoke(object parameter)
		{
			((Flyout)this.get_TargetObject()).IsOpen = Value;
		}
	}
}
