using System;
using System.Reflection;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class InitialInsertionMode : InsertionMode{

			public override void AppendToken(TreeConstruction tree, Token token){

				if(token.IsWhiteSpace) return;
				if(token is CommentToken){
					tree.Document.AppendComment((CommentToken)token);
					return;
				}

				if(token is DoctypeToken){
					tree.Document.AppendDoctype((DoctypeToken)token);
					
					// NoQuirks以外の文書型はパースエラー
					// UnKnownDoctypeの場合はパースエラーだがNoQuirksになることに注意
					if(tree.Document.DoctypeInfo is UnKnownDoctype){
						tree.Parser.OnParseErrorRaised(string.Format("未知の文書型宣言です。"));
					} else if(tree.Document.DoctypeInfo is QuirksDoctype){
						tree.Parser.OnParseErrorRaised(string.Format("Quirksモードとなる文書型宣言が指定されています。"));
					} else if(tree.Document.DoctypeInfo is LimitedQuirksDoctype){
						tree.Parser.OnParseErrorRaised(string.Format("Limited Quirksモードとなる文書型宣言が指定されています。"));
					}
					tree.ChangeInsertionMode<BeforeHtmlInsertionMode>();
					return;
				}

				// ToDo:iframe srcdoc document.
/*
If the document is not an iframesrcdoc document, then this is a parse error; set the Document to quirks mode.
In any case, switch the insertion mode to "before html", then reprocess the current token.
*/

				
				return;
			}

		}
	}
}
