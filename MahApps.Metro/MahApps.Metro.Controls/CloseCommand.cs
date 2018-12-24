using System;
using System.Windows.Input;

namespace MahApps.Metro.Controls
{
	internal class CloseCommand : ICommand
	{
		private readonly Func<object, bool> canExecute;

		private readonly Action<object> executeAction;

		public event EventHandler CanExecuteChanged
		{
			add
			{
				CommandManager.RequerySuggested += value;
			}
			remove
			{
				CommandManager.RequerySuggested -= value;
			}
		}

		public CloseCommand(Func<object, bool> canExecute, Action<object> executeAction)
		{
			this.canExecute = canExecute;
			this.executeAction = executeAction;
		}

		public bool CanExecute(object parameter)
		{
			if (canExecute != null)
			{
				return canExecute(parameter);
			}
			return false;
		}

		public void Execute(object parameter)
		{
			executeAction?.Invoke(parameter);
		}
	}
}
