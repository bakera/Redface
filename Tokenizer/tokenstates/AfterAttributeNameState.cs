using System;

namespace Bakera.RedFace{

		public class AfterAttributeNameState : TokenizationState{

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();

			switch(c){
				case Chars.CHARACTER_TABULATION:
				case Chars.LINE_FEED:
				case Chars.FORM_FEED:
				case Chars.SPACE:
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
					if(t.CurrentTagToken.FixAttribute() == false){
						OnMessageRaised(new DuplicateAttributeError(t.CurrentTagToken.CurrentAttribute.Name));
						t.CurrentTagToken.CurrentAttribute = null;
					}
					OnMessageRaised(new NullInAttributeNameError());
					t.CurrentTagToken.CreateAttribute(Chars.REPLACEMENT_CHARACTER, "");
					return;
				case Chars.QUOTATION_MARK:
				case Chars.APOSTROPHE:
				case Chars.LESS_THAN_SIGN:
					OnMessageRaised(new InvaridCharAtAfterAttributeNameError(c,  t.CurrentTagToken.CurrentAttribute.Name));
					goto default;
				case null:
					OnMessageRaised(new SuddenlyEndAtAttributeError());
					t.UnConsume(1);
					t.ChangeTokenState<DataState>();
					return;
				default:
					if(t.CurrentTagToken.FixAttribute() == false){
						OnMessageRaised(new DuplicateAttributeError(t.CurrentTagToken.CurrentAttribute.Name));
						t.CurrentTagToken.CurrentAttribute = null;
					}
					if(c.IsLatinCapitalLetter()){
						t.CurrentTagToken.CreateAttribute(c.ToLower(), "");
					} else {
						t.CurrentTagToken.CreateAttribute(c, "");
					}
					t.ChangeTokenState<AttributeNameState>();
					return;
			}
		}
	}
}
