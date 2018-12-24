using System.Collections.Generic;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	internal class XmlNodeWrapper : IXmlNode
	{
		private readonly XmlNode _node;

		private List<IXmlNode> _childNodes;

		private List<IXmlNode> _attributes;

		public object WrappedNode => _node;

		public XmlNodeType NodeType => _node.NodeType;

		public virtual string LocalName => _node.LocalName;

		public List<IXmlNode> ChildNodes
		{
			get
			{
				if (_childNodes == null)
				{
					if (!_node.HasChildNodes)
					{
						_childNodes = XmlNodeConverter.EmptyChildNodes;
					}
					else
					{
						_childNodes = new List<IXmlNode>(_node.ChildNodes.Count);
						foreach (XmlNode childNode in _node.ChildNodes)
						{
							_childNodes.Add(WrapNode(childNode));
						}
					}
				}
				return _childNodes;
			}
		}

		protected virtual bool HasChildNodes => _node.HasChildNodes;

		public List<IXmlNode> Attributes
		{
			get
			{
				if (_attributes == null)
				{
					if (!HasAttributes)
					{
						_attributes = XmlNodeConverter.EmptyChildNodes;
					}
					else
					{
						_attributes = new List<IXmlNode>(_node.Attributes.Count);
						foreach (XmlAttribute attribute in _node.Attributes)
						{
							_attributes.Add(WrapNode(attribute));
						}
					}
				}
				return _attributes;
			}
		}

		private bool HasAttributes
		{
			get
			{
				XmlElement xmlElement;
				if ((xmlElement = (_node as XmlElement)) != null)
				{
					return xmlElement.HasAttributes;
				}
				XmlAttributeCollection attributes = _node.Attributes;
				if (attributes == null)
				{
					return false;
				}
				return attributes.Count > 0;
			}
		}

		public IXmlNode ParentNode
		{
			get
			{
				XmlAttribute xmlAttribute;
				XmlNode xmlNode = ((xmlAttribute = (_node as XmlAttribute)) != null) ? xmlAttribute.OwnerElement : _node.ParentNode;
				if (xmlNode == null)
				{
					return null;
				}
				return WrapNode(xmlNode);
			}
		}

		public string Value
		{
			get
			{
				return _node.Value;
			}
			set
			{
				_node.Value = value;
			}
		}

		public string NamespaceUri => _node.NamespaceURI;

		public XmlNodeWrapper(XmlNode node)
		{
			_node = node;
		}

		internal static IXmlNode WrapNode(XmlNode node)
		{
			switch (node.NodeType)
			{
			case XmlNodeType.Element:
				return new XmlElementWrapper((XmlElement)node);
			case XmlNodeType.XmlDeclaration:
				return new XmlDeclarationWrapper((XmlDeclaration)node);
			case XmlNodeType.DocumentType:
				return new XmlDocumentTypeWrapper((XmlDocumentType)node);
			default:
				return new XmlNodeWrapper(node);
			}
		}

		public IXmlNode AppendChild(IXmlNode newChild)
		{
			XmlNodeWrapper xmlNodeWrapper = (XmlNodeWrapper)newChild;
			_node.AppendChild(xmlNodeWrapper._node);
			_childNodes = null;
			_attributes = null;
			return newChild;
		}
	}
}
