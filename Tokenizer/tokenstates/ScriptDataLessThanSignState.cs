using System;

namespace Bakera.RedFace{

	public class ScriptDataLessThanSignState : TokenizationState{

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();
			if(c == Chars.SOLIDUS){
				t.TemporaryBuffer = "";
				t.ChangeTokenState<ScriptDataEndTagOpenState>();
				return;
			}
			if(c == Chars.EXCLAMATION_MARK){
				t.EmitToken(Chars.LESS_THAN_SIGN);
				t.EmitToken(Chars.EXCLAMATION_MARK);
				t.ChangeTokenState<ScriptDataEscapeStartState>();
				return;
			}
			t.EmitToken(Chars.LESS_THAN_SIGN);
			t.UnConsume(1);
			t.ChangeTokenState<ScriptDataState>();
			return;
		}
	}
}
