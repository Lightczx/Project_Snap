using System.Windows.Automation.Peers;

namespace MahApps.Metro.Controls
{
	public class FlyoutAutomationPeer : FrameworkElementAutomationPeer
	{
		public FlyoutAutomationPeer(Flyout owner)
			: base(owner)
		{
		}

		protected override string GetClassNameCore()
		{
			return "Flyout";
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
				text = (((Flyout)base.Owner).Header as string);
			}
			if (string.IsNullOrEmpty(text))
			{
				text = ((Flyout)base.Owner).Name;
			}
			if (string.IsNullOrEmpty(text))
			{
				text = GetClassNameCore();
			}
			return text;
		}
	}
}
