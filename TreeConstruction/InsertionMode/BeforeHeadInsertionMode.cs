using System;
using System.Xml;

namespace Bakera.RedFace{


	public class BeforeHeadInsertionMode : InsertionMode{

		public override void AppendToken(TreeConstruction tree, Token token){

			if(token.IsWhiteSpace){
				return;
			}

			if(token is CommentToken){
				tree.AppendCommentForToken((CommentToken)token);
				return;
			}

			if(token is DoctypeToken){
				OnParseErrorRaised(string.Format("先頭以外の箇所に文書型宣言があります。"));
				return;
			}

			if(token.IsStartTag("html")){
				tree.AppendToken<InBodyInsertionMode>(token);
				return;
			}

			if(token.IsStartTag("head")){
				XmlElement headElement = tree.InsertElementForToken((StartTagToken)token);
				tree.HeadElementPointer = headElement;
				tree.ChangeInsertionMode<InHeadInsertionMode>();
				return;
			}

			if(token is EndTagToken && !token.IsEndTag("head", "body", "html", "br")){
				OnParseErrorRaised(string.Format("不明な終了タグがあります。"));
				return;
			}

			XmlElement defaultHeadElement = tree.Document.CreateHtmlElement("head");
			tree.InsertElement(defaultHeadElement);
			tree.HeadElementPointer = defaultHeadElement;
			tree.ReprocessFlag = true;
			tree.ChangeInsertionMode<InHeadInsertionMode>();
			return;
		}

	}
}
