using System;
using System.Xml;

namespace Bakera.RedFace{

	public partial class Document : XmlDocument{

		public const string HtmlNamespace = "http://www.w3.org/1999/xhtml";
		public const string MathMLNamespace = "http://www.w3.org/1998/Math/MathML";
		public const string SVGNamespace = "http://www.w3.org/2000/svg";
		public const string XLinkNamespace = "http://www.w3.org/1999/xlink";
		public const string XmlNamespace = "http://www.w3.org/XML/1998/namespace";
		public const string XmlnsNamespace = "http://www.w3.org/2000/xmlns/";

		public DoctypeInfo DoctypeInfo{
			get;
			private set;
		}
		public DocumentMode DocumentMode{ get; set; }


// コンストラクタ
// XmlResolver = null にする
		public Document(){
			this.XmlResolver = null;
		}


		public void AppendDoctype(DoctypeToken token){
			this.DoctypeInfo = DoctypeInfo.CreateDoctypeInfo(token);
			XmlNode result = this.CreateDocumentType(token.Name, token.PublicIdentifier, token.SystemIdentifier, null);
			this.AppendChild(result);
			if(this.DocumentMode == DocumentMode.UnKnown){
				this.DocumentMode = this.DoctypeInfo.DocumentMode;
			}
		}

		public XmlElement CreateHtmlElement(string name){
			XmlElement result = base.CreateElement(name, HtmlNamespace);
			return result;
		}


		// 渡されたXmlElementが同じ名前、名前空間、属性を持っていればtrueを返します。
		// same tag name, namespace, and attributes as element
		public static bool IsSamePairElement(XmlElement e1, XmlElement e2){
			if(e1 == null) return e2 == null;

			if(e1.Name != e2.Name) return false;
			if(e1.Attributes.Count != e2.Attributes.Count) return false;
			foreach(XmlAttribute attr1 in e1.Attributes){
				XmlAttribute attr2 = e2.Attributes[attr1.Name, attr1.NamespaceURI];
				if(attr2 == null) return false;
				if(attr1.Value != attr2.Value) return false;
			}
			return true;
		}


	}

}



