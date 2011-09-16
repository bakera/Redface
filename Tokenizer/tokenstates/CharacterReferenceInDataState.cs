using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class CharacterReferenceInDataState : TokenizationState{

			public override void Read(Tokenizer t){
				ReferencedCharacterToken result = ConsumeCharacterReference(t);
				t.ChangeTokenState<DataState>();
				if(result == null){
					t.EmitToken(new CharacterToken(Chars.AMPERSAND));
				} else {
					t.EmitToken(result);
				}
			}
		}
	}

}
