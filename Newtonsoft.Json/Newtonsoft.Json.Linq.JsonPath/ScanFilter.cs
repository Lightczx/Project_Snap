using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
	internal class ScanFilter : PathFilter
	{
		public string Name
		{
			get;
			set;
		}

		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			foreach (JToken item in current)
			{
				if (Name == null)
				{
					yield return item;
				}
				JToken value = item;
				while (true)
				{
					JContainer container = value as JContainer;
					value = PathFilter.GetNextScanValue(item, container, value);
					if (value == null)
					{
						break;
					}
					JProperty jProperty;
					if ((jProperty = (value as JProperty)) != null)
					{
						if (jProperty.Name == Name)
						{
							yield return jProperty.Value;
						}
					}
					else if (Name == null)
					{
						yield return value;
					}
				}
			}
		}
	}
}
