using System;

namespace Bakera.RedFace{

	public class DataState : TokenizationState{

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();
			switch(c){
				case Chars.AMPERSAND:
					t.ChangeTokenState<CharacterReferenceInDataState>();
					break;
				case Chars.LESS_THAN_SIGN:
					t.ChangeTokenState<TagOpenState>();
					break;
				case Chars.NULL:
					OnMessageRaised(new NullInDataError());
					t.EmitToken(c);
					return;
				case null:
					t.EmitToken(new EndOfFileToken());
					return;
				default:
					t.EmitToken(c);
					return;
			}
		}

	}
}
