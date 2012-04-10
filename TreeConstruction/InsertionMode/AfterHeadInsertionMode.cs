using System;
using System.Xml;

namespace Bakera.RedFace{


	public class AfterHeadInsertionMode : InsertionMode{


		public override void AppendDoctypeToken(TreeConstruction tree, DoctypeToken token){
			OnMessageRaised(new UnexpectedDoctypeError());
		}

		public override void AppendCharacterToken(TreeConstruction tree, CharacterToken token){
			if(token.IsSpaceCharacter){
				tree.AppendToken<InHeadInsertionMode>(token);
				return;
			}
			AppendAnythingElse(tree, token);
		}

		public override void AppendCommentToken(TreeConstruction tree, CommentToken token){
			tree.AppendCommentForToken(token);
		}

		public override void AppendStartTagToken(TreeConstruction tree, StartTagToken token){
			if(token.IsStartTag("html")){
				tree.AppendToken<InBodyInsertionMode>(token);
				return;
			}

			if(token.IsStartTag("body")){
				tree.InsertElementForToken(token);
				tree.Parser.FramesetOK = false;
				tree.ChangeInsertionMode<InBodyInsertionMode>();
				return;
			}

			if(token.IsStartTag("frameset")){
				tree.InsertElementForToken(token);
				tree.ChangeInsertionMode<InFramesetInsertionMode>();
				return;
			}

			if(token.IsStartTag("base", "basefont", "bgsound", "link", "meta", "noframes", "script", "style", "title")){
				OnMessageRaised(new UnexpectedInHeadElementError(token.Name));
				tree.PutToStack(tree.HeadElementPointer);
				tree.AppendToken<InHeadInsertionMode>(token);
				tree.PopFromStack();
				return;
			}
			if(token.IsStartTag("head")){
				OnMessageRaised(new MultipleHeadElementError());
				return;
			}

			AppendAnythingElse(tree, token);
		}


		public override void AppendEndTagToken(TreeConstruction tree, EndTagToken token){
			if(token.IsEndTag("body", "html", "br")){
				AppendAnythingElse(tree, token);
				return;
			}
			OnMessageRaised(new UnexpectedEndTagError(token.Name));
			return;
		}

		public override void AppendAnythingElse(TreeConstruction tree, Token token){
			AppendStartTagToken(tree, new FakeStartTagToken(){Name = "body"});
			tree.Parser.FramesetOK = true;
			tree.ReprocessFlag = true;
			return;
		}
	}
}
