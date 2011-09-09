using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class CharacterReferenceInDataState : TokenState{

			public CharacterReferenceInDataState(Tokenizer t) : base(t){}

			public override Token Read(){
				string result = ConsumeCharacterReference();
				ChangeTokenState(typeof(DataState));
				if(result == null){
					return new CharacterToken(Chars.AMPERSAND);
				} else {
					return new CharacterToken(result);
				}
			}
		}
	}

}
