using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Newtonsoft.Json.Linq.JsonPath
{
	internal class ArraySliceFilter : PathFilter
	{
		public int? Start
		{
			get;
			set;
		}

		public int? End
		{
			get;
			set;
		}

		public int? Step
		{
			get;
			set;
		}

		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			int? step = Step;
			int num = 0;
			if ((step.GetValueOrDefault() == num) & step.HasValue)
			{
				throw new JsonException("Step cannot be zero.");
			}
			foreach (JToken item in current)
			{
				JArray jArray;
				JArray a = jArray = (item as JArray);
				if (jArray != null)
				{
					int stepCount = Step ?? 1;
					int num2 = Start ?? ((stepCount <= 0) ? (a.Count - 1) : 0);
					int stopIndex3 = End ?? ((stepCount > 0) ? a.Count : (-1));
					step = Start;
					num = 0;
					if ((step.GetValueOrDefault() < num) & step.HasValue)
					{
						num2 = a.Count + num2;
					}
					step = End;
					num = 0;
					if ((step.GetValueOrDefault() < num) & step.HasValue)
					{
						stopIndex3 = a.Count + stopIndex3;
					}
					num2 = Math.Max(num2, (stepCount <= 0) ? (-2147483648) : 0);
					num2 = Math.Min(num2, (stepCount > 0) ? a.Count : (a.Count - 1));
					stopIndex3 = Math.Max(stopIndex3, -1);
					stopIndex3 = Math.Min(stopIndex3, a.Count);
					bool positiveStep = stepCount > 0;
					if (IsValid(num2, stopIndex3, positiveStep))
					{
						for (int i = num2; IsValid(i, stopIndex3, positiveStep); i += stepCount)
						{
							yield return a[i];
						}
					}
					else if (errorWhenNoMatch)
					{
						throw new JsonException("Array slice of {0} to {1} returned no results.".FormatWith(CultureInfo.InvariantCulture, Start.HasValue ? Start.GetValueOrDefault().ToString(CultureInfo.InvariantCulture) : "*", End.HasValue ? End.GetValueOrDefault().ToString(CultureInfo.InvariantCulture) : "*"));
					}
				}
				else if (errorWhenNoMatch)
				{
					throw new JsonException("Array slice is not valid on {0}.".FormatWith(CultureInfo.InvariantCulture, item.GetType().Name));
				}
			}
		}

		private bool IsValid(int index, int stopIndex, bool positiveStep)
		{
			if (positiveStep)
			{
				return index < stopIndex;
			}
			return index > stopIndex;
		}
	}
}
