using System;
using System.Xml;

namespace Bakera.RedFace{


	public class AfterFramesetInsertionMode : InsertionMode{


		public override void AppendDoctypeToken(TreeConstruction tree, DoctypeToken token){
			OnMessageRaised(new UnexpectedDoctypeError());
			return;
		}


		public override void AppendCommentToken(TreeConstruction tree, CommentToken token){
			tree.AppendCommentForToken(token);
		}


		public override void AppendCharacterToken(TreeConstruction tree, CharacterToken token){
			if(token.IsSpaceCharacter){
				tree.InsertCharacter(token);
				return;
			}
			AppendAnythingElse(tree, token);
		}


		public override void AppendEndOfFileToken(TreeConstruction tree, EndOfFileToken token){
			tree.Parser.Stop();
			return;
		}


		public override void AppendStartTagToken(TreeConstruction tree, StartTagToken token){
			switch(token.Name){
			case "html":
				tree.AppendToken<InBodyInsertionMode>(token);
				return;

			case "noframes":
				tree.AppendToken<InHeadInsertionMode>(token);
				return;

			}
			AppendAnythingElse(tree, token);
		}


		public override void AppendEndTagToken(TreeConstruction tree, EndTagToken token){
			switch(token.Name){
			case "html":
				tree.ChangeInsertionMode<AfterAfterFramesetInsertionMode>();
				return;
			}
			AppendAnythingElse(tree, token);
		}


		public override void AppendAnythingElse(TreeConstruction tree, Token token){
			OnMessageRaised(new UnexpectedTokenAfterHtmlError(token.Name));
			return;
		}
	}
}
