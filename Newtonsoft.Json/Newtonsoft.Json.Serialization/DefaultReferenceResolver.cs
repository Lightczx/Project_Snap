using Newtonsoft.Json.Utilities;
using System.Globalization;

namespace Newtonsoft.Json.Serialization
{
	internal class DefaultReferenceResolver : IReferenceResolver
	{
		private int _referenceCount;

		private BidirectionalDictionary<string, object> GetMappings(object context)
		{
			JsonSerializerInternalBase jsonSerializerInternalBase;
			if ((jsonSerializerInternalBase = (context as JsonSerializerInternalBase)) == null)
			{
				JsonSerializerProxy jsonSerializerProxy;
				if ((jsonSerializerProxy = (context as JsonSerializerProxy)) == null)
				{
					throw new JsonException("The DefaultReferenceResolver can only be used internally.");
				}
				jsonSerializerInternalBase = jsonSerializerProxy.GetInternalSerializer();
			}
			return jsonSerializerInternalBase.DefaultReferenceMappings;
		}

		public object ResolveReference(object context, string reference)
		{
			GetMappings(context).TryGetByFirst(reference, out object second);
			return second;
		}

		public string GetReference(object context, object value)
		{
			BidirectionalDictionary<string, object> mappings = GetMappings(context);
			if (!mappings.TryGetBySecond(value, out string first))
			{
				_referenceCount++;
				first = _referenceCount.ToString(CultureInfo.InvariantCulture);
				mappings.Set(first, value);
			}
			return first;
		}

		public void AddReference(object context, string reference, object value)
		{
			GetMappings(context).Set(reference, value);
		}

		public bool IsReferenced(object context, object value)
		{
			string first;
			return GetMappings(context).TryGetBySecond(value, out first);
		}
	}
}
