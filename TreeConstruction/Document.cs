using System;
using System.Xml;

namespace Bakera.RedFace{

	public partial class Document : XmlDocument{

		public DoctypeInfo DoctypeInfo{get; private set;}
		public DocumentMode DocumentMode{
			get{
				if(this.DoctypeInfo == null) return DocumentMode.UnKnown;
				return this.DoctypeInfo.DocumentMode;
			}
		}


// コンストラクタ
// XmlResolver = null にする
		public Document(){
			this.XmlResolver = null;
		}

		public void AppendComment(CommentToken token){
			XmlNode result = this.CreateComment(token.Data);
			this.AppendChild(result);
		}

		public void AppendDoctype(DoctypeToken token){
			this.DoctypeInfo = DoctypeInfo.CreateDoctypeInfo(token);
			XmlNode result = this.CreateDocumentType(token.Name, token.PublicIdentifier, token.SystemIdentifier, null);
			this.AppendChild(result);
		}


	}

}



