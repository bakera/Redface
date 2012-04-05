using System;
using System.IO;

namespace Bakera.RedFace{

	public class ScriptDataEscapedDashDashState : TokenizationState{

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();
			switch(c){
				case Chars.HYPHEN_MINUS:
					t.EmitToken(Chars.HYPHEN_MINUS);
					return;
				case Chars.LESS_THAN_SIGN:
					t.ChangeTokenState<ScriptDataEscapedLessThanSignState>();
					return;
				case Chars.GREATER_THAN_SIGN:
					t.EmitToken(Chars.GREATER_THAN_SIGN);
					t.ChangeTokenState<ScriptDataState>();
					return;
				case Chars.NULL:
					OnMessageRaised(new NullInScriptError());
					t.EmitToken(Chars.REPLACEMENT_CHARACTER);
					t.ChangeTokenState<ScriptDataEscapedState>();
					return;
				case null:
					OnMessageRaised(new SuddenlyEndAtScriptError());

					t.UnConsume(1);
					t.ChangeTokenState<DataState>();
					return;
				default:
					t.EmitToken(c);
					t.ChangeTokenState<ScriptDataEscapedState>();
					return;
			}

		}
	}
}
