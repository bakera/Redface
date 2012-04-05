using System;

namespace Bakera.RedFace{

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
					OnMessageRaised(new NullInAttributeValueError());
					t.CurrentTagToken.CurrentAttribute.Value += Chars.REPLACEMENT_CHARACTER;
					t.ChangeTokenState<AttributeValueUnQuotedState>();
					return;
				case Chars.GREATER_THAN_SIGN:
					OnMessageRaised(new MissingAttributeValueError());
					t.ChangeTokenState<DataState>();
					t.EmitToken();
					return;
				case Chars.LESS_THAN_SIGN:
				case Chars.EQUALS_SIGN:
				case Chars.GRAVE_ACCENT:
					OnMessageRaised(new InvalidCharInUnquotedAttributeValueError(c));
					goto default;
				case null:
					OnMessageRaised(new SuddenlyEndAtAttributeError());
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
