using System;
using System.Xml;

namespace Bakera.RedFace{

	public partial class Document : XmlDocument{

		public const string HtmlNamespace = "http://www.w3.org/1999/xhtml";
		public const string MathMLNamespace = "http://www.w3.org/1998/Math/MathML";
		public const string SVGNamespace = "http://www.w3.org/2000/svg";
		public const string XLinkNamespace = "http://www.w3.org/1999/xlink";
		public const string XmlNamespace = "http://www.w3.org/XML/1998/namespace";
		public const string XmlnsNnamespace = "http://www.w3.org/2000/xmlns/";

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

		public new XmlElement CreateElement(string name){
			XmlElement result = base.CreateElement(name, HtmlNamespace);
			return result;
		}

	}

}



