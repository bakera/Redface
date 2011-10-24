using System;
namespace Bakera.RedFace{

	public class CharacterReferenceInAttributeState : TokenizationState{

		public override void Read(Tokenizer t){
			ReferencedCharacterToken result = ConsumeCharacterReference(t);
			if(result == null){
				t.CurrentTagToken.CurrentAttribute.Value += Chars.AMPERSAND;
			} else {
				t.CurrentTagToken.CurrentAttribute.Value += result.Data;
			}
			t.BackTokenState();
		}
	}
}

