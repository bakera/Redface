using System;

namespace Bakera.RedFace{

	public class ScriptDataDoubleEscapedState : TokenizationState{

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();

			switch(c){
				case Chars.HYPHEN_MINUS:
					t.ChangeTokenState<ScriptDataDoubleEscapedDashState>();
					t.EmitToken(Chars.HYPHEN_MINUS);
					return;
				case Chars.LESS_THAN_SIGN:
					t.ChangeTokenState<ScriptDataDoubleEscapedLessThanSignState>();
					t.EmitToken(Chars.LESS_THAN_SIGN);
					return;
				case Chars.NULL:
					OnMessageRaised(new NullInScriptError());
					t.EmitToken(Chars.REPLACEMENT_CHARACTER);
					return;
				case null:
					OnMessageRaised(new SuddenlyEndAtScriptError());

					t.UnConsume(1);
					t.ChangeTokenState<DataState>();
					return;
				default:
					t.EmitToken(c);
					return;
			}
		}
	}
}
