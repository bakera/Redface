using System;
using System.Xml;

namespace Bakera.RedFace{


	public class InBodyInsertionMode : InsertionMode{


		private string[] myEndOfFilePermitOpenTags = new string[]{"dd", "dt", "li", "p", "tbody", "td", "tfoot", "th", "thead", "tr", "body", "html"};
		private string[] myBodyEndTagPermitOpenTags = new string[]{"dd", "dt", "li", "optgroup", "option", "p", "rp", "rt", "tbody", "td", "tfoot", "th", "thead", "tr", "body", "html"};
		private string[] myHeadingElements = new string[]{"h1", "h2", "h3", "h4", "h5", "h6"};


		public override void AppendDoctypeToken(TreeConstruction tree, DoctypeToken token){
			OnParseErrorRaised(string.Format("先頭以外の箇所に文書型宣言があります。"));
			return;
		}

		public override void AppendCommentToken(TreeConstruction tree, CommentToken token){
			tree.AppendCommentForToken(token);
		}

		public override void AppendCharacterToken(TreeConstruction tree, CharacterToken token){
			if(token.IsNULL){
				OnParseErrorRaised(string.Format("NUL文字が出現しました。"));
				return;
			}
			if(token.IsWhiteSpace){
				Reconstruct(tree, token);
				tree.InsertCharacter(token);
				return;
			}
			Reconstruct(tree, token);
			tree.InsertCharacter(token);
			tree.Parser.FramesetOK = false;
		}

		public override void AppendEndOfFileToken(TreeConstruction tree, EndOfFileToken token){
			string invalidOpenTag = tree.StackOfOpenElements.NotEither(myEndOfFilePermitOpenTags);
			if(invalidOpenTag != null){
				OnParseErrorRaised(string.Format("{0}の終了タグが不足しています。", invalidOpenTag));
			}
			tree.Parser.Stop();
			return;
		}


		public override void AppendStartTagToken(TreeConstruction tree, StartTagToken token){
			if(token.IsStartTag("html")){
				OnParseErrorRaised(string.Format("予期せぬ箇所にhtml要素開始タグがあります。"));
				XmlElement topElement = tree.StackOfOpenElements[0];
				tree.MergeAttribute(topElement, token);
				return;
			}

			if(token.IsStartTag("base", "basefont", "bgsound", "command", "link", "meta", "noframes", "script", "style", "title")){
				tree.AppendToken<InHeadInsertionMode>(token);
				return;
			}

			if(token.IsStartTag("body")){
				OnParseErrorRaised(string.Format("予期せぬ箇所にbody要素開始タグがあります。"));
				XmlElement bodyElement = tree.StackOfOpenElements[1];
				if(bodyElement == null || bodyElement.Name != "body") return;
				tree.Parser.FramesetOK = false;

				tree.MergeAttribute(bodyElement, token);
				return;
			}

			if(token.IsStartTag("frameset")){
				OnParseErrorRaised(string.Format("予期せぬ箇所にframeset要素開始タグがあります。"));
				XmlElement bodyElement = tree.StackOfOpenElements[1];
				if(bodyElement == null || bodyElement.Name != "body") return;
				if(tree.Parser.FramesetOK == false) return;

				bodyElement.ParentNode.RemoveChild(bodyElement);
				while(tree.StackOfOpenElements.Count > 1) tree.StackOfOpenElements.Pop();
				tree.InsertElementForToken(token);
				tree.ChangeInsertionMode<InFramesetInsertionMode>();
				return;
			}

			if(token.IsStartTag("address", "article", "aside", "blockquote", "center", "details", "dir", "div", "dl", "fieldset", "figcaption", "figure", "footer", "header", "hgroup", "menu", "nav", "ol", "p", "section", "summary", "ul")){
				if(tree.StackOfOpenElements.HaveElementInButtonScope("p")){
					EndTagPHadBeSeen(tree, token);
				}
				tree.InsertElementForToken(token);
				return;
			}

			if(token.IsStartTag(myHeadingElements)){
				if(tree.StackOfOpenElements.HaveElementInButtonScope("p")){
					EndTagPHadBeSeen(tree, token);
				}
				if(tree.StackOfOpenElements.IsCurrentNameMatch(myHeadingElements)){
					OnParseErrorRaised(string.Format("見出し要素の終了タグがありません。: {0}", tree.CurrentNode.Name));
					tree.StackOfOpenElements.Pop();
				}
				tree.InsertElementForToken(token);
				return;
			}

			if(token.IsStartTag("pre", "listing")){
				if(tree.StackOfOpenElements.HaveElementInButtonScope("p")){
					EndTagPHadBeSeen(tree, token);
				}
				tree.InsertElementForToken(token);
				tree.Parser.FramesetOK = false;
				tree.IgnoreNextLineFeed = true;
				return;
			}

			if(token.IsStartTag("form")){
				if(tree.FormElementPointer != null){
					OnParseErrorRaised(string.Format("form要素が入れ子になっています。"));
					return;
				}
				if(tree.StackOfOpenElements.HaveElementInButtonScope("p")){
					EndTagPHadBeSeen(tree, token);
				}
				XmlElement form = tree.InsertElementForToken(token);
				tree.FormElementPointer = form;
				return;
			}

			if(token.IsStartTag("li")){
				tree.Parser.FramesetOK = false;
				foreach(XmlElement e in tree.StackOfOpenElements){
					if(StackOfElements.IsNameMatch(e, "li")){
						EndTagHadBeSeen(tree, "li");
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
				tree.InsertElementForToken(token);
				return;
			}

			if(token.IsStartTag("dd", "dt")){
				tree.Parser.FramesetOK = false;
				foreach(XmlElement e in tree.StackOfOpenElements){
					if(StackOfElements.IsNameMatch(e, "dd", "dt")){
						EndTagHadBeSeen(tree, token.Name);
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
				tree.InsertElementForToken(token);
				return;
			}

			if(token.IsStartTag("plaintext")){
				if(tree.StackOfOpenElements.HaveElementInButtonScope("p")){
					EndTagPHadBeSeen(tree, token);
				}
				tree.InsertElementForToken(token);
				tree.Parser.ChangeTokenState<PLAINTEXTState>();
				return;
			}

			if(token.IsStartTag("button")){
				if(tree.StackOfOpenElements.HaveElementInScope("button")){
					OnParseErrorRaised(string.Format("button要素の終了タグが不足しています。"));
					EndTagHadBeSeen(tree, "button");
					tree.ReprocessFlag = true;
					return;
				}
				Reconstruct(tree, token);
				tree.InsertElementForToken(token);
				tree.Parser.FramesetOK = false;
				return;
			}

			if(token.IsStartTag("a")){
				var list = tree.ListOfActiveFormatElements;
				var stack = tree.StackOfOpenElements;
				int index = list.GetAfterMarkerIndexByName("a");
				if(index >=0){
					XmlElement aElement = list.GetAfterMarkerByAfterIndex(index);
					OnParseErrorRaised(string.Format("a要素の中に他のa要素を入れ子にすることはできません。"));
					EndTagHadBeSeen(tree, "a");
					stack.Remove(aElement);
					list.Remove(aElement);
				}
				Reconstruct(tree, token);
				XmlElement e = tree.InsertElementForToken(token);
				tree.ListOfActiveFormatElements.Push(e, token);
				return;
			}

			if(token.IsStartTag("b", "big", "code", "em", "font", "i", "s", "small", "strike", "strong", "tt", "u")){
				Reconstruct(tree, token);
				XmlElement e = tree.InsertElementForToken(token);
				tree.ListOfActiveFormatElements.Push(e, token);
				return;
			}

			if(token.IsStartTag("nobr")){
				Reconstruct(tree, token);
				if(!tree.StackOfOpenElements.HaveElementInScope(token.Name)){
					OnParseErrorRaised(string.Format("nobr開始タグが出現しましたが、以前のnobrが終了していません。"));
					FormatEndTagHadBeSeen(tree, token, "nobr");
					Reconstruct(tree, token);
					return;
				}
				XmlElement e = tree.InsertElementForToken(token);
				tree.ListOfActiveFormatElements.Push(e, token);
				return;
			}

			if(token.IsStartTag("applet", "marquee", "object")){
				Reconstruct(tree, token);
				tree.InsertElementForToken(token);
				tree.Parser.FramesetOK = false;
				tree.ListOfActiveFormatElements.InsertMarker();
				return;
			}

			if(token.IsStartTag("table")){
				if(tree.Document.DocumentMode == DocumentMode.Quirks && tree.StackOfOpenElements.HaveElementInButtonScope("p")){
					EndTagPHadBeSeen(tree, token);
				}
				tree.InsertElementForToken(token);
				tree.Parser.FramesetOK = false;
				tree.ChangeInsertionMode<InTableInsertionMode>();
				return;
			}

			if(token.IsStartTag("area", "br", "embed", "img", "keygen", "wbr")){
				Reconstruct(tree, token);
				tree.InsertElementForToken(token);
				tree.PopFromStack();
				tree.AcknowledgeSelfClosingFlag(token);
				tree.Parser.FramesetOK = false;
				return;
			}

			if(token.IsStartTag("input")){
				Reconstruct(tree, token);
				tree.InsertElementForToken(token);
				tree.PopFromStack();
				tree.AcknowledgeSelfClosingFlag(token);
				if(!token.IsHiddenType()){
					tree.Parser.FramesetOK = false;
				}
				return;
			}

			if(token.IsStartTag("param", "source", "track")){
				tree.InsertElementForToken(token);
				tree.AcknowledgeSelfClosingFlag(token);
				tree.PopFromStack();
				return;
			}

			if(token.IsStartTag("hr")){
				if(tree.StackOfOpenElements.HaveElementInButtonScope("p")){
					EndTagPHadBeSeen(tree, token);
				}
				tree.InsertElementForToken(token);
				tree.AcknowledgeSelfClosingFlag(token);
				tree.PopFromStack();
				return;
			}

			if(token.IsStartTag("image")){
				OnParseErrorRaised("HTML5ではimage要素を使用することはできません。img要素に置き換えます。");
				token.Name = "img";
				tree.ReprocessFlag = true;
				return;
			}

			if(token.IsStartTag("isindex")){
				OnParseErrorRaised("HTML5ではisindex要素を使用することはできません。");
				XmlElement node = tree.FormElementPointer;
				if(node != null) return;
				tree.AcknowledgeSelfClosingFlag(token);

				StartTagHadBeSeen(tree, "form");
				XmlElement form = (XmlElement)tree.CurrentNode;
				string actionAttrValue = token.GetAttributeValue("action");
				if(actionAttrValue != null) form.SetAttribute("action", actionAttrValue);
				StartTagHadBeSeen(tree, "hr");
				StartTagHadBeSeen(tree, "label");
				string promptAttrValue = token.GetAttributeValue("prompt");
				if(promptAttrValue == null){
					tree.InsertText("This is a searchable index. Enter search keywords:");
				} else {
					tree.InsertText(promptAttrValue);

				}
				StartTagHadBeSeen(tree, "input");
				XmlElement input = (XmlElement)tree.CurrentNode["input"];
				input.SetAttribute("name", "isindex");
				foreach(AttributeToken at in token.Attributes){
					if(at.Name == "name" || at.Name == "action" || at.Name == "prompt") continue;
					input.SetAttribute(at.Name, at.Value);
				}
				EndTagHadBeSeen(tree, "label");
				StartTagHadBeSeen(tree, "hr");
				EndTagHadBeSeen(tree, "form");
				return;
			}

			if(token.IsStartTag("textarea")){
				tree.InsertElementForToken(token);
				tree.IgnoreNextLineFeed = true;
				tree.Parser.ChangeTokenState<RCDATAState>();
				tree.OriginalInsertionMode = tree.CurrentInsertionMode;
				tree.Parser.FramesetOK = false;
				tree.ChangeInsertionMode<TextInsertionMode>();
				return;
			}

			if(token.IsStartTag("xmp")){
				if(tree.StackOfOpenElements.HaveElementInButtonScope("p")){
					EndTagPHadBeSeen(tree, token);
				}
				Reconstruct(tree, token);
				tree.Parser.FramesetOK = false;
				GenericRawtextElementParsingAlgorithm(tree, token);
				return;
			}

			if(token.IsStartTag("iframe")){
				tree.Parser.FramesetOK = false;
				GenericRawtextElementParsingAlgorithm(tree, token);
				return;
			}

			// start tag whose tag name is "noscript", if the scripting flag is enabled
			if(token.IsStartTag("noembed")){
				GenericRawtextElementParsingAlgorithm(tree, token);
				return;
			}

			if(token.IsStartTag("select")){
				Reconstruct(tree, token);
				tree.InsertElementForToken(token);
				tree.Parser.FramesetOK = false;
				if(tree.CurrentInsertionMode is TableRelatedInsertionMode){
					tree.ChangeInsertionMode<InSelectInTableInsertionMode>();
				} else {
					tree.ChangeInsertionMode<InSelectInsertionMode>();
				}
				return;
			}

			if(token.IsStartTag("optgroup", "option")){
				if(tree.CurrentNode.Name == "option") EndTagHadBeSeen(tree, "option");
				Reconstruct(tree, token);
				tree.InsertElementForToken(token);
				return;
			}

			if(token.IsStartTag("rp", "rt")){
				if(tree.StackOfOpenElements.HaveElementInScope("ruby")){
					GenerateImpliedEndTags(tree, token);
					if(!tree.StackOfOpenElements.IsCurrentNameMatch("ruby")){
						OnParseErrorRaised(string.Format("{0}要素が出現しましたが、親要素がruby要素ではありません。: {1}", token.Name, tree.CurrentNode.Name));
						tree.StackOfOpenElements.PopUntilSameTagName("ruby");
					}
				}
				tree.InsertElementForToken(token);
				return;
			}

			if(token.IsStartTag("math")){
				Reconstruct(tree, token);
				StartTagToken t = (StartTagToken)token;
				t.AdjustMathMLAttributes();
				t.AdjustForeignAttributes();
				XmlElement result = tree.CreateElementForToken(t, Document.MathMLNamespace);
				tree.InsertElement(result);
				if(t.SelfClosing){
					tree.AcknowledgeSelfClosingFlag(t);
					tree.PopFromStack();
				}
				return;
			}

			if(token.IsStartTag("svg")){
				Reconstruct(tree, token);
				StartTagToken t = (StartTagToken)token;
				t.AdjustSVGAttributes();
				t.AdjustForeignAttributes();
				XmlElement result = tree.CreateElementForToken(t, Document.SVGNamespace);
				tree.InsertElement(result);
				if(t.SelfClosing){
					tree.AcknowledgeSelfClosingFlag(t);
					tree.PopFromStack();
				}
				return;
			}

			if(token.IsStartTag("caption", "col", "colgroup", "frame", "head", "tbody", "td", "tfoot", "th", "thead", "tr")){
				OnParseErrorRaised(string.Format("{0}要素の開始タグが出現しましたが、この文脈でこの要素が出現することはできません。", token.Name));
				return;
			}

			// Any Other Start Tags.
			Reconstruct(tree, token);
			tree.InsertElementForToken(token);
			return;

		}


		public override void AppendEndTagToken(TreeConstruction tree, EndTagToken token){

			if(token.IsEndTag("body")){
				if(!tree.StackOfOpenElements.HaveElementInScope("body")){
					OnParseErrorRaised(string.Format("予期せぬ箇所で終了タグが出現しました。: {0}", token.Name));
					return;
				}
				string invalidOpenTag = tree.StackOfOpenElements.NotEither(myBodyEndTagPermitOpenTags);
				if(invalidOpenTag != null){
					OnParseErrorRaised(string.Format("{0}の終了タグが不足しています。", invalidOpenTag));
				}
				tree.ChangeInsertionMode<AfterBodyInsertionMode>();
				return;
			}

			if(token.IsEndTag("html")){
				EndTagHadBeSeen(tree, "body");
				tree.ReprocessFlag = true;
				return;
			}

			if(token.IsEndTag("address", "article", "aside", "blockquote", "button", "center", "details", "dir", "div", "dl", "fieldset", "figcaption", "figure", "footer", "header", "hgroup", "listing", "menu", "nav", "ol", "pre", "section", "summary", "ul")){
				if(!tree.StackOfOpenElements.HaveElementInScope(token.Name)){
					OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", token.Name));
					return;
				}
				GenerateImpliedEndTags(tree, token);
				if(!tree.StackOfOpenElements.IsCurrentNameMatch(token.Name)){
					OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", token.Name));
				}
				tree.StackOfOpenElements.PopUntilSameTagName(token.Name);
				return;
			}

			if(token.IsEndTag("form")){
				XmlElement node = tree.FormElementPointer;
				tree.FormElementPointer = null;
				if(node == null || !tree.StackOfOpenElements.Contains(node)){
					OnParseErrorRaised(string.Format("formの終了タグが出現しましたが、対応するform要素がありません。"));
					return;
				}
				GenerateImpliedEndTags(tree, token);
				if(tree.CurrentNode != node){
					OnParseErrorRaised(string.Format("formの終了タグが出現しましたが、対応するform要素がありません。"));
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
					OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", token.Name));
					return;
				}
				GenerateImpliedEndTags(tree, token, token.Name);
				if(!tree.StackOfOpenElements.IsCurrentNameMatch(token.Name)){
					OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", token.Name));
				}
				tree.StackOfOpenElements.PopUntilSameTagName(token.Name);
				return;
			}

			if(token.IsEndTag("dd", "dt")){
				if(!tree.StackOfOpenElements.HaveElementInScope(token.Name)){
					OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", token.Name));
					return;
				}
				GenerateImpliedEndTags(tree, token, token.Name);
				if(!tree.StackOfOpenElements.IsCurrentNameMatch(token.Name)){
					OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", token.Name));
				}
				tree.StackOfOpenElements.PopUntilSameTagName(token.Name);
				return;
			}

			if(token.IsEndTag(myHeadingElements)){
				if(!tree.StackOfOpenElements.HaveElementInScope(token.Name)){
					OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", token.Name));
					return;
				}
				GenerateImpliedEndTags(tree, token);
				if(!tree.StackOfOpenElements.IsCurrentNameMatch(token.Name)){
					OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", token.Name));
				}
				tree.StackOfOpenElements.PopUntilSameTagName(myHeadingElements);
				return;
			}

			if(token.IsEndTag("sarcasm")){
				OnDeepBreath();
				AnyOtherEndTag(tree, token);
				return;
			}

			if(token.IsEndTag("a", "b", "big", "code", "em", "font", "i", "nobr", "s", "small", "strike", "strong", "tt", "u")){
				FormatEndTagHadBeSeen(tree, token, token.Name);
				return;
			}

			if(token.IsEndTag("applet", "marquee", "object")){
				if(!tree.StackOfOpenElements.HaveElementInScope(token.Name)){
					OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", token.Name));
					return;
				}
				GenerateImpliedEndTags(tree, token);
				if(!tree.StackOfOpenElements.IsCurrentNameMatch(token.Name)){
					OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", token.Name));
				}
				tree.StackOfOpenElements.PopUntilSameTagName(token.Name);
				tree.ListOfActiveFormatElements.ClearUpToTheLastMarker();
				return;
			}

			if(token.IsEndTag("br")){
				OnParseErrorRaised(string.Format("br要素の終了タグが出現しました。"));
				StartTagHadBeSeen(tree, "br");
				return;
			}

			AnyOtherEndTag(tree, token);
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
				OnImpliedEndTagInserted(e, token);
			}
			return;
		}


// tags had be seen

		private void StartTagHadBeSeen(TreeConstruction tree, string name){
			AppendStartTagToken(tree, new FakeStartTagToken(){Name = name});
		}

		private void EndTagHadBeSeen(TreeConstruction tree, string name){
			AppendEndTagToken(tree, new FakeEndTagToken(){Name = name});
		}

		private void EndTagPHadBeSeen(TreeConstruction tree, Token token){
			string tagName = "p";
			if(!tree.StackOfOpenElements.HaveElementInButtonScope(tagName)){
				OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", tagName));
				StartTagHadBeSeen(tree, "p");
				tree.ReprocessFlag = true;
				return;
			}
			GenerateImpliedEndTags(tree, token, tagName);
			if(!tree.StackOfOpenElements.IsCurrentNameMatch(tagName)){
				OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", tagName));
			}
			tree.StackOfOpenElements.PopUntilSameTagName(tagName);
			return;
		}

		private void AnyOtherEndTag(TreeConstruction tree, Token token){
			foreach(XmlElement node in tree.StackOfOpenElements){
				if(StackOfElements.IsNameMatch(node, token.Name)){
					GenerateImpliedEndTags(tree, token, token.Name);
					if(!tree.StackOfOpenElements.IsCurrentNameMatch(token.Name)){
						OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", token.Name));
					}
					tree.StackOfOpenElements.PopUntilSameElement(node);
					break;
				} else {
					if(StackOfElements.IsSpecialElement(node)){
						OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", token.Name));
						return;
					}
				}
			}
			return;
		}

		private void FormatEndTagHadBeSeen(TreeConstruction tree, Token token, string tagName){
			ListOfElements list = tree.ListOfActiveFormatElements;
			StackOfElements stack = tree.StackOfOpenElements;
			int outerLoopCounter = 0;

			while(outerLoopCounter < 8){
				outerLoopCounter++;
				int formattingElementItemIndex = list.GetAfterMarkerIndexByName(token.Name);
				if(formattingElementItemIndex < 0){
					AnyOtherEndTag(tree, token);
					return;
				}

				XmlElement formattingElement = list.GetAfterMarkerByAfterIndex(formattingElementItemIndex);

				if(!stack.IsInclude(formattingElement)){
					OnParseErrorRaised(string.Format("終了タグが出現しました。対応する要素はListOfActiveFormatElementsに含まれていますが、StackOfOpenElementsに含まれていません。: {0}", token.Name));
					list.RemoveAfterMarkerByAfterIndex(formattingElementItemIndex);
					return;
				}

				if(!stack.HaveElementInScope(formattingElement.Name)){
					OnParseErrorRaised(string.Format("終了タグが出現しました。対応する要素はListOfActiveFormatElementsに含まれていますが、StackOfOpenElementsのscope内に含まれていません。: {0}", token.Name));
					return;
				}

				if(tree.CurrentNode != formattingElement){
					OnParseErrorRaised(string.Format("終了タグが出現しました。対応する要素はListOfActiveFormatElementsに含まれており、StackOfOpenElementsのscope内にありますが、CurrentNodeではありません。: {0}", token.Name));
					// エラーだが処理は続行
				}

				XmlElement furthestBlock = stack.GetFurthestBlock(formattingElement);
				if(furthestBlock == null){
					stack.PopUntilSameElement(formattingElement);
					list.RemoveAfterMarkerByAfterIndex(formattingElementItemIndex);
					return;
				}

				XmlElement commonAncestor = stack.GetAncestor(formattingElement);

				// bookmark
				int bookmarkPosition = list.GetIndexByElement(formattingElement);

				XmlElement node = furthestBlock;
				XmlElement lastNode = furthestBlock;

				int innerLoopCounter = 0;
				while(innerLoopCounter < 3){
					innerLoopCounter++;
					node = stack.GetAncestor(node);
					int idx = list.GetIndexByElement(node);
					if(idx < 0){
						node = stack.Remove(node);
						continue;
					}
					if(stack.IsFormattingElement(node)){
						break;
					}
					TagToken creator = tree.GetToken(node);
					XmlElement newNode = tree.CreateElementForToken(creator);
					list[idx] = newNode;
					stack.Replace(node, newNode);
					node = newNode;
					if(lastNode == furthestBlock){
						bookmarkPosition = idx+1;
					}
					node.AppendChild(lastNode);
					lastNode = node;
				}
				if(stack.IsTableRealtedElement(commonAncestor)){
					stack.FosterParent(lastNode);
				} else {
					commonAncestor.AppendChild(lastNode);
				}

				TagToken formatElementCreator = tree.GetToken(node);
				XmlElement newFormattingElement = tree.CreateElementForToken(formatElementCreator);
				foreach(XmlNode x in furthestBlock.ChildNodes){
					newFormattingElement.AppendChild(x);
				}
				furthestBlock.AppendChild(newFormattingElement);

				list.Remove(formattingElement);
				list.Insert(bookmarkPosition, newFormattingElement);

				stack.Remove(formattingElement);
				stack.InsertBelow(furthestBlock, newFormattingElement);

			}
			return;
		}




// reconstruct 

		public void Reconstruct(TreeConstruction tree, Token token){
			ListOfElements list = tree.ListOfActiveFormatElements;
			StackOfElements stack = tree.StackOfOpenElements;
			if(list.Length == 0) return;

			XmlElement lastEntry = list[list.Length - 1];
			if(lastEntry == null) return;
			if(stack.IsInclude(lastEntry)) return;

			// step4～7は要するに「stackに含まれる要素やmarkerを除いた、最も先祖にあるentryを取得」という処理をする。
			// index = 0 なら最上位なので、そのentryを使用する。
			// 一つ上を見て、そのentryがstackもしくはmarkerなら、そのentryを使用する。
			// step7が別の目的にも使いまわされているのが読みにくい原因。
			int index = list.Length-1;
			while(index > 0){
				XmlElement parentEntry = list[index-1];
				if(parentEntry == null || stack.IsInclude(parentEntry)){
					break;
				}
				index--;
			}

			XmlElement entry = null;
			while(index < list.Length){
				entry = list[index];
				TagToken creator = tree.GetToken(entry);
				XmlElement e = tree.CreateElementForToken(creator);
				tree.InsertElement(e);
				list[index] = e;
				index++;
			}
			return;
		}

	}
}
