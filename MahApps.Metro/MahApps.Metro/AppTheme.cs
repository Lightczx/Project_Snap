using System;
using System.Diagnostics;
using System.Windows;

namespace MahApps.Metro
{
	[DebuggerDisplay("apptheme={Name}, res={Resources.Source}")]
	public class AppTheme
	{
		public ResourceDictionary Resources
		{
			get;
		}

		public string Name
		{
			get;
		}

		public AppTheme(string name, Uri resourceAddress)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (resourceAddress == null)
			{
				throw new ArgumentNullException("resourceAddress");
			}
			Name = name;
			Resources = new ResourceDictionary
			{
				Source = resourceAddress
			};
		}

		public AppTheme(string name, ResourceDictionary resourceDictionary)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (resourceDictionary == null)
			{
				throw new ArgumentNullException("resourceDictionary");
			}
			Name = name;
			Resources = resourceDictionary;
		}
	}
}
