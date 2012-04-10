using System;
using System.Xml;

namespace Bakera.RedFace{

	public class BeforeHtmlInsertionMode : InsertionMode{

		public override void AppendDoctypeToken(TreeConstruction tree, DoctypeToken token){
			OnMessageRaised(new UnexpectedDoctypeError());
		}

		public override void AppendCommentToken(TreeConstruction tree, CommentToken token){
			tree.AppendCommentForToken(token);
		}

		public override void AppendCharacterToken(TreeConstruction tree, CharacterToken token){
			if(token.IsSpaceCharacter) return;
			AppendAnythingElse(tree, token);
		}

		public override void AppendStartTagToken(TreeConstruction tree, StartTagToken token){
			if(token.IsStartTag("html")){
				XmlElement htmlElement = tree.CreateElementForToken((TagToken)token);
				tree.AppendChild(htmlElement);
				tree.PutToStack(htmlElement);
				tree.ChangeInsertionMode<BeforeHeadInsertionMode>();
				return;
			}
			AppendAnythingElse(tree, token);
		}

		public override void AppendEndTagToken(TreeConstruction tree, EndTagToken token){
			if(token.IsEndTag("head", "body", "html", "br")){
				AppendAnythingElse(tree, token);
				return;
			}
			OnMessageRaised(new UnexpectedEndTagError(token.Name));
			return;
		}

		public override void AppendAnythingElse(TreeConstruction tree, Token token){
			XmlElement defaultHtmlElement = tree.Document.CreateHtmlElement("html");
			tree.AppendChild(defaultHtmlElement);
			tree.PutToStack(defaultHtmlElement);
			tree.ChangeInsertionMode<BeforeHeadInsertionMode>();
			tree.ReprocessFlag = true;
		}

	}
}
