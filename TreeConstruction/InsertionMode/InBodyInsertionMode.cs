using System;
using System.Reflection;
using System.Xml;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class InBodyInsertionMode : InsertionMode{


			private string[] myEndOfFilePermitOpenTags = new string[]{"dd", "dt", "li", "p", "tbody", "td", "tfoot", "th", "thead", "tr", "body", "html"};
			private string[] myBodyEndTagPermitOpenTags = new string[]{"dd", "dt", "li", "optgroup", "option", "p", "rp", "rt", "tbody", "td", "tfoot", "th", "thead", "tr", "body", "html"};


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

				if(token is EndOfFileToken){
					string invalidOpenTag = tree.StackOfOpenElements.NotEither(myEndOfFilePermitOpenTags);

					if(invalidOpenTag != null){
						tree.Parser.OnParseErrorRaised(string.Format("{0}の終了タグが不足しています。", invalidOpenTag));
					}
					tree.Parser.Stop();
					return;
				}

				if(token.IsEndTag("body")){
					EndTagBodyHadBeSeen(tree, token);
					return;
				}

				if(token.IsEndTag("html")){
					EndTagBodyHadBeSeen(tree, token);
					tree.ReprocessFlag = true;
					return;
				}

				if(token.IsStartTag("address", "article", "aside", "blockquote", "center", "details", "dir", "div", "dl", "fieldset", "figcaption", "figure", "footer", "header", "hgroup", "menu", "nav", "ol", "p", "section", "summary", "ul")){
					if(!tree.StackOfOpenElements.HaveElementInButtonScope("p")){
						EndTagPHadBeSeen(tree, token);
					}
					tree.InsertElementForToken((TagToken)token);
					return;
				}

				Console.WriteLine("========\nnot implemented: {0} - {1}", this.Name, token);
				tree.Parser.Stop();
				return;
			}


// private

			private void EndTagBodyHadBeSeen(TreeConstruction tree, Token token){
				if(!tree.StackOfOpenElements.HaveElementInScope("body")){
					tree.Parser.OnParseErrorRaised(string.Format("予期せぬ箇所で終了タグが出現しました。: {0}", token.Name));
					return;
				}
				string invalidOpenTag = tree.StackOfOpenElements.NotEither(myBodyEndTagPermitOpenTags);
				if(invalidOpenTag != null){
					tree.Parser.OnParseErrorRaised(string.Format("{0}の終了タグが不足しています。", invalidOpenTag));
				}
				tree.ChangeInsertionMode<AfterBodyInsertionMode>();
				return;
			}

			private void EndTagPHadBeSeen(TreeConstruction tree, Token token){

			}


		}
	}
}
