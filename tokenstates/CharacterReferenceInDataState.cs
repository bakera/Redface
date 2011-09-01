using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class CharacterReferenceInDataState : TokenState{

			public CharacterReferenceInDataState(RedFaceParser p) : base(p){}

			public override void Read(){
				Parser.ConsumeChar();
				Parser.SaveUnConsumePosition();
				string result = ConsumeCharacterReference();
				if(result == null){
					Parser.UnConsume();
					Parser.Emit("\u0023");
				} else {
					Parser.Emit(result);
				}
				Parser.ChangeTokenState(typeof(DataState));
			}
		}
	}

}
