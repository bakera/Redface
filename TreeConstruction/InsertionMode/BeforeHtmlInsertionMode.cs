using System;
using System.Xml;

namespace Bakera.RedFace{

	public class BeforeHtmlInsertionMode : InsertionMode{

		public override void AppendToken(TreeConstruction tree, Token token){

			if(token is DoctypeToken){
				OnParseErrorRaised(string.Format("先頭以外の箇所に文書型宣言があります。"));
				return;
			}

			if(token is CommentToken){
				tree.AppendCommentForToken((CommentToken)token);
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
				OnParseErrorRaised(string.Format("不明な終了タグがあります。"));
				return;
			}

			XmlElement defaultHtmlElement = tree.Document.CreateHtmlElement("html");
			tree.AppendChild(defaultHtmlElement);
			tree.PutToStack(defaultHtmlElement);
			tree.ChangeInsertionMode<BeforeHeadInsertionMode>();
			tree.ReprocessFlag = true;
			return;
		}

	}
}
