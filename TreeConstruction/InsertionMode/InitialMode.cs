using System;
using System.Reflection;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class InitialMode : InsertionMode{

			public override void AppendToken(TreeConstruction tree, Token token){

				if(token.IsWhiteSpace) return;
				if(token is CommentToken){
					tree.Document.AppendComment((CommentToken)token);
					return;
				}

				if(token is DoctypeToken){
/*
If the DOCTYPE token's name is not a case-sensitive match for the string "html",
 or the token's public identifier is not missing,
 or the token's system identifier is neither missing nor a case-sensitive match for the string "about:legacy-compat",
 and none of the sets of conditions in the following list are matched, then there is a parse error.
*/

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
