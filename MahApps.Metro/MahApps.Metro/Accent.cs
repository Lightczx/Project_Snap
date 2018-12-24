using System;
using System.Diagnostics;
using System.Windows;

namespace MahApps.Metro
{
	[DebuggerDisplay("accent={Name}, res={Resources.Source}")]
	public class Accent
	{
		public ResourceDictionary Resources
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public Accent()
		{
		}

		public Accent(string name, Uri resourceAddress)
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

		public Accent(string name, ResourceDictionary resourceDictionary)
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
