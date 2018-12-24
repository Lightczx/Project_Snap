using System;
using System.Windows;
using System.Windows.Input;

namespace MahApps.Metro.Controls
{
	public class HamburgerMenuItem : Freezable, ICommandSource
	{
		public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(HamburgerMenuItem), new PropertyMetadata(null));

		public static readonly DependencyProperty TargetPageTypeProperty = DependencyProperty.Register("TargetPageType", typeof(Type), typeof(HamburgerMenuItem), new PropertyMetadata(null));

		public static readonly DependencyProperty TagProperty = DependencyProperty.Register("Tag", typeof(object), typeof(HamburgerMenuItem), new PropertyMetadata(null));

		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(HamburgerMenuItem), new PropertyMetadata(null, OnCommandChanged));

		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(HamburgerMenuItem), new PropertyMetadata(null));

		public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(HamburgerMenuItem), new PropertyMetadata(null));

		public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(bool), typeof(HamburgerMenuItem), new PropertyMetadata(true, null, IsEnabledCoerceValueCallback));

		public static readonly DependencyProperty ToolTipProperty = DependencyProperty.Register("ToolTip", typeof(object), typeof(HamburgerMenuItem), new PropertyMetadata(null));

		private bool canExecute;

		public string Label
		{
			get
			{
				return (string)GetValue(LabelProperty);
			}
			set
			{
				SetValue(LabelProperty, value);
			}
		}

		public Type TargetPageType
		{
			get
			{
				return (Type)GetValue(TargetPageTypeProperty);
			}
			set
			{
				SetValue(TargetPageTypeProperty, value);
			}
		}

		public object Tag
		{
			get
			{
				return GetValue(TagProperty);
			}
			set
			{
				SetValue(TagProperty, value);
			}
		}

		public ICommand Command
		{
			get
			{
				return (ICommand)GetValue(CommandProperty);
			}
			set
			{
				SetValue(CommandProperty, value);
			}
		}

		public object CommandParameter
		{
			get
			{
				return GetValue(CommandParameterProperty);
			}
			set
			{
				SetValue(CommandParameterProperty, value);
			}
		}

		public IInputElement CommandTarget
		{
			get
			{
				return (IInputElement)GetValue(CommandTargetProperty);
			}
			set
			{
				SetValue(CommandTargetProperty, value);
			}
		}

		public bool IsEnabled
		{
			get
			{
				return (bool)GetValue(IsEnabledProperty);
			}
			set
			{
				SetValue(IsEnabledProperty, value);
			}
		}

		public object ToolTip
		{
			get
			{
				return GetValue(ToolTipProperty);
			}
			set
			{
				SetValue(ToolTipProperty, value);
			}
		}

		private bool CanExecute
		{
			get
			{
				return canExecute;
			}
			set
			{
				if (value != canExecute)
				{
					canExecute = value;
					CoerceValue(IsEnabledProperty);
				}
			}
		}

		public void RaiseCommand()
		{
			CommandHelpers.ExecuteCommandSource(this);
		}

		private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((HamburgerMenuItem)d).OnCommandChanged((ICommand)e.OldValue, (ICommand)e.NewValue);
		}

		private void OnCommandChanged(ICommand oldCommand, ICommand newCommand)
		{
			if (oldCommand != null)
			{
				UnhookCommand(oldCommand);
			}
			if (newCommand != null)
			{
				HookCommand(newCommand);
			}
		}

		private void UnhookCommand(ICommand command)
		{
			CanExecuteChangedEventManager.RemoveHandler(command, OnCanExecuteChanged);
			UpdateCanExecute();
		}

		private void HookCommand(ICommand command)
		{
			CanExecuteChangedEventManager.AddHandler(command, OnCanExecuteChanged);
			UpdateCanExecute();
		}

		private void OnCanExecuteChanged(object sender, EventArgs e)
		{
			UpdateCanExecute();
		}

		private void UpdateCanExecute()
		{
			if (Command != null)
			{
				CanExecute = CommandHelpers.CanExecuteCommandSource(this);
			}
			else
			{
				CanExecute = true;
			}
		}

		private static object IsEnabledCoerceValueCallback(DependencyObject d, object value)
		{
			if (!(bool)value)
			{
				return false;
			}
			return ((HamburgerMenuItem)d).CanExecute;
		}

		protected override Freezable CreateInstanceCore()
		{
			return new HamburgerMenuItem();
		}
	}
}
