using System;
using System.Reflection;
using System.Xml;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class InHeadInsertionMode : InsertionMode{

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

				if(token.IsStartTag("base", "basefont", "bgsound", "command", "link")){
					tree.InsertElementForToken((TagToken)token);
					tree.PopFromStack();
					tree.AcknowledgeSelfClosingFlag((TagToken)token);
					return;
				}

				if(token.IsStartTag("meta")){
					tree.InsertElementForToken((TagToken)token);
					tree.PopFromStack();
					tree.AcknowledgeSelfClosingFlag((TagToken)token);
					// ToDo: process charset
					return;
				}

				if(token.IsStartTag("title")){
					GenericRawtextElementParsingAlgorithm(tree, token);
					return;
				}

				Console.WriteLine("========\nnot implemented: {0} - {1}", this.Name, token);
				tree.Parser.Stop();
			}

		}
	}
}
