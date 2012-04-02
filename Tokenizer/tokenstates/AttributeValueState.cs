using System;

namespace Bakera.RedFace{

	public class AttributeValueState<T> : TokenizationState where T : Quoted, new(){

		public override void Read(Tokenizer t){

			char? c = t.ConsumeChar();
			char quote = (new T()).Quote;

			if(c == quote){
				t.ChangeTokenState<AfterAttributeValueQuotedState>();
				return;
			}

			switch(c){
				case Chars.AMPERSAND:
					t.AdditionalAllowedCharacter = quote;
					t.ChangeTokenState<CharacterReferenceInAttributeState>();
					return;
				case Chars.NULL:
					OnMessageRaised(new NullInAttributeValueError());
					t.CurrentTagToken.CurrentAttribute.Value += Chars.REPLACEMENT_CHARACTER;
					return;
				case null:
					OnMessageRaised(new SuddenlyEndAtAttributeError());
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
