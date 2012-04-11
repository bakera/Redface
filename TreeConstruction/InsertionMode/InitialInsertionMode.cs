using System;

namespace Bakera.RedFace{

	public class InitialInsertionMode : InsertionMode{

		public override void AppendDoctypeToken(TreeConstruction tree, DoctypeToken token){
			if(string.IsNullOrEmpty(token.Name)){
				OnMessageRaised(new NamelessDoctypeFailure());
			} else {
				tree.Document.AppendDoctype((DoctypeToken)token);
				// NoQuirks以外の文書型はパースエラー
				// UnKnownDoctypeの場合はNoQuirksでパースエラーになることに注意
				if(tree.Document.DoctypeInfo is UnKnownDoctype){
					OnMessageRaised(new UnknownDoctypeError());
				} else if(tree.Document.DoctypeInfo is QuirksDoctype){
					OnMessageRaised(new QuirksDoctypeError());
				} else if(tree.Document.DoctypeInfo is LimitedQuirksDoctype){
					OnMessageRaised(new LimitedQuirksDoctypeError());
				}
			}
			tree.ChangeInsertionMode<BeforeHtmlInsertionMode>();
			return;
		}

		public override void AppendCharacterToken(TreeConstruction tree, CharacterToken token){
			if(token.IsSpaceCharacter) return;
			AppendAnythingElse(tree, token);
		}

		public override void AppendCommentToken(TreeConstruction tree, CommentToken token){
			tree.AppendCommentForToken(token);
		}

		public override void AppendAnythingElse(TreeConstruction tree, Token token){
			// 文書型宣言以外が出現
			OnMessageRaised(new NoDoctypeError());
			tree.Document.DocumentMode = DocumentMode.Quirks;
			tree.ChangeInsertionMode<BeforeHtmlInsertionMode>();
			tree.ReprocessFlag = true;
		}

	}
}
