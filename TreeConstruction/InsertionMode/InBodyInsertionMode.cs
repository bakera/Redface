using System;
using System.Reflection;
using System.Xml;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class InBodyInsertionMode : InsertionMode{


			private string[] myEndOfFilePermitOpenTags = new string[]{"dd", "dt", "li", "p", "tbody", "td", "tfoot", "th", "thead", "tr", "body", "html"};
			private string[] myBodyEndTagPermitOpenTags = new string[]{"dd", "dt", "li", "optgroup", "option", "p", "rp", "rt", "tbody", "td", "tfoot", "th", "thead", "tr", "body", "html"};
			private string[] myHeadingElements = new string[]{"h1", "h2", "h3", "h4", "h5", "h6"};

			public override void AppendToken(TreeConstruction tree, Token token){

				if(token.IsNULL){
					tree.Parser.OnParseErrorRaised(string.Format("NUL文字が出現しました。"));
					return;
				}

				if(token.IsWhiteSpace){
					Reconstruct(tree, token);
					tree.InsertCharacter((CharacterToken)token);
					return;
				}

				if(token is CharacterToken){
					Reconstruct(tree, token);
					tree.InsertCharacter((CharacterToken)token);
					tree.Parser.FramesetOK = false;
					return;
				}

				if(token is CommentToken){
					tree.AppendCommentForToken((CommentToken)token);
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
					if(tree.StackOfOpenElements.HaveElementInButtonScope("p")){
						EndTagPHadBeSeen(tree, token);
					}
					tree.InsertElementForToken((TagToken)token);
					return;
				}

				if(token.IsStartTag(myHeadingElements)){
					if(tree.StackOfOpenElements.HaveElementInButtonScope("p")){
						EndTagPHadBeSeen(tree, token);
					}
					if(tree.StackOfOpenElements.IsCurrentNameMatch(myHeadingElements)){
						tree.Parser.OnParseErrorRaised(string.Format("見出し要素の終了タグがありません。: {0}", tree.CurrentNode.Name));
						tree.StackOfOpenElements.Pop();
					}
					tree.InsertElementForToken((TagToken)token);
					return;
				}

				if(token.IsStartTag("pre", "listing")){
					if(tree.StackOfOpenElements.HaveElementInButtonScope("p")){
						EndTagPHadBeSeen(tree, token);
					}
					tree.InsertElementForToken((TagToken)token);
					tree.Parser.FramesetOK = false;
					tree.IgnoreNextWhiteSpace = true;
					return;
				}

				if(token.IsStartTag("form")){
					if(tree.FormElementPointer != null){
						tree.Parser.OnParseErrorRaised(string.Format("form要素が入れ子になっています。"));
						return;
					}
					if(tree.StackOfOpenElements.HaveElementInButtonScope("p")){
						EndTagPHadBeSeen(tree, token);
					}
					XmlElement form = tree.InsertElementForToken((TagToken)token);
					tree.FormElementPointer = form;
					return;
				}

				if(token.IsStartTag("li")){
					tree.Parser.FramesetOK = false;
					foreach(XmlElement e in tree.StackOfOpenElements){
						if(StackOfElements.IsNameMatch(e, "li")){
							EndTagLiHadBeSeen(tree, token);
							break;
						}
						if(StackOfElements.IsSpecialElement(e)){
							if(!StackOfElements.IsNameMatch(e, "address", "div", "p")){
								break;
							}
						}
					}
					if(tree.StackOfOpenElements.HaveElementInButtonScope("p")){
						EndTagPHadBeSeen(tree, token);
					}
					tree.InsertElementForToken((TagToken)token);
					return;
				}

				if(token.IsStartTag("dd", "dt")){
					tree.Parser.FramesetOK = false;
					foreach(XmlElement e in tree.StackOfOpenElements){
						if(StackOfElements.IsNameMatch(e, "dd", "dt")){
							EndTagLiHadBeSeen(tree, token);
							break;
						}
						if(StackOfElements.IsSpecialElement(e)){
							if(!StackOfElements.IsNameMatch(e, "address", "div", "p")){
								break;
							}
						}
					}
					if(tree.StackOfOpenElements.HaveElementInButtonScope("p")){
						EndTagPHadBeSeen(tree, token);
					}
					tree.InsertElementForToken((TagToken)token);
					return;
				}

				if(token.IsStartTag("plaintext")){
					if(tree.StackOfOpenElements.HaveElementInButtonScope("p")){
						EndTagPHadBeSeen(tree, token);
					}
					tree.InsertElementForToken((TagToken)token);
					tree.Parser.ChangeTokenState<PLAINTEXTState>();
					return;
				}

				if(token.IsStartTag("button")){
					if(tree.StackOfOpenElements.HaveElementInScope("button")){
						tree.Parser.OnParseErrorRaised(string.Format("button要素の終了タグが不足しています。"));
						EndTagButtonHadBeSeen(tree, token);
						tree.ReprocessFlag = true;
						return;
					}
					// ToDo: Reconstruct the active formatting elements, if any.
					tree.InsertElementForToken((TagToken)token);
					tree.Parser.FramesetOK = false;
					return;
				}

				if(token.IsEndTag("address", "article", "aside", "blockquote", "button", "center", "details", "dir", "div", "dl", "fieldset", "figcaption", "figure", "footer", "header", "hgroup", "listing", "menu", "nav", "ol", "pre", "section", "summary", "ul")){
					if(!tree.StackOfOpenElements.HaveElementInScope(token.Name)){
						tree.Parser.OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", token.Name));
						return;
					}
					GenerateImpliedEndTags(tree, token);
					if(!tree.StackOfOpenElements.IsCurrentNameMatch(token.Name)){
						tree.Parser.OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", token.Name));
					}
					tree.StackOfOpenElements.PopUntilSameTagName(token.Name);
					return;
				}


				if(token.IsEndTag("form")){
					XmlElement node = tree.FormElementPointer;
					tree.FormElementPointer = null;
					if(node == null || !tree.StackOfOpenElements.Contains(node)){
						tree.Parser.OnParseErrorRaised(string.Format("formの終了タグが出現しましたが、対応するform要素がありません。"));
						return;
					}
					GenerateImpliedEndTags(tree, token);
					if(tree.CurrentNode != node){
						tree.Parser.OnParseErrorRaised(string.Format("formの終了タグが出現しましたが、対応するform要素がありません。"));
					} else {
						tree.StackOfOpenElements.Pop();
					}
					return;
				}

				if(token.IsEndTag("p")){
					EndTagPHadBeSeen(tree, token);
					return;
				}

				if(token.IsEndTag("li")){
					if(!tree.StackOfOpenElements.HaveElementInListItemScope(token.Name)){
						tree.Parser.OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", token.Name));
						return;
					}
					GenerateImpliedEndTags(tree, token, token.Name);
					if(!tree.StackOfOpenElements.IsCurrentNameMatch(token.Name)){
						tree.Parser.OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", token.Name));
					}
					tree.StackOfOpenElements.PopUntilSameTagName(token.Name);
					return;
				}

				if(token.IsEndTag("dd", "dt")){
					if(!tree.StackOfOpenElements.HaveElementInScope(token.Name)){
						tree.Parser.OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", token.Name));
						return;
					}
					GenerateImpliedEndTags(tree, token, token.Name);
					if(!tree.StackOfOpenElements.IsCurrentNameMatch(token.Name)){
						tree.Parser.OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", token.Name));
					}
					tree.StackOfOpenElements.PopUntilSameTagName(token.Name);
					return;
				}

				if(token.IsEndTag(myHeadingElements)){
					if(!tree.StackOfOpenElements.HaveElementInScope(token.Name)){
						tree.Parser.OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", token.Name));
						return;
					}
					GenerateImpliedEndTags(tree, token);
					if(!tree.StackOfOpenElements.IsCurrentNameMatch(token.Name)){
						tree.Parser.OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", token.Name));
					}
					tree.StackOfOpenElements.PopUntilSameTagName(myHeadingElements);
					return;
				}

				if(token.IsEndTag("sarcasm")){
					tree.Parser.OnDeepBreath();
					AnyOtherEndTag(tree, token);
					return;
				}

				if(token.IsStartTag("a")){
					// ToDo: If the list of active formatting elements contains an element whose tag name is "a" between the end of the list and the last marker on the list (or the start of the list if there is no marker on the list), then this is a parse error; act as if an end tag with the tag name "a" had been seen, then remove that element from the list of active formatting elements and the stack of open elements if the end tag didn't already remove it (it might not have if the element is not in table scope).
					Reconstruct(tree, token);
					XmlElement e = tree.InsertElementForToken((TagToken)token);
					tree.ListOfActiveFormatElements.Push(e, (TagToken)token);
					return;
				}

				if(token.IsStartTag("b", "big", "code", "em", "font", "i", "s", "small", "strike", "strong", "tt", "u")){
					Reconstruct(tree, token);
					XmlElement e = tree.InsertElementForToken((TagToken)token);
					tree.ListOfActiveFormatElements.Push(e, (TagToken)token);
					return;
				}

				if(token.IsStartTag("nobr")){
					Reconstruct(tree, token);
					if(!tree.StackOfOpenElements.HaveElementInScope(token.Name)){
						tree.Parser.OnParseErrorRaised(string.Format("nobr開始タグが出現しましたが、以前のnobrが終了していません。"));
						FormatEndTagHadBeSeen(tree, token, "nobr");
						Reconstruct(tree, token);
						return;
					}
					XmlElement e = tree.InsertElementForToken((TagToken)token);
					tree.ListOfActiveFormatElements.Push(e, (TagToken)token);
					return;
				}

				if(token.IsEndTag("a", "b", "big", "code", "em", "font", "i", "nobr", "s", "small", "strike", "strong", "tt", "u")){
					FormatEndTagHadBeSeen(tree, token, token.Name);
					return;
				}

				if(token is StartTagToken){
					Reconstruct(tree, token);
					tree.InsertElementForToken((TagToken)token);
					return;
				}

				if(token is EndTagToken){
					AnyOtherEndTag(tree, token);
					return;
				}

				Console.WriteLine("========\nnot implemented: {0} - {1}", this.Name, token);
				tree.Parser.Stop();
				return;
			}


// private

			// 補える終了タグを全部補う処理
			// 補えないタイプの終了タグが出現したときにのみ呼ばれる
			private void GenerateImpliedEndTags(TreeConstruction tree, Token token){
				GenerateImpliedEndTags(tree, token, new string[0]);
				return;
			}

			// 補える終了タグを補う処理
			// 例外を指定
			private void GenerateImpliedEndTags(TreeConstruction tree, Token token, params string[] except){
				while(tree.StackOfOpenElements.IsImpliedEndTagElement()){
					if(tree.StackOfOpenElements.IsCurrentNameMatch(except)) break;
					XmlElement e = tree.StackOfOpenElements.Pop();
					tree.Parser.OnImpliedEndTagInserted(e, token);
				}
				return;
			}


// start tags 

			private void StartTagPHadBeSeen(TreeConstruction tree, Token token){
				if(tree.StackOfOpenElements.HaveElementInButtonScope("p")){
					EndTagPHadBeSeen(tree, token);
				}
				XmlElement p = tree.Document.CreateHtmlElement("p");
				tree.InsertElement(p);
				return;
			}



// end tags

			private void AnyOtherEndTag(TreeConstruction tree, Token token){
				foreach(XmlElement node in tree.StackOfOpenElements){
					if(StackOfElements.IsNameMatch(node, token.Name)){
						GenerateImpliedEndTags(tree, token, token.Name);
						if(!tree.StackOfOpenElements.IsCurrentNameMatch(token.Name)){
							tree.Parser.OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", token.Name));
						}
						tree.StackOfOpenElements.PopUntilSameElement(node);
						break;
					} else {
						if(StackOfElements.IsSpecialElement(node)){
							tree.Parser.OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", token.Name));
							return;
						}
					}
				}
				return;
			}


			private void FormatEndTagHadBeSeen(TreeConstruction tree, Token token, string tagName){

				// 1.Let outer loop counter be zero.
				int outerLoopCounter = 0;

				// 2.Outer loop: If outer loop counter is greater than or equal to eight, then abort these steps.
				while(outerLoopCounter < 8){
					// 3.Increment outer loop counter by one.
					outerLoopCounter++;
					// 4.Let the formatting element be the last element in the list of active formatting elements that:


				// ToDo:

				}
				return;
			}


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
				EndTagHadBeSeen(tree, token, "p");
			}

			private void EndTagHadBeSeen(TreeConstruction tree, Token token, string tagName){
				if(!tree.StackOfOpenElements.HaveElementInButtonScope(tagName)){
					tree.Parser.OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", tagName));
					StartTagPHadBeSeen(tree, token);
					tree.ReprocessFlag = true;
					return;
				}
				GenerateImpliedEndTags(tree, token, tagName);
				if(!tree.StackOfOpenElements.IsCurrentNameMatch(tagName)){
					tree.Parser.OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", tagName));
				}
				tree.StackOfOpenElements.PopUntilSameTagName(tagName);
				return;
			}


			private void EndTagLiHadBeSeen(TreeConstruction tree, Token token){

			}

			private void EndTagButtonHadBeSeen(TreeConstruction tree, Token token){

			}


// reconstruct 

			public void Reconstruct(TreeConstruction tree, Token token){
				if(tree.ListOfActiveFormatElements.Count == 0) return;
				// ToDo: Reconstructの仕組みを作る
				return;
			}

		}
	}
}
