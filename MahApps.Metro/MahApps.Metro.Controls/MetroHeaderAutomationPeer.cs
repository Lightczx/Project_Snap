using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	public class MetroHeaderAutomationPeer : GroupBoxAutomationPeer
	{
		public MetroHeaderAutomationPeer(GroupBox owner)
			: base(owner)
		{
		}

		protected override string GetClassNameCore()
		{
			return "MetroHeader";
		}
	}
}
