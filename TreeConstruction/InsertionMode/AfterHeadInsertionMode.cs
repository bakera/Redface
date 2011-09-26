using System;
using System.Reflection;
using System.Xml;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class AfterHeadInsertionMode : InsertionMode{

			public override void AppendToken(TreeConstruction tree, Token token){

				if(token.IsWhiteSpace){
					tree.InsertCharacter((CharacterToken)token);
					return;
				}

				if(token is CommentToken){
					tree.Document.AppendComment((CommentToken)token);
					return;
				}

				if(token is DoctypeToken){
					tree.Parser.OnParseErrorRaised(string.Format("先頭以外の箇所に文書型宣言があります。"));
					return;
				}

				if(token.IsStartTag("html")){
					// ToDo:
					// Process the token using the rules for the "in body" insertion mode.
					Console.WriteLine("not implemented:" + this.GetType().Name);
					tree.Parser.Stop();
					return;
				}

				if(token.IsStartTag("body")){
					tree.InsertElementForToken((TagToken)token);
					tree.Parser.FramesetOK = false;
					tree.ChangeInsertionMode<InBodyInsertionMode>();
					return;
				}









				Console.WriteLine("========\nnot implemented: {0} - {1}", this.Name, token);
				tree.Parser.Stop();
				return;
			}

		}
	}
}
