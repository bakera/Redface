using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class AttributeNameState : TokenizationState{

			public override void Read(Tokenizer t){
				char? c = t.ConsumeChar();

				switch(c){
					case Chars.CHARACTER_TABULATION:
					case Chars.LINE_FEED:
					case Chars.FORM_FEED:
					case Chars.SPACE:
						t.ChangeTokenState<AfterAttributeNameState>();
						return;
					case Chars.SOLIDUS:
						t.ChangeTokenState<SelfClosingStartTagState>();
						return;
					case Chars.EQUALS_SIGN:
						t.ChangeTokenState<BeforeAttributeValueState>();
						return;
					case Chars.GREATER_THAN_SIGN:
						t.ChangeTokenState<DataState>();
						t.EmitToken();
						return;
					case Chars.NULL:
						t.Parser.OnParseErrorRaised(string.Format("属性名の解析中にNULL文字を検出しました。"));
						t.CurrentTagToken.CurrentAttribute.Name += Chars.REPLACEMENT_CHARACTER;
						return;
					case Chars.QUOTATION_MARK:
					case Chars.APOSTROPHE:
					case Chars.LESS_THAN_SIGN:
						t.Parser.OnParseErrorRaised(string.Format("属性名として不正な文字を検出しました。: {0}", c));
						goto default;
					case null:
						t.Parser.OnParseErrorRaised(string.Format("属性名の解析中に終端に達しました。"));
						t.UnConsume(1);
						t.ChangeTokenState<DataState>();
						return;
					default:{
						if(c.IsLatinCapitalLetter()){
							t.CurrentTagToken.CurrentAttribute.Name += c.ToLower();
						} else {
							t.CurrentTagToken.CurrentAttribute.Name += c;
						}
						return;
					}
				}
			}
		}
	}
}
