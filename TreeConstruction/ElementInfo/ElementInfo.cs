using System;
using System.Xml;

namespace Bakera.RedFace{

	public abstract class ElementInfo{

		public virtual string Name{get; protected set;}
		public abstract string Namespace{get;}


		public virtual bool IsMatch(XmlElement e){
			return e.Name.Equals(this.Name, StringComparison.InvariantCulture) && e.NamespaceURI.Equals(this.Namespace, StringComparison.InvariantCulture);
		}

		public static  bool IsMathMLNameSpace(XmlElement e){
			return e.NamespaceURI.Equals(Document.MathMLNamespace, StringComparison.InvariantCulture);
		}

		public static  bool IsSVGNameSpace(XmlElement e){
			return e.NamespaceURI.Equals(Document.SVGNamespace, StringComparison.InvariantCulture);
		}

		public static  bool IsMathMLTextIntegrationPoint(XmlElement e){
			if(IsMathMLNameSpace(e)){
				if(e.Name.Equals("mi", StringComparison.InvariantCulture)) return true;
				if(e.Name.Equals("mo", StringComparison.InvariantCulture)) return true;
				if(e.Name.Equals("mn", StringComparison.InvariantCulture)) return true;
				if(e.Name.Equals("ms", StringComparison.InvariantCulture)) return true;
				if(e.Name.Equals("mtext", StringComparison.InvariantCulture)) return true;
			}
			return false;
		}

		public static bool IsHtmlIntegrationPoint(XmlElement e){
			if(IsMathMLNameSpace(e)){
				if(e.Name.Equals("annotation-xml", StringComparison.InvariantCulture)){
					string encodingAttrValue = e.GetAttribute("encoding");
					if(encodingAttrValue != null){
						if(encodingAttrValue.Equals("text/html", StringComparison.InvariantCultureIgnoreCase)) return true;
						if(encodingAttrValue.Equals("application/xhtml+xml", StringComparison.InvariantCultureIgnoreCase)) return true;
					}
				}
			} else if(IsSVGNameSpace(e)){
				if(e.Name.Equals("foreignObject", StringComparison.InvariantCulture)) return true;
				if(e.Name.Equals("desc", StringComparison.InvariantCulture)) return true;
				if(e.Name.Equals("title", StringComparison.InvariantCulture)) return true;
			}
			return false;
		}
	}

}



