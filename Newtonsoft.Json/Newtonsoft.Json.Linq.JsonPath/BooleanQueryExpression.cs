using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Newtonsoft.Json.Linq.JsonPath
{
	internal class BooleanQueryExpression : QueryExpression
	{
		public object Left
		{
			get;
			set;
		}

		public object Right
		{
			get;
			set;
		}

		private IEnumerable<JToken> GetResult(JToken root, JToken t, object o)
		{
			JToken jToken;
			if ((jToken = (o as JToken)) != null)
			{
				return new JToken[1]
				{
					jToken
				};
			}
			List<PathFilter> filters;
			if ((filters = (o as List<PathFilter>)) != null)
			{
				return JPath.Evaluate(filters, root, t, errorWhenNoMatch: false);
			}
			return CollectionUtils.ArrayEmpty<JToken>();
		}

		public override bool IsMatch(JToken root, JToken t)
		{
			if (base.Operator == QueryOperator.Exists)
			{
				return GetResult(root, t, Left).Any();
			}
			using (IEnumerator<JToken> enumerator = GetResult(root, t, Left).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					IEnumerable<JToken> result = GetResult(root, t, Right);
					ICollection<JToken> collection = (result as ICollection<JToken>) ?? result.ToList();
					do
					{
						JToken current = enumerator.Current;
						foreach (JToken item in collection)
						{
							if (MatchTokens(current, item))
							{
								return true;
							}
						}
					}
					while (enumerator.MoveNext());
				}
			}
			return false;
		}

		private bool MatchTokens(JToken leftResult, JToken rightResult)
		{
			JValue jValue;
			JValue jValue2;
			if ((jValue = (leftResult as JValue)) != null && (jValue2 = (rightResult as JValue)) != null)
			{
				switch (base.Operator)
				{
				case QueryOperator.RegexEquals:
					if (RegexEquals(jValue, jValue2))
					{
						return true;
					}
					break;
				case QueryOperator.Equals:
					if (EqualsWithStringCoercion(jValue, jValue2))
					{
						return true;
					}
					break;
				case QueryOperator.StrictEquals:
					if (EqualsWithStrictMatch(jValue, jValue2))
					{
						return true;
					}
					break;
				case QueryOperator.NotEquals:
					if (!EqualsWithStringCoercion(jValue, jValue2))
					{
						return true;
					}
					break;
				case QueryOperator.StrictNotEquals:
					if (!EqualsWithStrictMatch(jValue, jValue2))
					{
						return true;
					}
					break;
				case QueryOperator.GreaterThan:
					if (jValue.CompareTo(jValue2) > 0)
					{
						return true;
					}
					break;
				case QueryOperator.GreaterThanOrEquals:
					if (jValue.CompareTo(jValue2) >= 0)
					{
						return true;
					}
					break;
				case QueryOperator.LessThan:
					if (jValue.CompareTo(jValue2) < 0)
					{
						return true;
					}
					break;
				case QueryOperator.LessThanOrEquals:
					if (jValue.CompareTo(jValue2) <= 0)
					{
						return true;
					}
					break;
				case QueryOperator.Exists:
					return true;
				}
			}
			else
			{
				QueryOperator @operator = base.Operator;
				if ((uint)(@operator - 2) <= 1u)
				{
					return true;
				}
			}
			return false;
		}

		private static bool RegexEquals(JValue input, JValue pattern)
		{
			if (input.Type != JTokenType.String || pattern.Type != JTokenType.String)
			{
				return false;
			}
			string obj = (string)pattern.Value;
			int num = obj.LastIndexOf('/');
			string pattern2 = obj.Substring(1, num - 1);
			string optionsText = obj.Substring(num + 1);
			return Regex.IsMatch((string)input.Value, pattern2, MiscellaneousUtils.GetRegexOptions(optionsText));
		}

		internal static bool EqualsWithStringCoercion(JValue value, JValue queryValue)
		{
			if (value.Equals(queryValue))
			{
				return true;
			}
			if ((value.Type == JTokenType.Integer && queryValue.Type == JTokenType.Float) || (value.Type == JTokenType.Float && queryValue.Type == JTokenType.Integer))
			{
				return JValue.Compare(value.Type, value.Value, queryValue.Value) == 0;
			}
			if (queryValue.Type != JTokenType.String)
			{
				return false;
			}
			string b = (string)queryValue.Value;
			string a;
			switch (value.Type)
			{
			case JTokenType.Date:
				using (StringWriter stringWriter = StringUtils.CreateStringWriter(64))
				{
					object value2;
					if ((value2 = value.Value) is DateTimeOffset)
					{
						DateTimeOffset value3 = (DateTimeOffset)value2;
						DateTimeUtils.WriteDateTimeOffsetString(stringWriter, value3, DateFormatHandling.IsoDateFormat, null, CultureInfo.InvariantCulture);
					}
					else
					{
						DateTimeUtils.WriteDateTimeString(stringWriter, (DateTime)value.Value, DateFormatHandling.IsoDateFormat, null, CultureInfo.InvariantCulture);
					}
					a = stringWriter.ToString();
				}
				break;
			case JTokenType.Bytes:
				a = Convert.ToBase64String((byte[])value.Value);
				break;
			case JTokenType.Guid:
			case JTokenType.TimeSpan:
				a = value.Value.ToString();
				break;
			case JTokenType.Uri:
				a = ((Uri)value.Value).OriginalString;
				break;
			default:
				return false;
			}
			return string.Equals(a, b, StringComparison.Ordinal);
		}

		internal static bool EqualsWithStrictMatch(JValue value, JValue queryValue)
		{
			if ((value.Type == JTokenType.Integer && queryValue.Type == JTokenType.Float) || (value.Type == JTokenType.Float && queryValue.Type == JTokenType.Integer))
			{
				return JValue.Compare(value.Type, value.Value, queryValue.Value) == 0;
			}
			if (value.Type != queryValue.Type)
			{
				return false;
			}
			return value.Equals(queryValue);
		}
	}
}
