using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class CharacterReferenceInDataState : TokenizationState{

			public override Token Read(Tokenizer t){
				string result = ConsumeCharacterReference(t);
				t.ChangeTokenState<DataState>();
				if(result == null){
					return new CharacterToken(Chars.AMPERSAND);
				} else {
					return new CharacterToken(result);
				}
			}
		}
	}

}
