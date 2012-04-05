using System;

namespace Bakera.RedFace{

	public class RCDATAState : TokenizationState{

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();
			switch(c){
				case Chars.AMPERSAND:
					t.ChangeTokenState<CharacterReferenceInRCDATAState>();
					break;
				case Chars.LESS_THAN_SIGN:
					t.ChangeTokenState<RCDATALessThanSignState>();
					break;
				case Chars.NULL:
					OnMessageRaised(new NullInDataError());
					t.EmitToken(Chars.REPLACEMENT_CHARACTER);
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
