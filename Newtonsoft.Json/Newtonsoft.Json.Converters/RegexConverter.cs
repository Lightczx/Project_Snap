using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Newtonsoft.Json.Converters
{
	public class RegexConverter : JsonConverter
	{
		private const string PatternName = "Pattern";

		private const string OptionsName = "Options";

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
			}
			else
			{
				Regex regex = (Regex)value;
				BsonWriter writer2;
				if ((writer2 = (writer as BsonWriter)) != null)
				{
					WriteBson(writer2, regex);
				}
				else
				{
					WriteJson(writer, regex, serializer);
				}
			}
		}

		private bool HasFlag(RegexOptions options, RegexOptions flag)
		{
			return (options & flag) == flag;
		}

		private void WriteBson(BsonWriter writer, Regex regex)
		{
			string str = null;
			if (HasFlag(regex.Options, RegexOptions.IgnoreCase))
			{
				str += "i";
			}
			if (HasFlag(regex.Options, RegexOptions.Multiline))
			{
				str += "m";
			}
			if (HasFlag(regex.Options, RegexOptions.Singleline))
			{
				str += "s";
			}
			str += "u";
			if (HasFlag(regex.Options, RegexOptions.ExplicitCapture))
			{
				str += "x";
			}
			writer.WriteRegex(regex.ToString(), str);
		}

		private void WriteJson(JsonWriter writer, Regex regex, JsonSerializer serializer)
		{
			DefaultContractResolver defaultContractResolver = serializer.ContractResolver as DefaultContractResolver;
			writer.WriteStartObject();
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Pattern") : "Pattern");
			writer.WriteValue(regex.ToString());
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Options") : "Options");
			serializer.Serialize(writer, regex.Options);
			writer.WriteEndObject();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			switch (reader.TokenType)
			{
			case JsonToken.StartObject:
				return ReadRegexObject(reader, serializer);
			case JsonToken.String:
				return ReadRegexString(reader);
			case JsonToken.Null:
				return null;
			default:
				throw JsonSerializationException.Create(reader, "Unexpected token when reading Regex.");
			}
		}

		private object ReadRegexString(JsonReader reader)
		{
			string text = (string)reader.Value;
			if (text.Length > 0 && text[0] == '/')
			{
				int num = text.LastIndexOf('/');
				if (num > 0)
				{
					string pattern = text.Substring(1, num - 1);
					RegexOptions regexOptions = MiscellaneousUtils.GetRegexOptions(text.Substring(num + 1));
					return new Regex(pattern, regexOptions);
				}
			}
			throw JsonSerializationException.Create(reader, "Regex pattern must be enclosed by slashes.");
		}

		private Regex ReadRegexObject(JsonReader reader, JsonSerializer serializer)
		{
			string text = null;
			RegexOptions? regexOptions = null;
			while (reader.Read())
			{
				switch (reader.TokenType)
				{
				case JsonToken.PropertyName:
				{
					string a = reader.Value.ToString();
					if (!reader.Read())
					{
						throw JsonSerializationException.Create(reader, "Unexpected end when reading Regex.");
					}
					if (string.Equals(a, "Pattern", StringComparison.OrdinalIgnoreCase))
					{
						text = (string)reader.Value;
					}
					else if (string.Equals(a, "Options", StringComparison.OrdinalIgnoreCase))
					{
						regexOptions = serializer.Deserialize<RegexOptions>(reader);
					}
					else
					{
						reader.Skip();
					}
					break;
				}
				case JsonToken.EndObject:
					if (text == null)
					{
						throw JsonSerializationException.Create(reader, "Error deserializing Regex. No pattern found.");
					}
					return new Regex(text, regexOptions ?? RegexOptions.None);
				}
			}
			throw JsonSerializationException.Create(reader, "Unexpected end when reading Regex.");
		}

		public override bool CanConvert(Type objectType)
		{
			if (objectType.Name == "Regex")
			{
				return IsRegex(objectType);
			}
			return false;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool IsRegex(Type objectType)
		{
			return objectType == typeof(Regex);
		}
	}
}
