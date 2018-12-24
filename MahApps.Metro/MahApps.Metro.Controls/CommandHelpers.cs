using System.Security;
using System.Windows;
using System.Windows.Input;

namespace MahApps.Metro.Controls
{
	internal static class CommandHelpers
	{
		internal static bool CanExecuteCommandSource(ICommandSource commandSource)
		{
			ICommand command = commandSource.Command;
			if (command == null)
			{
				return false;
			}
			object parameter = commandSource.CommandParameter ?? commandSource;
			RoutedCommand routedCommand = command as RoutedCommand;
			if (routedCommand != null)
			{
				IInputElement target = commandSource.CommandTarget ?? (commandSource as IInputElement);
				return routedCommand.CanExecute(parameter, target);
			}
			return command.CanExecute(parameter);
		}

		[SecurityCritical]
		[SecuritySafeCritical]
		internal static void ExecuteCommandSource(ICommandSource commandSource)
		{
			CriticalExecuteCommandSource(commandSource);
		}

		[SecurityCritical]
		internal static void CriticalExecuteCommandSource(ICommandSource commandSource)
		{
			ICommand command = commandSource.Command;
			if (command != null)
			{
				object parameter = commandSource.CommandParameter ?? commandSource;
				RoutedCommand routedCommand = command as RoutedCommand;
				if (routedCommand != null)
				{
					IInputElement target = commandSource.CommandTarget ?? (commandSource as IInputElement);
					if (routedCommand.CanExecute(parameter, target))
					{
						routedCommand.Execute(parameter, target);
					}
				}
				else if (command.CanExecute(parameter))
				{
					command.Execute(parameter);
				}
			}
		}
	}
}
