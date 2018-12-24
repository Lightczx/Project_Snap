using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MahApps.Metro.Controls.Dialogs
{
	public class ProgressDialogController
	{
		private ProgressDialog WrappedDialog
		{
			get;
		}

		private Func<Task> CloseCallback
		{
			get;
		}

		public bool IsCanceled
		{
			get;
			private set;
		}

		public bool IsOpen
		{
			get;
			private set;
		}

		public double Minimum
		{
			get
			{
				return WrappedDialog.Invoke(() => WrappedDialog.Minimum);
			}
			set
			{
				WrappedDialog.Invoke(() => WrappedDialog.Minimum = value);
			}
		}

		public double Maximum
		{
			get
			{
				return WrappedDialog.Invoke(() => WrappedDialog.Maximum);
			}
			set
			{
				WrappedDialog.Invoke(() => WrappedDialog.Maximum = value);
			}
		}

		public event EventHandler Closed;

		public event EventHandler Canceled;

		internal ProgressDialogController(ProgressDialog dialog, Func<Task> closeCallBack)
		{
			WrappedDialog = dialog;
			CloseCallback = closeCallBack;
			IsOpen = dialog.IsVisible;
			WrappedDialog.Invoke(delegate
			{
				WrappedDialog.PART_NegativeButton.Click += PART_NegativeButton_Click;
			});
			dialog.CancellationToken.Register(delegate
			{
				PART_NegativeButton_Click(null, new RoutedEventArgs());
			});
		}

		private void PART_NegativeButton_Click(object sender, RoutedEventArgs e)
		{
			Action invokeAction = delegate
			{
				IsCanceled = true;
				this.Canceled?.Invoke(this, EventArgs.Empty);
				WrappedDialog.PART_NegativeButton.IsEnabled = false;
			};
			WrappedDialog.Invoke(invokeAction);
		}

		public void SetIndeterminate()
		{
			WrappedDialog.Invoke(delegate
			{
				WrappedDialog.SetIndeterminate();
			});
		}

		public void SetCancelable(bool value)
		{
			WrappedDialog.Invoke(() => WrappedDialog.IsCancelable = value);
		}

		public void SetProgress(double value)
		{
			Action invokeAction = delegate
			{
				if (value < WrappedDialog.Minimum || value > WrappedDialog.Maximum)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				WrappedDialog.ProgressValue = value;
			};
			WrappedDialog.Invoke(invokeAction);
		}

		public void SetMessage(string message)
		{
			WrappedDialog.Invoke(() => WrappedDialog.Message = message);
		}

		public void SetTitle(string title)
		{
			WrappedDialog.Invoke(() => WrappedDialog.Title = title);
		}

		public void SetProgressBarForegroundBrush(Brush brush)
		{
			WrappedDialog.Invoke(() => WrappedDialog.ProgressBarForeground = brush);
		}

		public Task CloseAsync()
		{
			Action invokeAction = delegate
			{
				if (!WrappedDialog.IsVisible)
				{
					throw new InvalidOperationException("Dialog isn't visible to close");
				}
				WrappedDialog.Dispatcher.VerifyAccess();
				WrappedDialog.PART_NegativeButton.Click -= PART_NegativeButton_Click;
			};
			WrappedDialog.Invoke(invokeAction);
			return CloseCallback().ContinueWith(delegate
			{
				WrappedDialog.Invoke(delegate
				{
					IsOpen = false;
					this.Closed?.Invoke(this, EventArgs.Empty);
				});
			});
		}
	}
}
