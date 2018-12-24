using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls.Dialogs
{
	public static class DialogManager
	{
		public static event EventHandler<DialogStateChangedEventArgs> DialogOpened;

		public static event EventHandler<DialogStateChangedEventArgs> DialogClosed;

		public static Task<LoginDialogData> ShowLoginAsync(this MetroWindow window, string title, string message, LoginDialogSettings settings = null)
		{
			window.Dispatcher.VerifyAccess();
			settings = (settings ?? new LoginDialogSettings());
			return HandleOverlayOnShow(settings, window).ContinueWith((Task z) => window.Dispatcher.Invoke(delegate
			{
				LoginDialog dialog = new LoginDialog(window, settings)
				{
					Title = title,
					Message = message
				};
				SetDialogFontSizes(settings, dialog);
				SizeChangedEventHandler sizeHandler = SetupAndOpenDialog(window, dialog);
				dialog.SizeChangedHandler = sizeHandler;
				return dialog.WaitForLoadAsync().ContinueWith(delegate
				{
					if (DialogManager.DialogOpened != null)
					{
						window.Dispatcher.BeginInvoke((Action)delegate
						{
							DialogManager.DialogOpened(window, new DialogStateChangedEventArgs());
						});
					}
					return dialog.WaitForButtonPressAsync().ContinueWith(delegate(Task<LoginDialogData> y)
					{
						dialog.OnClose();
						if (DialogManager.DialogClosed != null)
						{
							window.Dispatcher.BeginInvoke((Action)delegate
							{
								DialogManager.DialogClosed(window, new DialogStateChangedEventArgs());
							});
						}
						return window.Dispatcher.Invoke(() => dialog._WaitForCloseAsync()).ContinueWith((Task a) => window.Dispatcher.Invoke(delegate
						{
							window.SizeChanged -= sizeHandler;
							window.RemoveDialog(dialog);
							return HandleOverlayOnHide(settings, window);
						}).ContinueWith((Task y3) => y).Unwrap());
					}).Unwrap();
				}).Unwrap()
					.Unwrap();
			})).Unwrap();
		}

		public static Task<string> ShowInputAsync(this MetroWindow window, string title, string message, MetroDialogSettings settings = null)
		{
			window.Dispatcher.VerifyAccess();
			settings = (settings ?? window.MetroDialogOptions);
			return HandleOverlayOnShow(settings, window).ContinueWith((Task z) => window.Dispatcher.Invoke(delegate
			{
				InputDialog dialog = new InputDialog(window, settings)
				{
					Title = title,
					Message = message,
					Input = settings.DefaultText
				};
				SetDialogFontSizes(settings, dialog);
				SizeChangedEventHandler sizeHandler = SetupAndOpenDialog(window, dialog);
				dialog.SizeChangedHandler = sizeHandler;
				return dialog.WaitForLoadAsync().ContinueWith(delegate
				{
					if (DialogManager.DialogOpened != null)
					{
						window.Dispatcher.BeginInvoke((Action)delegate
						{
							DialogManager.DialogOpened(window, new DialogStateChangedEventArgs());
						});
					}
					return dialog.WaitForButtonPressAsync().ContinueWith(delegate(Task<string> y)
					{
						dialog.OnClose();
						if (DialogManager.DialogClosed != null)
						{
							window.Dispatcher.BeginInvoke((Action)delegate
							{
								DialogManager.DialogClosed(window, new DialogStateChangedEventArgs());
							});
						}
						return window.Dispatcher.Invoke(() => dialog._WaitForCloseAsync()).ContinueWith((Task a) => window.Dispatcher.Invoke(delegate
						{
							window.SizeChanged -= sizeHandler;
							window.RemoveDialog(dialog);
							return HandleOverlayOnHide(settings, window);
						}).ContinueWith((Task y3) => y).Unwrap());
					}).Unwrap();
				}).Unwrap()
					.Unwrap();
			})).Unwrap();
		}

		public static Task<MessageDialogResult> ShowMessageAsync(this MetroWindow window, string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative, MetroDialogSettings settings = null)
		{
			window.Dispatcher.VerifyAccess();
			settings = (settings ?? window.MetroDialogOptions);
			return HandleOverlayOnShow(settings, window).ContinueWith((Task z) => window.Dispatcher.Invoke(delegate
			{
				MessageDialog dialog = new MessageDialog(window, settings)
				{
					Message = message,
					Title = title,
					ButtonStyle = style
				};
				SetDialogFontSizes(settings, dialog);
				SizeChangedEventHandler sizeHandler = SetupAndOpenDialog(window, dialog);
				dialog.SizeChangedHandler = sizeHandler;
				return dialog.WaitForLoadAsync().ContinueWith(delegate
				{
					if (DialogManager.DialogOpened != null)
					{
						window.Dispatcher.BeginInvoke((Action)delegate
						{
							DialogManager.DialogOpened(window, new DialogStateChangedEventArgs());
						});
					}
					return dialog.WaitForButtonPressAsync().ContinueWith(delegate(Task<MessageDialogResult> y)
					{
						dialog.OnClose();
						if (DialogManager.DialogClosed != null)
						{
							window.Dispatcher.BeginInvoke((Action)delegate
							{
								DialogManager.DialogClosed(window, new DialogStateChangedEventArgs());
							});
						}
						return window.Dispatcher.Invoke(() => dialog._WaitForCloseAsync()).ContinueWith((Task a) => window.Dispatcher.Invoke(delegate
						{
							window.SizeChanged -= sizeHandler;
							window.RemoveDialog(dialog);
							return HandleOverlayOnHide(settings, window);
						}).ContinueWith((Task y3) => y).Unwrap());
					}).Unwrap();
				}).Unwrap()
					.Unwrap();
			})).Unwrap();
		}

		public static Task<ProgressDialogController> ShowProgressAsync(this MetroWindow window, string title, string message, bool isCancelable = false, MetroDialogSettings settings = null)
		{
			window.Dispatcher.VerifyAccess();
			settings = (settings ?? window.MetroDialogOptions);
			return HandleOverlayOnShow(settings, window).ContinueWith((Task z) => window.Dispatcher.Invoke(delegate
			{
				ProgressDialog dialog = new ProgressDialog(window, settings)
				{
					Title = title,
					Message = message,
					IsCancelable = isCancelable
				};
				SetDialogFontSizes(settings, dialog);
				SizeChangedEventHandler sizeHandler = SetupAndOpenDialog(window, dialog);
				dialog.SizeChangedHandler = sizeHandler;
				return dialog.WaitForLoadAsync().ContinueWith(delegate
				{
					if (DialogManager.DialogOpened != null)
					{
						window.Dispatcher.BeginInvoke((Action)delegate
						{
							DialogManager.DialogOpened(window, new DialogStateChangedEventArgs());
						});
					}
					return new ProgressDialogController(dialog, delegate
					{
						dialog.OnClose();
						if (DialogManager.DialogClosed != null)
						{
							window.Dispatcher.BeginInvoke((Action)delegate
							{
								DialogManager.DialogClosed(window, new DialogStateChangedEventArgs());
							});
						}
						return window.Dispatcher.Invoke(() => dialog._WaitForCloseAsync()).ContinueWith((Task a) => window.Dispatcher.Invoke(delegate
						{
							window.SizeChanged -= sizeHandler;
							window.RemoveDialog(dialog);
							return HandleOverlayOnHide(settings, window);
						})).Unwrap();
					});
				});
			})).Unwrap();
		}

		private static Task HandleOverlayOnHide(MetroDialogSettings settings, MetroWindow window)
		{
			Task task2 = null;
			if (!window.metroActiveDialogContainer.Children.OfType<BaseMetroDialog>().Any())
			{
				task2 = ((settings == null || settings.AnimateHide) ? window.HideOverlayAsync() : Task.Factory.StartNew(delegate
				{
					window.Dispatcher.Invoke(window.HideOverlay);
				}));
			}
			else
			{
				TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
				taskCompletionSource.SetResult(null);
				task2 = taskCompletionSource.Task;
			}
			task2.ContinueWith(delegate
			{
				window.Invoke(delegate
				{
					if (window.metroActiveDialogContainer.Children.Count == 0)
					{
						window.SetValue(MetroWindow.IsCloseButtonEnabledWithDialogPropertyKey, true);
						window.RestoreFocus();
					}
					else
					{
						MetroDialogSettings metroDialogSettings = window.metroActiveDialogContainer.Children.OfType<BaseMetroDialog>().LastOrDefault()?.DialogSettings;
						window.SetValue(MetroWindow.IsCloseButtonEnabledWithDialogPropertyKey, window.ShowDialogsOverTitleBar || metroDialogSettings == null || metroDialogSettings.OwnerCanCloseWithDialog);
					}
				});
			});
			return task2;
		}

		private static Task HandleOverlayOnShow(MetroDialogSettings settings, MetroWindow window)
		{
			return Task.Factory.StartNew(delegate
			{
				window.Invoke(delegate
				{
					window.SetValue(MetroWindow.IsCloseButtonEnabledWithDialogPropertyKey, window.ShowDialogsOverTitleBar || settings == null || settings.OwnerCanCloseWithDialog);
				});
			}).ContinueWith((Task task) => window.Invoke(delegate
			{
				if (!window.metroActiveDialogContainer.Children.OfType<BaseMetroDialog>().Any())
				{
					if (settings != null && !settings.AnimateShow)
					{
						return Task.Factory.StartNew(delegate
						{
							window.Dispatcher.Invoke(window.ShowOverlay);
						});
					}
					return window.ShowOverlayAsync();
				}
				TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
				taskCompletionSource.SetResult(null);
				return taskCompletionSource.Task;
			})).Unwrap();
		}

		public static Task ShowMetroDialogAsync(this MetroWindow window, BaseMetroDialog dialog, MetroDialogSettings settings = null)
		{
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}
			window.Dispatcher.VerifyAccess();
			if (dialog == null)
			{
				throw new ArgumentNullException("dialog");
			}
			if (window.metroActiveDialogContainer.Children.Contains(dialog) || window.metroInactiveDialogContainer.Children.Contains(dialog))
			{
				throw new InvalidOperationException("The provided dialog is already visible in the specified window.");
			}
			settings = (settings ?? dialog.DialogSettings ?? window.MetroDialogOptions);
			return HandleOverlayOnShow(settings, window).ContinueWith((Task z) => window.Dispatcher.Invoke(delegate
			{
				SetDialogFontSizes(settings, dialog);
				SizeChangedEventHandler sizeChangedHandler = SetupAndOpenDialog(window, dialog);
				dialog.SizeChangedHandler = sizeChangedHandler;
				return dialog.WaitForLoadAsync().ContinueWith(delegate
				{
					dialog.OnShown();
					if (DialogManager.DialogOpened != null)
					{
						window.Dispatcher.BeginInvoke((Action)delegate
						{
							DialogManager.DialogOpened(window, new DialogStateChangedEventArgs());
						});
					}
				});
			})).Unwrap();
		}

		public static Task<TDialog> ShowMetroDialogAsync<TDialog>(this MetroWindow window, MetroDialogSettings settings = null) where TDialog : BaseMetroDialog
		{
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}
			window.Dispatcher.VerifyAccess();
			TDialog dialog = (TDialog)(TDialog)Activator.CreateInstance(typeof(TDialog), window, settings);
			return HandleOverlayOnShow(dialog.DialogSettings, window).ContinueWith((Task z) => window.Dispatcher.Invoke(delegate
			{
				SetDialogFontSizes(dialog.DialogSettings, dialog);
				SizeChangedEventHandler sizeChangedHandler = SetupAndOpenDialog(window, dialog);
				dialog.SizeChangedHandler = sizeChangedHandler;
				return dialog.WaitForLoadAsync().ContinueWith(delegate
				{
					dialog.OnShown();
					if (DialogManager.DialogOpened != null)
					{
						window.Dispatcher.BeginInvoke((Action)delegate
						{
							DialogManager.DialogOpened(window, new DialogStateChangedEventArgs());
						});
					}
				}).ContinueWith((Task x) => (TDialog)dialog);
			})).Unwrap();
		}

		public static Task HideMetroDialogAsync(this MetroWindow window, BaseMetroDialog dialog, MetroDialogSettings settings = null)
		{
			window.Dispatcher.VerifyAccess();
			if (!window.metroActiveDialogContainer.Children.Contains(dialog) && !window.metroInactiveDialogContainer.Children.Contains(dialog))
			{
				throw new InvalidOperationException("The provided dialog is not visible in the specified window.");
			}
			window.SizeChanged -= dialog.SizeChangedHandler;
			dialog.OnClose();
			return window.Dispatcher.Invoke((Func<Task>)dialog._WaitForCloseAsync).ContinueWith(delegate
			{
				if (DialogManager.DialogClosed != null)
				{
					window.Dispatcher.BeginInvoke((Action)delegate
					{
						DialogManager.DialogClosed(window, new DialogStateChangedEventArgs());
					});
				}
				return window.Dispatcher.Invoke(delegate
				{
					window.RemoveDialog(dialog);
					settings = (settings ?? dialog.DialogSettings ?? window.MetroDialogOptions);
					return HandleOverlayOnHide(settings, window);
				});
			}).Unwrap();
		}

		public static Task<TDialog> GetCurrentDialogAsync<TDialog>(this MetroWindow window) where TDialog : BaseMetroDialog
		{
			window.Dispatcher.VerifyAccess();
			TaskCompletionSource<TDialog> t = (TaskCompletionSource<TDialog>)new TaskCompletionSource<TDialog>();
			window.Dispatcher.Invoke(delegate
			{
				Grid metroActiveDialogContainer = window.metroActiveDialogContainer;
				TDialog result = (metroActiveDialogContainer != null) ? metroActiveDialogContainer.Children.OfType<TDialog>().LastOrDefault() : null;
				((TaskCompletionSource<TDialog>)t).TrySetResult(result);
			});
			return ((TaskCompletionSource<TDialog>)t).Task;
		}

		private static SizeChangedEventHandler SetupAndOpenDialog(MetroWindow window, BaseMetroDialog dialog)
		{
			dialog.SetValue(Panel.ZIndexProperty, (int)window.overlayBox.GetValue(Panel.ZIndexProperty) + 1);
			dialog.MinHeight = window.ActualHeight / 4.0;
			dialog.MaxHeight = window.ActualHeight;
			SizeChangedEventHandler sizeChangedEventHandler = delegate
			{
				dialog.MinHeight = window.ActualHeight / 4.0;
				dialog.MaxHeight = window.ActualHeight;
			};
			window.SizeChanged += sizeChangedEventHandler;
			window.AddDialog(dialog);
			dialog.OnShown();
			return sizeChangedEventHandler;
		}

		private static void AddDialog(this MetroWindow window, BaseMetroDialog dialog)
		{
			window.StoreFocus();
			UIElement uIElement = window.metroActiveDialogContainer.Children.Cast<UIElement>().SingleOrDefault();
			if (uIElement != null)
			{
				window.metroActiveDialogContainer.Children.Remove(uIElement);
				window.metroInactiveDialogContainer.Children.Add(uIElement);
			}
			window.metroActiveDialogContainer.Children.Add(dialog);
			window.SetValue(MetroWindow.IsAnyDialogOpenPropertyKey, true);
		}

		private static void RemoveDialog(this MetroWindow window, BaseMetroDialog dialog)
		{
			if (window.metroActiveDialogContainer.Children.Contains(dialog))
			{
				window.metroActiveDialogContainer.Children.Remove(dialog);
				UIElement uIElement = window.metroInactiveDialogContainer.Children.Cast<UIElement>().LastOrDefault();
				if (uIElement != null)
				{
					window.metroInactiveDialogContainer.Children.Remove(uIElement);
					window.metroActiveDialogContainer.Children.Add(uIElement);
				}
			}
			else
			{
				window.metroInactiveDialogContainer.Children.Remove(dialog);
			}
			window.SetValue(MetroWindow.IsAnyDialogOpenPropertyKey, window.metroActiveDialogContainer.Children.Count > 0);
		}

		public static BaseMetroDialog ShowDialogExternally(this BaseMetroDialog dialog)
		{
			Window window = SetupExternalDialogWindow(dialog);
			dialog.OnShown();
			window.Show();
			return dialog;
		}

		public static BaseMetroDialog ShowModalDialogExternally(this BaseMetroDialog dialog)
		{
			Window window = SetupExternalDialogWindow(dialog);
			dialog.OnShown();
			window.ShowDialog();
			return dialog;
		}

		private static Window CreateExternalWindow()
		{
			return new MetroWindow
			{
				ShowInTaskbar = false,
				ShowActivated = true,
				Topmost = true,
				ResizeMode = ResizeMode.NoResize,
				WindowStyle = WindowStyle.None,
				WindowStartupLocation = WindowStartupLocation.CenterScreen,
				ShowTitleBar = false,
				ShowCloseButton = false,
				WindowTransitionsEnabled = false
			};
		}

		private static Window SetupExternalDialogWindow(BaseMetroDialog dialog)
		{
			Window win = CreateExternalWindow();
			win.Width = SystemParameters.PrimaryScreenWidth;
			win.MinHeight = SystemParameters.PrimaryScreenHeight / 4.0;
			win.SizeToContent = SizeToContent.Height;
			dialog.ParentDialogWindow = win;
			win.Content = dialog;
			dialog.HandleThemeChange();
			EventHandler closedHandler = null;
			closedHandler = delegate
			{
				win.Closed -= closedHandler;
				dialog.ParentDialogWindow = null;
				win.Content = null;
			};
			win.Closed += closedHandler;
			return win;
		}

		private static Window CreateModalExternalWindow(MetroWindow window)
		{
			Window window2 = CreateExternalWindow();
			window2.Owner = window;
			window2.Topmost = false;
			window2.WindowStartupLocation = WindowStartupLocation.CenterOwner;
			window2.Width = window.ActualWidth;
			window2.MaxHeight = window.ActualHeight;
			window2.SizeToContent = SizeToContent.Height;
			return window2;
		}

		public static LoginDialogData ShowModalLoginExternal(this MetroWindow window, string title, string message, LoginDialogSettings settings = null)
		{
			Window win = CreateModalExternalWindow(window);
			settings = (settings ?? new LoginDialogSettings());
			LoginDialog loginDialog = new LoginDialog(window, settings)
			{
				Title = title,
				Message = message
			};
			SetDialogFontSizes(settings, loginDialog);
			win.Content = loginDialog;
			LoginDialogData result = null;
			loginDialog.WaitForButtonPressAsync().ContinueWith(delegate(Task<LoginDialogData> task)
			{
				result = task.Result;
				win.Invoke(win.Close);
			});
			HandleOverlayOnShow(settings, window);
			win.ShowDialog();
			HandleOverlayOnHide(settings, window);
			return result;
		}

		public static string ShowModalInputExternal(this MetroWindow window, string title, string message, MetroDialogSettings settings = null)
		{
			Window win = CreateModalExternalWindow(window);
			settings = (settings ?? window.MetroDialogOptions);
			InputDialog inputDialog = new InputDialog(window, settings)
			{
				Message = message,
				Title = title,
				Input = settings.DefaultText
			};
			SetDialogFontSizes(settings, inputDialog);
			win.Content = inputDialog;
			string result = null;
			inputDialog.WaitForButtonPressAsync().ContinueWith(delegate(Task<string> task)
			{
				result = task.Result;
				win.Invoke(win.Close);
			});
			HandleOverlayOnShow(settings, window);
			win.ShowDialog();
			HandleOverlayOnHide(settings, window);
			return result;
		}

		public static MessageDialogResult ShowModalMessageExternal(this MetroWindow window, string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative, MetroDialogSettings settings = null)
		{
			Window win = CreateModalExternalWindow(window);
			settings = (settings ?? window.MetroDialogOptions);
			MessageDialog messageDialog = new MessageDialog(window, settings)
			{
				Message = message,
				Title = title,
				ButtonStyle = style
			};
			SetDialogFontSizes(settings, messageDialog);
			win.Content = messageDialog;
			MessageDialogResult result = MessageDialogResult.Affirmative;
			messageDialog.WaitForButtonPressAsync().ContinueWith(delegate(Task<MessageDialogResult> task)
			{
				result = task.Result;
				win.Invoke(win.Close);
			});
			HandleOverlayOnShow(settings, window);
			win.ShowDialog();
			HandleOverlayOnHide(settings, window);
			return result;
		}

		private static void SetDialogFontSizes(MetroDialogSettings settings, BaseMetroDialog dialog)
		{
			if (settings != null)
			{
				if (!double.IsNaN(settings.DialogTitleFontSize))
				{
					dialog.DialogTitleFontSize = settings.DialogTitleFontSize;
				}
				if (!double.IsNaN(settings.DialogMessageFontSize))
				{
					dialog.DialogMessageFontSize = settings.DialogMessageFontSize;
				}
			}
		}
	}
}
