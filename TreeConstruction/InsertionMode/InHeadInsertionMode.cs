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
					GenericRCDATAElementParsingAlgorithm(tree, token);
					return;
				}

				// RedFaceParserは常にScriptingDisabled
				if(token.IsStartTag("noframes", "style")){
					GenericRawtextElementParsingAlgorithm(tree, token);
					return;
				}

				// RedFaceParserは常にScriptingDisabled
				if(token.IsStartTag("noscript")){
					tree.InsertElementForToken((TagToken)token);
					tree.ChangeInsertionMode<InHeadNoscriptInsertionMode>();
					return;
				}

				if(token.IsStartTag("script")){
					//ToDo:
				}

				if(token.IsEndTag("head")){
					tree.PopFromStack();
					tree.ChangeInsertionMode<AfterHeadInsertionMode>();
					return;
				}

				if(token is EndTagToken && !token.IsEndTag("body", "html", "br")){
					tree.Parser.OnParseErrorRaised(string.Format("不明な終了タグがあります。"));
					return;
				}

				if(token.IsStartTag("head")){
					tree.Parser.OnParseErrorRaised(string.Format("head要素の開始タグが重複しています。"));
					return;
				}

				tree.PopFromStack();
				tree.ChangeInsertionMode<AfterHeadInsertionMode>();
				tree.ReprocessFlag = true;
				return;

			}

		}
	}
}
