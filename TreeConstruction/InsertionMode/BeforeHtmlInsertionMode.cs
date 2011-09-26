using System;
using System.Reflection;
using System.Xml;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class BeforeHtmlInsertionMode : InsertionMode{

			public override void AppendToken(TreeConstruction tree, Token token){

				if(token is DoctypeToken){
					tree.Parser.OnParseErrorRaised(string.Format("先頭以外の箇所に文書型宣言があります。"));
					return;
				}

				if(token is CommentToken){
					tree.Document.AppendComment((CommentToken)token);
					return;
				}

				if(token.IsWhiteSpace){
					return;
				}

				if(token.IsStartTag("html")){
					XmlElement htmlElement = tree.CreateElementForToken((TagToken)token);
					tree.AppendChild(htmlElement);
					tree.PutToStack(htmlElement);
					tree.ChangeInsertionMode<BeforeHeadInsertionMode>();
					return;
				}

				if(token is EndTagToken && !token.IsEndTag("head", "body", "html", "br")){
					tree.Parser.OnParseErrorRaised(string.Format("不明な終了タグがあります。"));
					return;
				}

				XmlElement defaultHtmlElement = tree.Document.CreateElement("html");
				tree.AppendChild(defaultHtmlElement);
				tree.PutToStack(defaultHtmlElement);
				tree.ChangeInsertionMode<BeforeHeadInsertionMode>();
				tree.ReprocessFlag = true;
				return;
			}

		}
	}
}
