using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class RCDATALessThanSignState : TokenizationState{


			public override void Read(Tokenizer t){
				char? c = t.ConsumeChar();
				if(c == Chars.SOLIDUS){
					t.TemporaryBuffer = "";
					t.ChangeTokenState<RCDATAEndTagOpenState>();
					return;
				}
				t.EmitToken(new CharacterToken(Chars.LESS_THAN_SIGN));
				return;
			}
		}
	}
}
