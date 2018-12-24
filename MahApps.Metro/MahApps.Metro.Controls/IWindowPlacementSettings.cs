using ControlzEx.Standard;

namespace MahApps.Metro.Controls
{
	public interface IWindowPlacementSettings
	{
		WINDOWPLACEMENT Placement
		{
			get;
			set;
		}

		bool UpgradeSettings
		{
			get;
			set;
		}

		void Reload();

		void Upgrade();

		void Save();
	}
}
