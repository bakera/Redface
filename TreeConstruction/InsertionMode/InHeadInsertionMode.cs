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
				string charsetValue = token.GetAttributeValue("charset");
				string httpEquivValue = token.GetAttributeValue("http-equiv");
				string contentValue = token.GetAttributeValue("content");
				string charsetName = null;


				if(charsetValue != null){
					OnMessageRaised(EventLevel.Verbose, string.Format("meta要素のcharset属性が指定されています。: {0}", charsetValue));
					charsetName = charsetValue;
					if(charsetName == ""){
						OnInformationRaised(string.Format("meta要素のcharset属性に空の値が指定されています。: {0}", charsetValue));
						return;
					}
					OnInformationRaised(string.Format("meta charset で文字符号化方式が指定されています。: {0}", charsetValue));
				} else if(httpEquivValue != null && httpEquivValue.Equals("Content-Type", StringComparison.InvariantCultureIgnoreCase) && contentValue != null){
					charsetName = EncodingSniffer.ExtractEncodingNameFromMetaElement(contentValue);

					if(string.IsNullOrEmpty(charsetName)){
						OnInformationRaised(string.Format("meta要素でhttp-equiv=\"Content-Type\"が指定されていますが、content属性に有効なcharsetの指定がありません。: {0}", contentValue));
						return;
					}
					OnInformationRaised(string.Format("meta http-equivで文字符号化方式が指定されています。: {0}({1})", contentValue, charsetName));
				}

				if(charsetName == null) return;

				InputStream stream = tree.Parser.InputStream;
				if(stream.EncodingConfidence == EncodingConfidence.Irrelevant){
					OnMessageRaised(EventLevel.Verbose, string.Format("metaで文字符号化方式が指定されていますが、文字符号化方式の判別が必要ないモードであるため、指定を無視します。"));
					return;
				}
				OnMessageRaised(EventLevel.Verbose, string.Format("metaの属性値から文字符号化方式を決定します。"));
				Encoding enc = EncodingSniffer.GetEncodingByName(charsetName);
				if(enc == null){
					OnMessageRaised(EventLevel.Information, string.Format("指定された名前から文字符号化方式を取得できませんでした。: {0}", charsetName));
					return;
				}

				if(stream.EncodingConfidence == EncodingConfidence.Certain){
					if(enc == stream.Encoding){
						OnInformationRaised(string.Format("metaで文字符号化方式が指定されていますが、既に確定している文字符号化方式と一致しています。指定を無視します。: {0}", enc.EncodingName));
					} else {
						OnInformationRaised(string.Format("文字符号化方式は既に確定していますが、metaでは異なる文字符号化方式が指定されています。指定を無視します。: {0}", enc.EncodingName));
					}
					return;
				}

				// ToDo: Encodingを変更する
				stream.ChangeEncoding(enc);
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
