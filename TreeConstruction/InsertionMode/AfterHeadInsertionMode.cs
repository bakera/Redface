using System;
using System.Xml;

namespace Bakera.RedFace{


	public class AfterHeadInsertionMode : InsertionMode{

		public override void AppendToken(TreeConstruction tree, Token token){

			if(token.IsWhiteSpace){
				tree.InsertCharacter((CharacterToken)token);
				return;
			}

			if(token is CommentToken){
				tree.AppendCommentForToken((CommentToken)token);
				return;
			}

			if(token is DoctypeToken){
				OnParseErrorRaised(string.Format("先頭以外の箇所に文書型宣言があります。"));
				return;
			}

			if(token.IsStartTag("html")){
				tree.AppendToken<InBodyInsertionMode>(token);
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
				OnParseErrorRaised(string.Format("head要素以外の箇所に要素があります。: {0}", token.Name));

				tree.PutToStack(tree.HeadElementPointer);
				tree.AppendToken<InHeadInsertionMode>(token);
				tree.PopFromStack();

				return;
			}

			if(token.IsEndTag("body", "html", "br")){
				AnythingElse(tree, token);
				return;
			}

			if(token.IsStartTag("head") || token is EndTagToken){
				OnParseErrorRaised(string.Format("この場所には出現できないトークンです。: {0}", token.Name));
				return;
			}

			AnythingElse(tree, token);
			return;
		}

		private void AnythingElse(TreeConstruction tree, Token token){
			XmlElement defaultBodyElement = tree.Document.CreateHtmlElement("body");
			tree.InsertElement(defaultBodyElement);
			tree.Parser.FramesetOK = true;
			tree.ChangeInsertionMode<InBodyInsertionMode>();
			tree.ReprocessFlag = true;
			return;
		}
	}
}
