using System;
using System.Threading.Tasks;
using System.Windows;

namespace MahApps.Metro.Controls.Dialogs
{
	public class DialogCoordinator : IDialogCoordinator
	{
		public static readonly IDialogCoordinator Instance = new DialogCoordinator();

		public Task<string> ShowInputAsync(object context, string title, string message, MetroDialogSettings settings = null)
		{
			MetroWindow metroWindow = GetMetroWindow(context);
			return metroWindow.Invoke(() => metroWindow.ShowInputAsync(title, message, settings));
		}

		public string ShowModalInputExternal(object context, string title, string message, MetroDialogSettings metroDialogSettings = null)
		{
			return GetMetroWindow(context).ShowModalInputExternal(title, message, metroDialogSettings);
		}

		public Task<LoginDialogData> ShowLoginAsync(object context, string title, string message, LoginDialogSettings settings = null)
		{
			MetroWindow metroWindow = GetMetroWindow(context);
			return metroWindow.Invoke(() => metroWindow.ShowLoginAsync(title, message, settings));
		}

		public LoginDialogData ShowModalLoginExternal(object context, string title, string message, LoginDialogSettings settings = null)
		{
			return GetMetroWindow(context).ShowModalLoginExternal(title, message, settings);
		}

		public Task<MessageDialogResult> ShowMessageAsync(object context, string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative, MetroDialogSettings settings = null)
		{
			MetroWindow metroWindow = GetMetroWindow(context);
			return metroWindow.Invoke(() => metroWindow.ShowMessageAsync(title, message, style, settings));
		}

		public MessageDialogResult ShowModalMessageExternal(object context, string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative, MetroDialogSettings settings = null)
		{
			return GetMetroWindow(context).ShowModalMessageExternal(title, message, style, settings);
		}

		public Task<ProgressDialogController> ShowProgressAsync(object context, string title, string message, bool isCancelable = false, MetroDialogSettings settings = null)
		{
			MetroWindow metroWindow = GetMetroWindow(context);
			return metroWindow.Invoke(() => metroWindow.ShowProgressAsync(title, message, isCancelable, settings));
		}

		public Task ShowMetroDialogAsync(object context, BaseMetroDialog dialog, MetroDialogSettings settings = null)
		{
			MetroWindow metroWindow = GetMetroWindow(context);
			return metroWindow.Invoke(() => metroWindow.ShowMetroDialogAsync(dialog, settings));
		}

		public Task HideMetroDialogAsync(object context, BaseMetroDialog dialog, MetroDialogSettings settings = null)
		{
			MetroWindow metroWindow = GetMetroWindow(context);
			return metroWindow.Invoke(() => metroWindow.HideMetroDialogAsync(dialog, settings));
		}

		public Task<TDialog> GetCurrentDialogAsync<TDialog>(object context) where TDialog : BaseMetroDialog
		{
			MetroWindow metroWindow = GetMetroWindow(context);
			return metroWindow.Invoke(() => metroWindow.GetCurrentDialogAsync<TDialog>());
		}

		private static MetroWindow GetMetroWindow(object context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (!DialogParticipation.IsRegistered(context))
			{
				throw new InvalidOperationException("Context is not registered. Consider using DialogParticipation.Register in XAML to bind in the DataContext.");
			}
			DependencyObject association = DialogParticipation.GetAssociation(context);
			MetroWindow metroWindow = association.Invoke(() => Window.GetWindow(association) as MetroWindow);
			if (metroWindow == null)
			{
				throw new InvalidOperationException("Context is not inside a MetroWindow.");
			}
			return metroWindow;
		}
	}
}
