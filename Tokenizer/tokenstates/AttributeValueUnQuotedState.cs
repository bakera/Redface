using System;

namespace Bakera.RedFace{

	public class AttributeValueUnQuotedState : TokenizationState{

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();

			switch(c){
				case Chars.CHARACTER_TABULATION:
				case Chars.LINE_FEED:
				case Chars.FORM_FEED:
				case Chars.SPACE:
					t.ChangeTokenState<BeforeAttributeNameState>();
					return;
				case Chars.AMPERSAND:
					t.AdditionalAllowedCharacter = Chars.GREATER_THAN_SIGN;
					t.ChangeTokenState<CharacterReferenceInAttributeState>();
					return;
				case Chars.GREATER_THAN_SIGN:
					t.ChangeTokenState<DataState>();
					t.EmitToken();
					return;
				case Chars.NULL:
					OnParseErrorRaised(string.Format("属性値にNUL文字が含まれています。"));
					t.CurrentTagToken.CurrentAttribute.Value += Chars.REPLACEMENT_CHARACTER;
					return;
				case Chars.QUOTATION_MARK:
				case Chars.APOSTROPHE:
				case Chars.LESS_THAN_SIGN:
				case Chars.EQUALS_SIGN:
				case Chars.GRAVE_ACCENT:
					OnParseErrorRaised(string.Format("引用符でくくられていない属性値の中に出現できない文字が出現しました。: {0}", c));
					goto default;
				case null:
					OnParseErrorRaised(string.Format("属性値の解析中に終端に達しました。"));
					t.UnConsume(1);
					t.ChangeTokenState<DataState>();
					return;
				default:
					t.CurrentTagToken.CurrentAttribute.Value += c;
					return;
			}
		}

	}
}
