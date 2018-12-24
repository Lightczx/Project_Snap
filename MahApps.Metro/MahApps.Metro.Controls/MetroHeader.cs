using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	public class MetroHeader : GroupBox
	{
		static MetroHeader()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroHeader), new FrameworkPropertyMetadata(typeof(MetroHeader)));
		}

		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new MetroHeaderAutomationPeer(this);
		}
	}
}
