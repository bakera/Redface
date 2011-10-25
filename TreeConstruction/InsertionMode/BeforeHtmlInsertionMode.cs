using System;
using System.Xml;

namespace Bakera.RedFace{

	public class BeforeHtmlInsertionMode : InsertionMode{

		protected override void AppendDoctypeToken(TreeConstruction tree, DoctypeToken token){
			OnParseErrorRaised(string.Format("先頭以外の箇所に文書型宣言があります。"));
		}

		protected override void AppendCommentToken(TreeConstruction tree, CommentToken token){
			tree.AppendCommentForToken(token);
		}

		protected override void AppendCharacterToken(TreeConstruction tree, CharacterToken token){
			if(token.IsWhiteSpace) return;
			AppendAnythingElse(tree, token);
		}

		protected override void AppendStartTagToken(TreeConstruction tree, StartTagToken token){
			if(token.IsStartTag("html")){
				XmlElement htmlElement = tree.CreateElementForToken((TagToken)token);
				tree.AppendChild(htmlElement);
				tree.PutToStack(htmlElement);
				tree.ChangeInsertionMode<BeforeHeadInsertionMode>();
				return;
			}
			AppendAnythingElse(tree, token);
		}

		protected override void AppendEndTagToken(TreeConstruction tree, EndTagToken token){
			if(token.IsEndTag("head", "body", "html", "br")){
				AppendAnythingElse(tree, token);
				return;
			}
			OnParseErrorRaised(string.Format("不明な終了タグがあります。"));
			return;
		}

		protected override void AppendAnythingElse(TreeConstruction tree, Token token){
			XmlElement defaultHtmlElement = tree.Document.CreateHtmlElement("html");
			tree.AppendChild(defaultHtmlElement);
			tree.PutToStack(defaultHtmlElement);
			tree.ChangeInsertionMode<BeforeHeadInsertionMode>();
			tree.ReprocessFlag = true;
		}

	}
}
