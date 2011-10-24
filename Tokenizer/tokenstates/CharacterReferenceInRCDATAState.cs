using System;

namespace Bakera.RedFace{

	public class CharacterReferenceInRCDATAState : TokenizationState{
		public override void Read(Tokenizer t){
			t.AdditionalAllowedCharacter = null;
			ReferencedCharacterToken result = ConsumeCharacterReference(t);
			if(result == null){
				t.EmitToken(Chars.AMPERSAND);
			} else {
				t.EmitToken(result);
			}
			t.ChangeTokenState<RCDATAState>();
		}
	}
}

