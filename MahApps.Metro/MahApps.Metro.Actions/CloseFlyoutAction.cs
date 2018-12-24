using MahApps.Metro.Controls;
using System.Windows.Input;

namespace MahApps.Metro.Actions
{
	public class CloseFlyoutAction : CommandTriggerAction
	{
		private Flyout associatedFlyout;

		private Flyout AssociatedFlyout => associatedFlyout ?? (associatedFlyout = base.get_AssociatedObject().TryFindParent<Flyout>());

		protected override void Invoke(object parameter)
		{
			if (base.get_AssociatedObject() != null && (base.get_AssociatedObject() == null || base.get_AssociatedObject().IsEnabled))
			{
				ICommand command = base.Command;
				if (command != null)
				{
					object commandParameter = GetCommandParameter();
					if (command.CanExecute(commandParameter))
					{
						command.Execute(commandParameter);
					}
				}
				else
				{
					AssociatedFlyout?.SetCurrentValue(Flyout.IsOpenProperty, false);
				}
			}
		}

		protected override object GetCommandParameter()
		{
			return base.CommandParameter ?? AssociatedFlyout;
		}
	}
}
