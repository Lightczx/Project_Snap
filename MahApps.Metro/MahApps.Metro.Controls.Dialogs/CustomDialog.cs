namespace MahApps.Metro.Controls.Dialogs
{
	public class CustomDialog : BaseMetroDialog
	{
		public CustomDialog()
			: this(null, null)
		{
		}

		public CustomDialog(MetroWindow parentWindow)
			: this(parentWindow, null)
		{
		}

		public CustomDialog(MetroDialogSettings settings)
			: this(null, settings)
		{
		}

		public CustomDialog(MetroWindow parentWindow, MetroDialogSettings settings)
			: base(parentWindow, settings)
		{
		}
	}
}
