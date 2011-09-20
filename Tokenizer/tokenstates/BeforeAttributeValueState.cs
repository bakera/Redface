using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class BeforeAttributeValueState : TokenizationState{

			public override void Read(Tokenizer t){
				char? c = t.ConsumeChar();

				switch(c){
					case Chars.CHARACTER_TABULATION:
					case Chars.LINE_FEED:
					case Chars.FORM_FEED:
					case Chars.SPACE:
						return;
					case Chars.QUOTATION_MARK:
						t.ChangeTokenState<AttributeValueState<DoubleQuoted>>();
						return;
					case Chars.AMPERSAND:
						t.ChangeTokenState<AttributeValueUnQuotedState>();
						return;
					case Chars.APOSTROPHE:
						t.ChangeTokenState<AttributeValueState<SingleQuoted>>();
						return;
					case Chars.NULL:
						t.Parser.OnParseErrorRaised(string.Format("属性値にNUL文字が含まれています。"));
						t.CurrentTagToken.CurrentAttribute.Value += Chars.REPLACEMENT_CHARACTER;
						t.ChangeTokenState<AttributeValueUnQuotedState>();
						return;
					case Chars.GREATER_THAN_SIGN:
						t.Parser.OnParseErrorRaised(string.Format("属性値がありません。"));
						t.ChangeTokenState<DataState>();
						t.EmitToken();
						return;
					case Chars.LESS_THAN_SIGN:
					case Chars.EQUALS_SIGN:
					case Chars.GRAVE_ACCENT:
						t.Parser.OnParseErrorRaised(string.Format("属性値に不正な文字を検出しました。: {0}", c));
						goto default;
					case null:
						t.Parser.OnParseErrorRaised(string.Format("属性値の解析中に終端に達しました。"));
						t.UnConsume(1);
						t.ChangeTokenState<DataState>();
						return;
					default:
						t.CurrentTagToken.CurrentAttribute.Value += c;
						t.ChangeTokenState<AttributeValueUnQuotedState>();
						return;
				}
			}
		}
	}
}
