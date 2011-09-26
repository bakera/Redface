using System;
using System.Reflection;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class InitialInsertionMode : InsertionMode{

			public override void AppendToken(TreeConstruction tree, Token token){

				if(token.IsWhiteSpace){
					return;
				}

				if(token is CommentToken){
					tree.Document.AppendComment((CommentToken)token);
					return;
				}

				if(token is DoctypeToken){
					tree.Document.AppendDoctype((DoctypeToken)token);
					// NoQuirks以外の文書型はパースエラー
					// UnKnownDoctypeの場合はNoQuirksでパースエラーになることに注意
					if(tree.Document.DoctypeInfo is UnKnownDoctype){
						tree.Parser.OnParseErrorRaised(string.Format("未知の文書型宣言です。"));
					} else if(tree.Document.DoctypeInfo is QuirksDoctype){
						tree.Parser.OnParseErrorRaised(string.Format("Quirksモードとなる文書型宣言が指定されています。"));
					} else if(tree.Document.DoctypeInfo is LimitedQuirksDoctype){
						tree.Parser.OnParseErrorRaised(string.Format("Limited Quirksモードとなる文書型宣言が指定されています。"));
					}
					tree.ChangeInsertionMode<BeforeHtmlInsertionMode>();
					tree.Parser.OnDocumentModeChanged();
					return;
				}

				// 文書型宣言以外が出現
				// ToDo:iframe srcdoc documentに対応する?
				tree.Parser.OnParseErrorRaised(string.Format("文書型宣言がありません。"));
				tree.Document.DocumentMode = DocumentMode.Quirks;
				tree.Parser.OnDocumentModeChanged();
				tree.ChangeInsertionMode<BeforeHtmlInsertionMode>();
				tree.ReprocessFlag = true;
				return;
			}

		}
	}
}
