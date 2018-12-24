using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
	internal class ArrayMultipleIndexFilter : PathFilter
	{
		public List<int> Indexes
		{
			get;
			set;
		}

		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			foreach (JToken item in current)
			{
				foreach (int index in Indexes)
				{
					JToken tokenIndex = PathFilter.GetTokenIndex(item, errorWhenNoMatch, index);
					if (tokenIndex != null)
					{
						yield return tokenIndex;
					}
				}
			}
		}
	}
}
