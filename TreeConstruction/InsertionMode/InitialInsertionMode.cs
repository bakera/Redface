using System;

namespace Bakera.RedFace{

	public class InitialInsertionMode : InsertionMode{

		public override void AppendToken(TreeConstruction tree, Token token){

			if(token.IsWhiteSpace){
				return;
			}

			if(token is CommentToken){
				tree.AppendCommentForToken((CommentToken)token);
				return;
			}

			if(token is DoctypeToken){
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
				OnDocumentModeChanged();
				return;
			}

			// 文書型宣言以外が出現
			// ToDo:iframe srcdoc documentに対応する?
			OnParseErrorRaised(string.Format("文書型宣言がありません。"));
			tree.Document.DocumentMode = DocumentMode.Quirks;
			OnDocumentModeChanged();
			tree.ChangeInsertionMode<BeforeHtmlInsertionMode>();
			tree.ReprocessFlag = true;
			return;
		}

	}
}
