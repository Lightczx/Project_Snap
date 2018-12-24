using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace MahApps.Metro.Actions
{
	public class CommandTriggerAction : TriggerAction<FrameworkElement>
	{
		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(CommandTriggerAction), new PropertyMetadata(null, delegate(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			OnCommandChanged(s as CommandTriggerAction, e);
		}));

		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(CommandTriggerAction), new PropertyMetadata(null, delegate(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			CommandTriggerAction commandTriggerAction = s as CommandTriggerAction;
			if (commandTriggerAction?.get_AssociatedObject() != null)
			{
				commandTriggerAction.EnableDisableElement();
			}
		}));

		public ICommand Command
		{
			get
			{
				return (ICommand)base.GetValue(CommandProperty);
			}
			set
			{
				base.SetValue(CommandProperty, (object)value);
			}
		}

		public object CommandParameter
		{
			get
			{
				return base.GetValue(CommandParameterProperty);
			}
			set
			{
				base.SetValue(CommandParameterProperty, value);
			}
		}

		protected override void OnAttached()
		{
			this.OnAttached();
			EnableDisableElement();
		}

		protected override void Invoke(object parameter)
		{
			if (base.get_AssociatedObject() != null && (base.get_AssociatedObject() == null || base.get_AssociatedObject().IsEnabled))
			{
				ICommand command = Command;
				if (command != null)
				{
					object commandParameter = GetCommandParameter();
					if (command.CanExecute(commandParameter))
					{
						command.Execute(commandParameter);
					}
				}
			}
		}

		private static void OnCommandChanged(CommandTriggerAction action, DependencyPropertyChangedEventArgs e)
		{
			if (action != null)
			{
				if (e.OldValue != null)
				{
					((ICommand)e.OldValue).CanExecuteChanged -= action.OnCommandCanExecuteChanged;
				}
				ICommand command = (ICommand)e.NewValue;
				if (command != null)
				{
					command.CanExecuteChanged += action.OnCommandCanExecuteChanged;
				}
				action.EnableDisableElement();
			}
		}

		protected virtual object GetCommandParameter()
		{
			return CommandParameter ?? base.get_AssociatedObject();
		}

		private void EnableDisableElement()
		{
			if (base.get_AssociatedObject() != null)
			{
				ICommand command = Command;
				base.get_AssociatedObject().SetCurrentValue(UIElement.IsEnabledProperty, command?.CanExecute(GetCommandParameter()) ?? true);
			}
		}

		private void OnCommandCanExecuteChanged(object sender, EventArgs e)
		{
			EnableDisableElement();
		}
	}
}
