using System.Collections.Generic;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	internal class XContainerWrapper : XObjectWrapper
	{
		private List<IXmlNode> _childNodes;

		private XContainer Container => (XContainer)base.WrappedNode;

		public override List<IXmlNode> ChildNodes
		{
			get
			{
				if (_childNodes == null)
				{
					if (!HasChildNodes)
					{
						_childNodes = XmlNodeConverter.EmptyChildNodes;
					}
					else
					{
						_childNodes = new List<IXmlNode>();
						foreach (XNode item in Container.Nodes())
						{
							_childNodes.Add(WrapNode(item));
						}
					}
				}
				return _childNodes;
			}
		}

		protected virtual bool HasChildNodes => Container.LastNode != null;

		public override IXmlNode ParentNode
		{
			get
			{
				if (Container.Parent == null)
				{
					return null;
				}
				return WrapNode(Container.Parent);
			}
		}

		public XContainerWrapper(XContainer container)
			: base(container)
		{
		}

		internal static IXmlNode WrapNode(XObject node)
		{
			XDocument document;
			if ((document = (node as XDocument)) != null)
			{
				return new XDocumentWrapper(document);
			}
			XElement element;
			if ((element = (node as XElement)) != null)
			{
				return new XElementWrapper(element);
			}
			XContainer container;
			if ((container = (node as XContainer)) != null)
			{
				return new XContainerWrapper(container);
			}
			XProcessingInstruction processingInstruction;
			if ((processingInstruction = (node as XProcessingInstruction)) != null)
			{
				return new XProcessingInstructionWrapper(processingInstruction);
			}
			XText text;
			if ((text = (node as XText)) != null)
			{
				return new XTextWrapper(text);
			}
			XComment text2;
			if ((text2 = (node as XComment)) != null)
			{
				return new XCommentWrapper(text2);
			}
			XAttribute attribute;
			if ((attribute = (node as XAttribute)) != null)
			{
				return new XAttributeWrapper(attribute);
			}
			XDocumentType documentType;
			if ((documentType = (node as XDocumentType)) != null)
			{
				return new XDocumentTypeWrapper(documentType);
			}
			return new XObjectWrapper(node);
		}

		public override IXmlNode AppendChild(IXmlNode newChild)
		{
			Container.Add(newChild.WrappedNode);
			_childNodes = null;
			return newChild;
		}
	}
}
