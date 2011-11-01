using System;
using System.Xml;

namespace Bakera.RedFace{

	public class InForeignContent : InsertionMode{

		public override void AppendCharacterToken(TreeConstruction tree, CharacterToken token){
			if(token.IsNULL){
				OnParseErrorRaised(string.Format("NUL文字が出現しました。"));
				tree.InsertCharacter(Chars.REPLACEMENT_CHARACTER);
				return;
			}
			if(token.IsSpaceCharacter){
				tree.InsertCharacter(token);
				return;
			}
			tree.InsertCharacter(token);
			tree.Parser.FramesetOK = false;
		}


		public override void AppendCommentToken(TreeConstruction tree, CommentToken token){
			tree.AppendCommentForToken(token);
		}


		public override void AppendDoctypeToken(TreeConstruction tree, DoctypeToken token){
			OnParseErrorRaised(string.Format("先頭以外の箇所に文書型宣言があります。"));
			return;
		}


		public override void AppendEndOfFileToken(TreeConstruction tree, EndOfFileToken token){
			tree.Parser.Stop();
			return;
		}


		public override void AppendStartTagToken(TreeConstruction tree, StartTagToken token){
			switch(token.Name){
			case "b":
			case "big":
			case "blockquote":
			case "body":
			case "br":
			case "center":
			case "code":
			case "dd":
			case "div":
			case "dl":
			case "dt":
			case "em":
			case "embed":
			case "h1":
			case "h2":
			case "h3":
			case "h4":
			case "h5":
			case "h6":
			case "head":
			case "hr":
			case "i":
			case "img":
			case "li":
			case "listing":
			case "menu":
			case "meta":
			case "nobr":
			case "ol":
			case "p":
			case "pre":
			case "ruby":
			case "s":
			case "small":
			case "span":
			case "strong":
			case "strike":
			case "sub":
			case "sup":
			case "table":
			case "tt":
			case "u":
			case "ul":
			case "var":
				AppendHtmlFormatStartTags(tree, token);
				return; 

			case "font":
				if(token.HasAttribute("color") || token.HasAttribute("face") || token.HasAttribute("size")){
					AppendHtmlFormatStartTags(tree, token);
					return;
				} else {
					break; // not return, treat as any other start tag
				}
			}

			// Any Other Start Tag
			XmlElement currentNode = tree.CurrentNode as XmlElement;
			if(ElementInfo.IsMathMLNameSpace(currentNode)){
				token.AdjustMathMLAttributes();
				token.AdjustForeignAttributes();
				tree.InsertForeignElementForToken(token, Document.MathMLNamespace);
			} else if(ElementInfo.IsSVGNameSpace(currentNode)){
				token.AdjustSVGElementName();
				token.AdjustSVGAttributes();
				token.AdjustForeignAttributes();
				tree.InsertForeignElementForToken(token, Document.SVGNamespace);
			}
			if(token.SelfClosing){
				tree.AcknowledgeSelfClosingFlag(token);
				tree.PopFromStack();
			}
			return;
		}


		public override void AppendEndTagToken(TreeConstruction tree, EndTagToken token){
			switch(token.Name){
			case "script":
				XmlElement currentNode = tree.CurrentNode as XmlElement;
				if(ElementInfo.IsSVGNameSpace(currentNode) && currentNode.Name == "script"){
					tree.PopFromStack();
					return;
				}
				break;
			}

			// Any Other End Tag
			XmlElement node = tree.CurrentNode as XmlElement;
			if(!token.IsEndTag(node.Name)){
				OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", token.Name));
			}
			while(node != null){
				if(token.IsEndTag(node.Name.ToLower())){
					tree.StackOfOpenElements.PopUntilSameElement(node);
					break;
				}
				node = tree.StackOfOpenElements.GetAncestor(node);
				if(ElementInfo.IsHtmlNameSpace(node)) break;
			}
			tree.AppendToken(token);
		}


		public override void AppendAnythingElse(TreeConstruction tree, Token token){
			throw new Exception(string.Format("このモードでの処理が定義されていないトークンです。モード: {0} トークン: {1}", this.Name, token));
		}


		private void AppendHtmlFormatStartTags(TreeConstruction tree, Token token){
			OnParseErrorRaised(string.Format("{0}要素の開始タグが出現しましたが、この文脈でこの要素が出現することはできません。", token.Name));
			tree.PopFromStack();
			XmlElement currentNode = tree.CurrentNode as XmlElement;
			while(currentNode != null){
				if(ElementInfo.IsMathMLTextIntegrationPoint(currentNode)) break;
				if(ElementInfo.IsHtmlIntegrationPoint(currentNode)) break;
				if(currentNode.NamespaceURI.Equals(Document.HtmlNamespace, StringComparison.InvariantCulture)) break;
				tree.PopFromStack();
				currentNode = tree.CurrentNode as XmlElement;
			}
			tree.ReprocessFlag = true;
		}

	}
}
