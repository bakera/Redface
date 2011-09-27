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

				if(token.IsStartTag("frameset")){
					tree.InsertElementForToken((TagToken)token);
					tree.ChangeInsertionMode<InFramesetInsertionMode>();
					return;
				}

				if(token.IsStartTag("base", "basefont", "bgsound", "link", "meta", "noframes", "script", "style", "title")){
					tree.Parser.OnParseErrorRaised(string.Format("head要素以外の箇所に要素があります。: {0}", token.Name));

					tree.PutToStack(tree.HeadElementPointer);
					tree.AppendToken<InHeadInsertionMode>(token);

//Process the token using the rules for the "in head" insertion mode.


					return;
				}









				Console.WriteLine("========\nnot implemented: {0} - {1}", this.Name, token);
				tree.Parser.Stop();
				return;
			}

		}
	}
}
