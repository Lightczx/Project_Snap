using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	public class XmlNodeConverter : JsonConverter
	{
		internal static readonly List<IXmlNode> EmptyChildNodes = new List<IXmlNode>();

		private const string TextName = "#text";

		private const string CommentName = "#comment";

		private const string CDataName = "#cdata-section";

		private const string WhitespaceName = "#whitespace";

		private const string SignificantWhitespaceName = "#significant-whitespace";

		private const string DeclarationName = "?xml";

		private const string JsonNamespaceUri = "http://james.newtonking.com/projects/json";

		public string DeserializeRootElementName
		{
			get;
			set;
		}

		public bool WriteArrayAttribute
		{
			get;
			set;
		}

		public bool OmitRootObject
		{
			get;
			set;
		}

		public bool EncodeSpecialCharacters
		{
			get;
			set;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
			}
			else
			{
				IXmlNode node = WrapXml(value);
				XmlNamespaceManager manager = new XmlNamespaceManager(new NameTable());
				PushParentNamespaces(node, manager);
				if (!OmitRootObject)
				{
					writer.WriteStartObject();
				}
				SerializeNode(writer, node, manager, !OmitRootObject);
				if (!OmitRootObject)
				{
					writer.WriteEndObject();
				}
			}
		}

		private IXmlNode WrapXml(object value)
		{
			XObject node;
			if ((node = (value as XObject)) != null)
			{
				return XContainerWrapper.WrapNode(node);
			}
			XmlNode node2;
			if ((node2 = (value as XmlNode)) != null)
			{
				return XmlNodeWrapper.WrapNode(node2);
			}
			throw new ArgumentException("Value must be an XML object.", "value");
		}

		private void PushParentNamespaces(IXmlNode node, XmlNamespaceManager manager)
		{
			List<IXmlNode> list = null;
			IXmlNode xmlNode = node;
			while ((xmlNode = xmlNode.ParentNode) != null)
			{
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					if (list == null)
					{
						list = new List<IXmlNode>();
					}
					list.Add(xmlNode);
				}
			}
			if (list != null)
			{
				list.Reverse();
				foreach (IXmlNode item in list)
				{
					manager.PushScope();
					foreach (IXmlNode attribute in item.Attributes)
					{
						if (attribute.NamespaceUri == "http://www.w3.org/2000/xmlns/" && attribute.LocalName != "xmlns")
						{
							manager.AddNamespace(attribute.LocalName, attribute.Value);
						}
					}
				}
			}
		}

		private string ResolveFullName(IXmlNode node, XmlNamespaceManager manager)
		{
			string text = (node.NamespaceUri == null || (node.LocalName == "xmlns" && node.NamespaceUri == "http://www.w3.org/2000/xmlns/")) ? null : manager.LookupPrefix(node.NamespaceUri);
			if (!string.IsNullOrEmpty(text))
			{
				return text + ":" + XmlConvert.DecodeName(node.LocalName);
			}
			return XmlConvert.DecodeName(node.LocalName);
		}

		private string GetPropertyName(IXmlNode node, XmlNamespaceManager manager)
		{
			switch (node.NodeType)
			{
			case XmlNodeType.Attribute:
				if (node.NamespaceUri == "http://james.newtonking.com/projects/json")
				{
					return "$" + node.LocalName;
				}
				return "@" + ResolveFullName(node, manager);
			case XmlNodeType.CDATA:
				return "#cdata-section";
			case XmlNodeType.Comment:
				return "#comment";
			case XmlNodeType.Element:
				if (node.NamespaceUri == "http://james.newtonking.com/projects/json")
				{
					return "$" + node.LocalName;
				}
				return ResolveFullName(node, manager);
			case XmlNodeType.ProcessingInstruction:
				return "?" + ResolveFullName(node, manager);
			case XmlNodeType.DocumentType:
				return "!" + ResolveFullName(node, manager);
			case XmlNodeType.XmlDeclaration:
				return "?xml";
			case XmlNodeType.SignificantWhitespace:
				return "#significant-whitespace";
			case XmlNodeType.Text:
				return "#text";
			case XmlNodeType.Whitespace:
				return "#whitespace";
			default:
				throw new JsonSerializationException("Unexpected XmlNodeType when getting node name: " + node.NodeType);
			}
		}

		private bool IsArray(IXmlNode node)
		{
			foreach (IXmlNode attribute in node.Attributes)
			{
				if (attribute.LocalName == "Array" && attribute.NamespaceUri == "http://james.newtonking.com/projects/json")
				{
					return XmlConvert.ToBoolean(attribute.Value);
				}
			}
			return false;
		}

		private void SerializeGroupedNodes(JsonWriter writer, IXmlNode node, XmlNamespaceManager manager, bool writePropertyName)
		{
			switch (node.ChildNodes.Count)
			{
			case 0:
				break;
			case 1:
			{
				string propertyName2 = GetPropertyName(node.ChildNodes[0], manager);
				WriteGroupedNodes(writer, manager, writePropertyName, node.ChildNodes, propertyName2);
				break;
			}
			default:
			{
				Dictionary<string, object> dictionary = null;
				string text = null;
				for (int i = 0; i < node.ChildNodes.Count; i++)
				{
					IXmlNode xmlNode = node.ChildNodes[i];
					string propertyName = GetPropertyName(xmlNode, manager);
					object value;
					if (dictionary == null)
					{
						if (text == null)
						{
							text = propertyName;
						}
						else if (!(propertyName == text))
						{
							dictionary = new Dictionary<string, object>();
							if (i > 1)
							{
								List<IXmlNode> list = new List<IXmlNode>(i);
								for (int j = 0; j < i; j++)
								{
									list.Add(node.ChildNodes[j]);
								}
								dictionary.Add(text, list);
							}
							else
							{
								dictionary.Add(text, node.ChildNodes[0]);
							}
							dictionary.Add(propertyName, xmlNode);
						}
					}
					else if (!dictionary.TryGetValue(propertyName, out value))
					{
						dictionary.Add(propertyName, xmlNode);
					}
					else
					{
						List<IXmlNode> list2;
						if ((list2 = (value as List<IXmlNode>)) == null)
						{
							list2 = (List<IXmlNode>)(dictionary[propertyName] = new List<IXmlNode>
							{
								(IXmlNode)value
							});
						}
						list2.Add(xmlNode);
					}
				}
				if (dictionary == null)
				{
					WriteGroupedNodes(writer, manager, writePropertyName, node.ChildNodes, text);
				}
				else
				{
					foreach (KeyValuePair<string, object> item in dictionary)
					{
						List<IXmlNode> groupedNodes;
						if ((groupedNodes = (item.Value as List<IXmlNode>)) != null)
						{
							WriteGroupedNodes(writer, manager, writePropertyName, groupedNodes, item.Key);
						}
						else
						{
							WriteGroupedNodes(writer, manager, writePropertyName, (IXmlNode)item.Value, item.Key);
						}
					}
				}
				break;
			}
			}
		}

		private void WriteGroupedNodes(JsonWriter writer, XmlNamespaceManager manager, bool writePropertyName, List<IXmlNode> groupedNodes, string elementNames)
		{
			if (groupedNodes.Count == 1 && !IsArray(groupedNodes[0]))
			{
				SerializeNode(writer, groupedNodes[0], manager, writePropertyName);
			}
			else
			{
				if (writePropertyName)
				{
					writer.WritePropertyName(elementNames);
				}
				writer.WriteStartArray();
				for (int i = 0; i < groupedNodes.Count; i++)
				{
					SerializeNode(writer, groupedNodes[i], manager, writePropertyName: false);
				}
				writer.WriteEndArray();
			}
		}

		private void WriteGroupedNodes(JsonWriter writer, XmlNamespaceManager manager, bool writePropertyName, IXmlNode node, string elementNames)
		{
			if (!IsArray(node))
			{
				SerializeNode(writer, node, manager, writePropertyName);
			}
			else
			{
				if (writePropertyName)
				{
					writer.WritePropertyName(elementNames);
				}
				writer.WriteStartArray();
				SerializeNode(writer, node, manager, writePropertyName: false);
				writer.WriteEndArray();
			}
		}

		private void SerializeNode(JsonWriter writer, IXmlNode node, XmlNamespaceManager manager, bool writePropertyName)
		{
			switch (node.NodeType)
			{
			case XmlNodeType.Document:
			case XmlNodeType.DocumentFragment:
				SerializeGroupedNodes(writer, node, manager, writePropertyName);
				break;
			case XmlNodeType.Element:
				if (IsArray(node) && AllSameName(node) && node.ChildNodes.Count > 0)
				{
					SerializeGroupedNodes(writer, node, manager, writePropertyName: false);
				}
				else
				{
					manager.PushScope();
					foreach (IXmlNode attribute in node.Attributes)
					{
						if (attribute.NamespaceUri == "http://www.w3.org/2000/xmlns/")
						{
							string prefix = (attribute.LocalName != "xmlns") ? XmlConvert.DecodeName(attribute.LocalName) : string.Empty;
							string value = attribute.Value;
							manager.AddNamespace(prefix, value);
						}
					}
					if (writePropertyName)
					{
						writer.WritePropertyName(GetPropertyName(node, manager));
					}
					if (!ValueAttributes(node.Attributes) && node.ChildNodes.Count == 1 && node.ChildNodes[0].NodeType == XmlNodeType.Text)
					{
						writer.WriteValue(node.ChildNodes[0].Value);
					}
					else if (node.ChildNodes.Count == 0 && node.Attributes.Count == 0)
					{
						if (((IXmlElement)node).IsEmpty)
						{
							writer.WriteNull();
						}
						else
						{
							writer.WriteValue(string.Empty);
						}
					}
					else
					{
						writer.WriteStartObject();
						for (int i = 0; i < node.Attributes.Count; i++)
						{
							SerializeNode(writer, node.Attributes[i], manager, writePropertyName: true);
						}
						SerializeGroupedNodes(writer, node, manager, writePropertyName: true);
						writer.WriteEndObject();
					}
					manager.PopScope();
				}
				break;
			case XmlNodeType.Comment:
				if (writePropertyName)
				{
					writer.WriteComment(node.Value);
				}
				break;
			case XmlNodeType.Attribute:
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
			case XmlNodeType.ProcessingInstruction:
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
				if ((!(node.NamespaceUri == "http://www.w3.org/2000/xmlns/") || !(node.Value == "http://james.newtonking.com/projects/json")) && (!(node.NamespaceUri == "http://james.newtonking.com/projects/json") || !(node.LocalName == "Array")))
				{
					if (writePropertyName)
					{
						writer.WritePropertyName(GetPropertyName(node, manager));
					}
					writer.WriteValue(node.Value);
				}
				break;
			case XmlNodeType.XmlDeclaration:
			{
				IXmlDeclaration xmlDeclaration = (IXmlDeclaration)node;
				writer.WritePropertyName(GetPropertyName(node, manager));
				writer.WriteStartObject();
				if (!string.IsNullOrEmpty(xmlDeclaration.Version))
				{
					writer.WritePropertyName("@version");
					writer.WriteValue(xmlDeclaration.Version);
				}
				if (!string.IsNullOrEmpty(xmlDeclaration.Encoding))
				{
					writer.WritePropertyName("@encoding");
					writer.WriteValue(xmlDeclaration.Encoding);
				}
				if (!string.IsNullOrEmpty(xmlDeclaration.Standalone))
				{
					writer.WritePropertyName("@standalone");
					writer.WriteValue(xmlDeclaration.Standalone);
				}
				writer.WriteEndObject();
				break;
			}
			case XmlNodeType.DocumentType:
			{
				IXmlDocumentType xmlDocumentType = (IXmlDocumentType)node;
				writer.WritePropertyName(GetPropertyName(node, manager));
				writer.WriteStartObject();
				if (!string.IsNullOrEmpty(xmlDocumentType.Name))
				{
					writer.WritePropertyName("@name");
					writer.WriteValue(xmlDocumentType.Name);
				}
				if (!string.IsNullOrEmpty(xmlDocumentType.Public))
				{
					writer.WritePropertyName("@public");
					writer.WriteValue(xmlDocumentType.Public);
				}
				if (!string.IsNullOrEmpty(xmlDocumentType.System))
				{
					writer.WritePropertyName("@system");
					writer.WriteValue(xmlDocumentType.System);
				}
				if (!string.IsNullOrEmpty(xmlDocumentType.InternalSubset))
				{
					writer.WritePropertyName("@internalSubset");
					writer.WriteValue(xmlDocumentType.InternalSubset);
				}
				writer.WriteEndObject();
				break;
			}
			default:
				throw new JsonSerializationException("Unexpected XmlNodeType when serializing nodes: " + node.NodeType);
			}
		}

		private static bool AllSameName(IXmlNode node)
		{
			foreach (IXmlNode childNode in node.ChildNodes)
			{
				if (childNode.LocalName != node.LocalName)
				{
					return false;
				}
			}
			return true;
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			switch (reader.TokenType)
			{
			case JsonToken.Null:
				return null;
			default:
				throw JsonSerializationException.Create(reader, "XmlNodeConverter can only convert JSON that begins with an object.");
			case JsonToken.StartObject:
			{
				XmlNamespaceManager manager = new XmlNamespaceManager(new NameTable());
				IXmlDocument xmlDocument = null;
				IXmlNode xmlNode = null;
				if (typeof(XObject).IsAssignableFrom(objectType))
				{
					if (objectType != typeof(XContainer) && objectType != typeof(XDocument) && objectType != typeof(XElement) && objectType != typeof(XNode) && objectType != typeof(XObject))
					{
						throw JsonSerializationException.Create(reader, "XmlNodeConverter only supports deserializing XDocument, XElement, XContainer, XNode or XObject.");
					}
					xmlDocument = new XDocumentWrapper(new XDocument());
					xmlNode = xmlDocument;
				}
				if (typeof(XmlNode).IsAssignableFrom(objectType))
				{
					if (objectType != typeof(XmlDocument) && objectType != typeof(XmlElement) && objectType != typeof(XmlNode))
					{
						throw JsonSerializationException.Create(reader, "XmlNodeConverter only supports deserializing XmlDocument, XmlElement or XmlNode.");
					}
					xmlDocument = new XmlDocumentWrapper(new XmlDocument
					{
						XmlResolver = null
					});
					xmlNode = xmlDocument;
				}
				if (xmlDocument == null || xmlNode == null)
				{
					throw JsonSerializationException.Create(reader, "Unexpected type when converting XML: " + objectType);
				}
				if (!string.IsNullOrEmpty(DeserializeRootElementName))
				{
					ReadElement(reader, xmlDocument, xmlNode, DeserializeRootElementName, manager);
				}
				else
				{
					reader.ReadAndAssert();
					DeserializeNode(reader, xmlDocument, manager, xmlNode);
				}
				if (objectType == typeof(XElement))
				{
					XElement obj = (XElement)xmlDocument.DocumentElement.WrappedNode;
					obj.Remove();
					return obj;
				}
				if (objectType == typeof(XmlElement))
				{
					return xmlDocument.DocumentElement.WrappedNode;
				}
				return xmlDocument.WrappedNode;
			}
			}
		}

		private void DeserializeValue(JsonReader reader, IXmlDocument document, XmlNamespaceManager manager, string propertyName, IXmlNode currentNode)
		{
			if (!EncodeSpecialCharacters)
			{
				if (propertyName == "#text")
				{
					currentNode.AppendChild(document.CreateTextNode(ConvertTokenToXmlValue(reader)));
					return;
				}
				if (propertyName == "#cdata-section")
				{
					currentNode.AppendChild(document.CreateCDataSection(ConvertTokenToXmlValue(reader)));
					return;
				}
				if (propertyName == "#whitespace")
				{
					currentNode.AppendChild(document.CreateWhitespace(ConvertTokenToXmlValue(reader)));
					return;
				}
				if (propertyName == "#significant-whitespace")
				{
					currentNode.AppendChild(document.CreateSignificantWhitespace(ConvertTokenToXmlValue(reader)));
					return;
				}
				if (!string.IsNullOrEmpty(propertyName) && propertyName[0] == '?')
				{
					CreateInstruction(reader, document, currentNode, propertyName);
					return;
				}
				if (string.Equals(propertyName, "!DOCTYPE", StringComparison.OrdinalIgnoreCase))
				{
					CreateDocumentType(reader, document, currentNode);
					return;
				}
			}
			if (reader.TokenType == JsonToken.StartArray)
			{
				ReadArrayElements(reader, document, propertyName, currentNode, manager);
			}
			else
			{
				ReadElement(reader, document, currentNode, propertyName, manager);
			}
		}

		private void ReadElement(JsonReader reader, IXmlDocument document, IXmlNode currentNode, string propertyName, XmlNamespaceManager manager)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				throw JsonSerializationException.Create(reader, "XmlNodeConverter cannot convert JSON with an empty property name to XML.");
			}
			Dictionary<string, string> attributeNameValues = null;
			string elementPrefix = null;
			if (!EncodeSpecialCharacters)
			{
				attributeNameValues = (ShouldReadInto(reader) ? ReadAttributeElements(reader, manager) : null);
				elementPrefix = MiscellaneousUtils.GetPrefix(propertyName);
				if (propertyName.StartsWith('@'))
				{
					string text = propertyName.Substring(1);
					string prefix = MiscellaneousUtils.GetPrefix(text);
					AddAttribute(reader, document, currentNode, propertyName, text, manager, prefix);
					return;
				}
				if (propertyName.StartsWith('$'))
				{
					if (propertyName == "$values")
					{
						propertyName = propertyName.Substring(1);
						elementPrefix = manager.LookupPrefix("http://james.newtonking.com/projects/json");
						CreateElement(reader, document, currentNode, propertyName, manager, elementPrefix, attributeNameValues);
						return;
					}
					if (propertyName == "$id" || propertyName == "$ref" || propertyName == "$type" || propertyName == "$value")
					{
						string attributeName = propertyName.Substring(1);
						string attributePrefix = manager.LookupPrefix("http://james.newtonking.com/projects/json");
						AddAttribute(reader, document, currentNode, propertyName, attributeName, manager, attributePrefix);
						return;
					}
				}
			}
			else if (ShouldReadInto(reader))
			{
				reader.ReadAndAssert();
			}
			CreateElement(reader, document, currentNode, propertyName, manager, elementPrefix, attributeNameValues);
		}

		private void CreateElement(JsonReader reader, IXmlDocument document, IXmlNode currentNode, string elementName, XmlNamespaceManager manager, string elementPrefix, Dictionary<string, string> attributeNameValues)
		{
			IXmlElement xmlElement = CreateElement(elementName, document, elementPrefix, manager);
			currentNode.AppendChild(xmlElement);
			if (attributeNameValues != null)
			{
				foreach (KeyValuePair<string, string> attributeNameValue in attributeNameValues)
				{
					string text = XmlConvert.EncodeName(attributeNameValue.Key);
					string prefix = MiscellaneousUtils.GetPrefix(attributeNameValue.Key);
					IXmlNode attributeNode = (!string.IsNullOrEmpty(prefix)) ? document.CreateAttribute(text, manager.LookupNamespace(prefix) ?? string.Empty, attributeNameValue.Value) : document.CreateAttribute(text, attributeNameValue.Value);
					xmlElement.SetAttributeNode(attributeNode);
				}
			}
			switch (reader.TokenType)
			{
			case JsonToken.Null:
				break;
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Date:
			case JsonToken.Bytes:
			{
				string text2 = ConvertTokenToXmlValue(reader);
				if (text2 != null)
				{
					xmlElement.AppendChild(document.CreateTextNode(text2));
				}
				break;
			}
			case JsonToken.EndObject:
				manager.RemoveNamespace(string.Empty, manager.DefaultNamespace);
				break;
			default:
				manager.PushScope();
				DeserializeNode(reader, document, manager, xmlElement);
				manager.PopScope();
				manager.RemoveNamespace(string.Empty, manager.DefaultNamespace);
				break;
			}
		}

		private static void AddAttribute(JsonReader reader, IXmlDocument document, IXmlNode currentNode, string propertyName, string attributeName, XmlNamespaceManager manager, string attributePrefix)
		{
			if (currentNode.NodeType == XmlNodeType.Document)
			{
				throw JsonSerializationException.Create(reader, "JSON root object has property '{0}' that will be converted to an attribute. A root object cannot have any attribute properties. Consider specifying a DeserializeRootElementName.".FormatWith(CultureInfo.InvariantCulture, propertyName));
			}
			string text = XmlConvert.EncodeName(attributeName);
			string value = ConvertTokenToXmlValue(reader);
			IXmlNode attributeNode = (!string.IsNullOrEmpty(attributePrefix)) ? document.CreateAttribute(text, manager.LookupNamespace(attributePrefix), value) : document.CreateAttribute(text, value);
			((IXmlElement)currentNode).SetAttributeNode(attributeNode);
		}

		private static string ConvertTokenToXmlValue(JsonReader reader)
		{
			switch (reader.TokenType)
			{
			case JsonToken.String:
				return reader.Value?.ToString();
			case JsonToken.Integer:
			{
				object value;
				if ((value = reader.Value) is BigInteger)
				{
					return ((BigInteger)value).ToString(CultureInfo.InvariantCulture);
				}
				return XmlConvert.ToString(Convert.ToInt64(reader.Value, CultureInfo.InvariantCulture));
			}
			case JsonToken.Float:
			{
				object value;
				if ((value = reader.Value) is decimal)
				{
					decimal value4 = (decimal)value;
					return XmlConvert.ToString(value4);
				}
				if ((value = reader.Value) is float)
				{
					float value5 = (float)value;
					return XmlConvert.ToString(value5);
				}
				return XmlConvert.ToString(Convert.ToDouble(reader.Value, CultureInfo.InvariantCulture));
			}
			case JsonToken.Boolean:
				return XmlConvert.ToString(Convert.ToBoolean(reader.Value, CultureInfo.InvariantCulture));
			case JsonToken.Date:
			{
				object value;
				if ((value = reader.Value) is DateTimeOffset)
				{
					DateTimeOffset value2 = (DateTimeOffset)value;
					return XmlConvert.ToString(value2);
				}
				DateTime value3 = Convert.ToDateTime(reader.Value, CultureInfo.InvariantCulture);
				return XmlConvert.ToString(value3, DateTimeUtils.ToSerializationMode(value3.Kind));
			}
			case JsonToken.Bytes:
				return Convert.ToBase64String((byte[])reader.Value);
			case JsonToken.Null:
				return null;
			default:
				throw JsonSerializationException.Create(reader, "Cannot get an XML string value from token type '{0}'.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
		}

		private void ReadArrayElements(JsonReader reader, IXmlDocument document, string propertyName, IXmlNode currentNode, XmlNamespaceManager manager)
		{
			string prefix = MiscellaneousUtils.GetPrefix(propertyName);
			IXmlElement xmlElement = CreateElement(propertyName, document, prefix, manager);
			currentNode.AppendChild(xmlElement);
			int num = 0;
			while (reader.Read() && reader.TokenType != JsonToken.EndArray)
			{
				DeserializeValue(reader, document, manager, propertyName, xmlElement);
				num++;
			}
			if (WriteArrayAttribute)
			{
				AddJsonArrayAttribute(xmlElement, document);
			}
			if (num == 1 && WriteArrayAttribute)
			{
				foreach (IXmlNode childNode in xmlElement.ChildNodes)
				{
					IXmlElement xmlElement2;
					if ((xmlElement2 = (childNode as IXmlElement)) != null && xmlElement2.LocalName == propertyName)
					{
						AddJsonArrayAttribute(xmlElement2, document);
						break;
					}
				}
			}
		}

		private void AddJsonArrayAttribute(IXmlElement element, IXmlDocument document)
		{
			element.SetAttributeNode(document.CreateAttribute("json:Array", "http://james.newtonking.com/projects/json", "true"));
			if (element is XElementWrapper && element.GetPrefixOfNamespace("http://james.newtonking.com/projects/json") == null)
			{
				element.SetAttributeNode(document.CreateAttribute("xmlns:json", "http://www.w3.org/2000/xmlns/", "http://james.newtonking.com/projects/json"));
			}
		}

		private bool ShouldReadInto(JsonReader reader)
		{
			switch (reader.TokenType)
			{
			case JsonToken.StartConstructor:
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Null:
			case JsonToken.Date:
			case JsonToken.Bytes:
				return false;
			default:
				return true;
			}
		}

		private Dictionary<string, string> ReadAttributeElements(JsonReader reader, XmlNamespaceManager manager)
		{
			Dictionary<string, string> dictionary = null;
			bool flag = false;
			while (!flag && reader.Read())
			{
				switch (reader.TokenType)
				{
				case JsonToken.PropertyName:
				{
					string text = reader.Value.ToString();
					if (!string.IsNullOrEmpty(text))
					{
						switch (text[0])
						{
						case '@':
						{
							if (dictionary == null)
							{
								dictionary = new Dictionary<string, string>();
							}
							text = text.Substring(1);
							reader.ReadAndAssert();
							string value = ConvertTokenToXmlValue(reader);
							dictionary.Add(text, value);
							if (IsNamespaceAttribute(text, out string prefix))
							{
								manager.AddNamespace(prefix, value);
							}
							break;
						}
						case '$':
							if (text == "$values" || text == "$id" || text == "$ref" || text == "$type" || text == "$value")
							{
								string text2 = manager.LookupPrefix("http://james.newtonking.com/projects/json");
								if (text2 == null)
								{
									if (dictionary == null)
									{
										dictionary = new Dictionary<string, string>();
									}
									int? num = null;
									while (manager.LookupNamespace("json" + num) != null)
									{
										num = num.GetValueOrDefault() + 1;
									}
									text2 = "json" + num;
									dictionary.Add("xmlns:" + text2, "http://james.newtonking.com/projects/json");
									manager.AddNamespace(text2, "http://james.newtonking.com/projects/json");
								}
								if (text == "$values")
								{
									flag = true;
								}
								else
								{
									text = text.Substring(1);
									reader.ReadAndAssert();
									if (!JsonTokenUtils.IsPrimitiveToken(reader.TokenType))
									{
										throw JsonSerializationException.Create(reader, "Unexpected JsonToken: " + reader.TokenType);
									}
									if (dictionary == null)
									{
										dictionary = new Dictionary<string, string>();
									}
									string value = reader.Value?.ToString();
									dictionary.Add(text2 + ":" + text, value);
								}
							}
							else
							{
								flag = true;
							}
							break;
						default:
							flag = true;
							break;
						}
					}
					else
					{
						flag = true;
					}
					break;
				}
				case JsonToken.Comment:
				case JsonToken.EndObject:
					flag = true;
					break;
				default:
					throw JsonSerializationException.Create(reader, "Unexpected JsonToken: " + reader.TokenType);
				}
			}
			return dictionary;
		}

		private void CreateInstruction(JsonReader reader, IXmlDocument document, IXmlNode currentNode, string propertyName)
		{
			if (propertyName == "?xml")
			{
				string version = null;
				string encoding = null;
				string standalone = null;
				while (reader.Read() && reader.TokenType != JsonToken.EndObject)
				{
					switch (reader.Value.ToString())
					{
					case "@version":
						reader.ReadAndAssert();
						version = ConvertTokenToXmlValue(reader);
						break;
					case "@encoding":
						reader.ReadAndAssert();
						encoding = ConvertTokenToXmlValue(reader);
						break;
					case "@standalone":
						reader.ReadAndAssert();
						standalone = ConvertTokenToXmlValue(reader);
						break;
					default:
						throw JsonSerializationException.Create(reader, "Unexpected property name encountered while deserializing XmlDeclaration: " + reader.Value);
					}
				}
				IXmlNode newChild = document.CreateXmlDeclaration(version, encoding, standalone);
				currentNode.AppendChild(newChild);
			}
			else
			{
				IXmlNode newChild2 = document.CreateProcessingInstruction(propertyName.Substring(1), ConvertTokenToXmlValue(reader));
				currentNode.AppendChild(newChild2);
			}
		}

		private void CreateDocumentType(JsonReader reader, IXmlDocument document, IXmlNode currentNode)
		{
			string name = null;
			string publicId = null;
			string systemId = null;
			string internalSubset = null;
			while (reader.Read() && reader.TokenType != JsonToken.EndObject)
			{
				switch (reader.Value.ToString())
				{
				case "@name":
					reader.ReadAndAssert();
					name = ConvertTokenToXmlValue(reader);
					break;
				case "@public":
					reader.ReadAndAssert();
					publicId = ConvertTokenToXmlValue(reader);
					break;
				case "@system":
					reader.ReadAndAssert();
					systemId = ConvertTokenToXmlValue(reader);
					break;
				case "@internalSubset":
					reader.ReadAndAssert();
					internalSubset = ConvertTokenToXmlValue(reader);
					break;
				default:
					throw JsonSerializationException.Create(reader, "Unexpected property name encountered while deserializing XmlDeclaration: " + reader.Value);
				}
			}
			IXmlNode newChild = document.CreateXmlDocumentType(name, publicId, systemId, internalSubset);
			currentNode.AppendChild(newChild);
		}

		private IXmlElement CreateElement(string elementName, IXmlDocument document, string elementPrefix, XmlNamespaceManager manager)
		{
			string text = EncodeSpecialCharacters ? XmlConvert.EncodeLocalName(elementName) : XmlConvert.EncodeName(elementName);
			string text2 = string.IsNullOrEmpty(elementPrefix) ? manager.DefaultNamespace : manager.LookupNamespace(elementPrefix);
			if (string.IsNullOrEmpty(text2))
			{
				return document.CreateElement(text);
			}
			return document.CreateElement(text, text2);
		}

		private void DeserializeNode(JsonReader reader, IXmlDocument document, XmlNamespaceManager manager, IXmlNode currentNode)
		{
			do
			{
				JsonToken tokenType = reader.TokenType;
				switch (tokenType)
				{
				default:
					if ((uint)(tokenType - 13) <= 1u)
					{
						return;
					}
					throw JsonSerializationException.Create(reader, "Unexpected JsonToken when deserializing node: " + reader.TokenType);
				case JsonToken.PropertyName:
				{
					if (currentNode.NodeType == XmlNodeType.Document && document.DocumentElement != null)
					{
						throw JsonSerializationException.Create(reader, "JSON root object has multiple properties. The root object must have a single property in order to create a valid XML document. Consider specifying a DeserializeRootElementName.");
					}
					string text = reader.Value.ToString();
					reader.ReadAndAssert();
					if (reader.TokenType == JsonToken.StartArray)
					{
						int num = 0;
						while (reader.Read() && reader.TokenType != JsonToken.EndArray)
						{
							DeserializeValue(reader, document, manager, text, currentNode);
							num++;
						}
						if (num == 1 && WriteArrayAttribute)
						{
							MiscellaneousUtils.GetQualifiedNameParts(text, out string prefix, out string localName);
							string b = string.IsNullOrEmpty(prefix) ? manager.DefaultNamespace : manager.LookupNamespace(prefix);
							foreach (IXmlNode childNode in currentNode.ChildNodes)
							{
								IXmlElement xmlElement;
								if ((xmlElement = (childNode as IXmlElement)) != null && xmlElement.LocalName == localName && xmlElement.NamespaceUri == b)
								{
									AddJsonArrayAttribute(xmlElement, document);
									break;
								}
							}
						}
					}
					else
					{
						DeserializeValue(reader, document, manager, text, currentNode);
					}
					break;
				}
				case JsonToken.StartConstructor:
				{
					string propertyName = reader.Value.ToString();
					while (reader.Read() && reader.TokenType != JsonToken.EndConstructor)
					{
						DeserializeValue(reader, document, manager, propertyName, currentNode);
					}
					break;
				}
				case JsonToken.Comment:
					currentNode.AppendChild(document.CreateComment((string)reader.Value));
					break;
				}
			}
			while (reader.Read());
		}

		private bool IsNamespaceAttribute(string attributeName, out string prefix)
		{
			if (attributeName.StartsWith("xmlns", StringComparison.Ordinal))
			{
				if (attributeName.Length == 5)
				{
					prefix = string.Empty;
					return true;
				}
				if (attributeName[5] == ':')
				{
					prefix = attributeName.Substring(6, attributeName.Length - 6);
					return true;
				}
			}
			prefix = null;
			return false;
		}

		private bool ValueAttributes(List<IXmlNode> c)
		{
			foreach (IXmlNode item in c)
			{
				if (!(item.NamespaceUri == "http://james.newtonking.com/projects/json") && (!(item.NamespaceUri == "http://www.w3.org/2000/xmlns/") || !(item.Value == "http://james.newtonking.com/projects/json")))
				{
					return true;
				}
			}
			return false;
		}

		public override bool CanConvert(Type valueType)
		{
			if (valueType.AssignableToTypeName("System.Xml.Linq.XObject", searchInterfaces: false))
			{
				return IsXObject(valueType);
			}
			if (valueType.AssignableToTypeName("System.Xml.XmlNode", searchInterfaces: false))
			{
				return IsXmlNode(valueType);
			}
			return false;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool IsXObject(Type valueType)
		{
			return typeof(XObject).IsAssignableFrom(valueType);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool IsXmlNode(Type valueType)
		{
			return typeof(XmlNode).IsAssignableFrom(valueType);
		}
	}
}
