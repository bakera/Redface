using System;
using System.Xml;

namespace Bakera.RedFace{


	public class BeforeHeadInsertionMode : InsertionMode{

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
				tree.AppendToken<InBodyInsertionMode>(token);
				return;
			}

			if(token.IsStartTag("head")){
				XmlElement headElement = tree.InsertElementForToken((StartTagToken)token);
				tree.HeadElementPointer = headElement;
				tree.ChangeInsertionMode<InHeadInsertionMode>();
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
			AppendStartTagToken(tree, new FakeStartTagToken(){Name = "head"});
			tree.ReprocessFlag = true;
			return;
		}

	}
}
