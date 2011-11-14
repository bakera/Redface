using System;

namespace Bakera.RedFace{

	public class InitialInsertionMode : InsertionMode{

		public override void AppendDoctypeToken(TreeConstruction tree, DoctypeToken token){
			if(string.IsNullOrEmpty(token.Name)){
				// ToDo: システム都合のエラーなので扱いをちょっと何とかする
				OnParseErrorRaised(string.Format("名前のない文書型宣言を扱うことはできません。"));
			} else {
				tree.Document.AppendDoctype((DoctypeToken)token);
				// NoQuirks以外の文書型はパースエラー
				// UnKnownDoctypeの場合はNoQuirksでパースエラーになることに注意
				if(tree.Document.DoctypeInfo is UnKnownDoctype){
					OnParseErrorRaised(string.Format("未知の文書型宣言です。"));
				} else if(tree.Document.DoctypeInfo is QuirksDoctype){
					OnParseErrorRaised(string.Format("Quirksモードとなる文書型宣言が指定されています。"));
				} else if(tree.Document.DoctypeInfo is LimitedQuirksDoctype){
					OnParseErrorRaised(string.Format("Limited Quirksモードとなる文書型宣言が指定されています。"));
				}
			}
			tree.ChangeInsertionMode<BeforeHtmlInsertionMode>();
			OnMessageRaised(EventLevel.Information, string.Format("ドキュメントモードが変更されました。: {0}", tree.Document.DocumentMode));
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
			OnParseErrorRaised(string.Format("文書型宣言がありません。"));
			tree.Document.DocumentMode = DocumentMode.Quirks;
			OnMessageRaised(EventLevel.Information, string.Format("ドキュメントモードが変更されました。: {0}", tree.Document.DocumentMode));
			tree.ChangeInsertionMode<BeforeHtmlInsertionMode>();
			tree.ReprocessFlag = true;
		}

	}
}
