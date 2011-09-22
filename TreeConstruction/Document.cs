using System;
using System.Xml;

namespace Bakera.RedFace{

	public partial class Document : XmlDocument{

		public DocumentMode DocumentMode{get; set;}


// コンストラクタ
// XmlResolver = null にする
		public Document(){
			this.XmlResolver = null;
		}

		public void AppendComment(CommentToken token){
			XmlNode result = this.CreateComment(token.Data);
			this.AppendChild(result);
		}

	}

}



