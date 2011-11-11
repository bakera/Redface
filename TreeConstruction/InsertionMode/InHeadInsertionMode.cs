using System;
using System.Text;
using System.Xml;

namespace Bakera.RedFace{


	public class InHeadInsertionMode : InsertionMode{


		public override void AppendCharacterToken(TreeConstruction tree, CharacterToken token){
			if(token.IsSpaceCharacter){
				tree.InsertCharacter((CharacterToken)token);
				return;
			}
			AppendAnythingElse(tree, token);
		}

		public override void AppendCommentToken(TreeConstruction tree, CommentToken token){
			tree.AppendCommentForToken(token);
		}

		public override void AppendDoctypeToken(TreeConstruction tree, DoctypeToken token){
			OnParseErrorRaised(string.Format("先頭以外の箇所に文書型宣言があります。"));
		}

		public override void AppendStartTagToken(TreeConstruction tree, StartTagToken token){
			if(token.IsStartTag("html")){
				tree.AppendToken<InBodyInsertionMode>(token);
				return;
			}

			if(token.IsStartTag("base", "basefont", "bgsound", "command", "link")){
				tree.InsertElementForToken(token);
				tree.PopFromStack();
				tree.AcknowledgeSelfClosingFlag(token);
				return;
			}

			if(token.IsStartTag("meta")){
				tree.InsertElementForToken(token);
				tree.PopFromStack();
				tree.AcknowledgeSelfClosingFlag(token);

				// charset判定
				// tentative 

				string charsetValue = token.GetAttributeValue("charset");
				if(!string.IsNullOrEmpty(charsetValue)){
					Encoding enc = EncodingSniffer.GetEncodingByName(charsetValue);
					OnInformationRaised(string.Format("meta charset で文字符号化方式が指定されています。: {0} / {1}", charsetValue, enc.EncodingName));
				// ToDo: process charset
				}

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
				tree.InsertElementForToken(token);
				tree.ChangeInsertionMode<InHeadNoscriptInsertionMode>();
				return;
			}

			if(token.IsStartTag("script")){
				XmlElement scriptElement = tree.CreateElementForToken(token);
				tree.AppendChild(scriptElement);
				tree.PutToStack(scriptElement);
				tree.Parser.ChangeTokenState<ScriptDataState>();
				tree.OriginalInsertionMode = tree.CurrentInsertionMode;
				tree.ChangeInsertionMode<TextInsertionMode>();
				return;
			}

			if(token.IsStartTag("head")){
				OnParseErrorRaised(string.Format("head要素の開始タグが重複しています。"));
				return;
			}
			AppendAnythingElse(tree, token);
		}

		public override void AppendEndTagToken(TreeConstruction tree, EndTagToken token){
			if(token.IsEndTag("head")){
				tree.PopFromStack();
				tree.ChangeInsertionMode<AfterHeadInsertionMode>();
				return;
			}

			if(token.IsEndTag("body", "html", "br")){
				AppendAnythingElse(tree, token);
				return;
			}
			OnParseErrorRaised(string.Format("不明な終了タグがあります。"));
			return;
		}

		public override void AppendAnythingElse(TreeConstruction tree, Token token){
			AppendEndTagToken(tree, new FakeEndTagToken(){Name = "head"});
			tree.ReprocessFlag = true;
			return;
		}

	}
}
