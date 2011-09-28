using System;
using System.Reflection;
using System.Xml;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class InBodyInsertionMode : InsertionMode{

			public override void AppendToken(TreeConstruction tree, Token token){

				if(token.IsNULL){
					tree.Parser.OnParseErrorRaised(string.Format("NUL文字が出現しました。"));
					return;
				}

				if(token.IsWhiteSpace){
					// ToDo: Reconstruct the active formatting elements, if any.
					tree.InsertCharacter((CharacterToken)token);
					return;
				}

				if(token is CharacterToken){
					// ToDo: Reconstruct the active formatting elements, if any.
					tree.InsertCharacter((CharacterToken)token);
					tree.Parser.FramesetOK = false;
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
					tree.Parser.OnParseErrorRaised(string.Format("予期せぬ箇所にhtml要素開始タグがあります。"));
					XmlElement topElement = tree.StackOfOpenElements[0];
					tree.MergeAttribute(topElement, (TagToken)token);
					return;
				}

				if(token.IsStartTag("base", "basefont", "bgsound", "command", "link", "meta", "noframes", "script", "style", "title")){
					tree.AppendToken<InHeadInsertionMode>(token);
					return;
				}

				if(token.IsStartTag("body")){
					tree.Parser.OnParseErrorRaised(string.Format("予期せぬ箇所にbody要素開始タグがあります。"));
					XmlElement bodyElement = tree.StackOfOpenElements[1];
					if(bodyElement == null || bodyElement.Name != "body") return;
					tree.Parser.FramesetOK = false;

					tree.MergeAttribute(bodyElement, (TagToken)token);
					return;
				}

				if(token.IsStartTag("frameset")){
					tree.Parser.OnParseErrorRaised(string.Format("予期せぬ箇所にframeset要素開始タグがあります。"));
					XmlElement bodyElement = tree.StackOfOpenElements[1];
					if(bodyElement == null || bodyElement.Name != "body") return;
					if(tree.Parser.FramesetOK == false) return;

					bodyElement.ParentNode.RemoveChild(bodyElement);
					while(tree.StackOfOpenElements.Count > 1) tree.StackOfOpenElements.Pop();
					tree.InsertElementForToken((TagToken)token);
					tree.ChangeInsertionMode<InFramesetInsertionMode>();

					return;
				}

				if(token.IsEndTag("body")){

				}


				Console.WriteLine("========\nnot implemented: {0} - {1}", this.Name, token);
				tree.Parser.Stop();
				return;
			}

		}
	}
}
