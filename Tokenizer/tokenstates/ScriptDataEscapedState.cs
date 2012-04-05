using System;

namespace Bakera.RedFace{

	public class ScriptDataEscapedState : TokenizationState{

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();
			switch(c){
				case Chars.HYPHEN_MINUS:
					t.EmitToken(Chars.HYPHEN_MINUS);
					t.ChangeTokenState<ScriptDataEscapedDashState>();
					return;
				case Chars.LESS_THAN_SIGN:
					t.ChangeTokenState<ScriptDataEscapedLessThanSignState>();
					return;
				case Chars.NULL:
					OnMessageRaised(new NullInDataError());
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
