using System.Windows.Automation.Peers;

namespace MahApps.Metro.Controls.Dialogs
{
	public class MetroDialogAutomationPeer : FrameworkElementAutomationPeer
	{
		public MetroDialogAutomationPeer(BaseMetroDialog owner)
			: base(owner)
		{
		}

		protected override string GetClassNameCore()
		{
			return base.Owner.GetType().Name;
		}

		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Custom;
		}

		protected override string GetNameCore()
		{
			string text = base.GetNameCore();
			if (string.IsNullOrEmpty(text))
			{
				text = ((BaseMetroDialog)base.Owner).Title;
			}
			if (string.IsNullOrEmpty(text))
			{
				text = ((BaseMetroDialog)base.Owner).Name;
			}
			if (string.IsNullOrEmpty(text))
			{
				text = GetClassNameCore();
			}
			return text;
		}
	}
}
