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
			if(token.IsWhiteSpace){
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
				OnParseErrorRaised(string.Format("{0}要素の開始タグが出現しましたが、この文脈でこの要素が出現することはできません。", token.Name));
				return; 

			}
			// Any Other Start Tag
		}

		public override void AppendAnythingElse(TreeConstruction tree, Token token){
			throw new Exception(string.Format("このモードでの処理が定義されていないトークンです。モード: {0} トークン: {1}", this.Name, token));
		}

	}
}
