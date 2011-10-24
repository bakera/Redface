using System;

namespace Bakera.RedFace{

	public class RAWTEXTLessThanSignState : TokenizationState{

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();
			if(c == Chars.SOLIDUS){
				t.TemporaryBuffer = "";
				t.ChangeTokenState<RAWTEXTEndTagOpenState>();
				return;
			}
			t.EmitToken(Chars.LESS_THAN_SIGN);
			t.UnConsume(1);
			t.ChangeTokenState<RAWTEXTState>();
			return;
		}
	}
}
