using System;
using System.Xml;

namespace Bakera.RedFace{

	public class InColumnGroupInsertionMode : TableRelatedInsertionMode{

		public override void AppendDoctypeToken(TreeConstruction tree, DoctypeToken token){
			OnMessageRaised(new UnexpectedDoctypeError());
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


		public override void AppendStartTagToken(TreeConstruction tree, StartTagToken token){
			switch(token.Name){
			case "html":
				tree.AppendToken<InBodyInsertionMode>(token);
				return;
			case "col":
				tree.InsertElementForToken(token);
				tree.PopFromStack();
				tree.AcknowledgeSelfClosingFlag(token);
				return;
			}
			AppendAnythingElse(tree, token);
		}


		public override void AppendEndTagToken(TreeConstruction tree, EndTagToken token){
			switch(token.Name){
			case "colgroup":
				tree.PopFromStack();
				tree.ChangeInsertionMode<InTableInsertionMode>();
				return;
			case "col":
				OnParseErrorRaised(string.Format("col要素の終了タグが出現しました。"));
				return;
			}
			AppendAnythingElse(tree, token);
		}


		public override void AppendAnythingElse(TreeConstruction tree, Token token){
			AppendEndTagToken(tree, new FakeEndTagToken(){Name = "colgroup"});
			tree.ReprocessFlag = true;
			return;
		}
	}
}
