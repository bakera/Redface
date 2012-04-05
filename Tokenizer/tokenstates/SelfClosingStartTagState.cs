using System;

namespace Bakera.RedFace{

	public class SelfClosingStartTagState : TokenizationState{

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();
			switch(c){
				case Chars.GREATER_THAN_SIGN:
					((TagToken)t.CurrentToken).SelfClosing = true;
					t.EmitToken();
					t.ChangeTokenState<DataState>();
					return;
				case null:
					OnMessageRaised(new SuddenlyEndAtScriptError());
					t.UnConsume(1);
					t.ChangeTokenState<DataState>();
					return;
				default:
					OnMessageRaised(new InvaridAttributeInSelfClosingTagError());
					t.UnConsume(1);
					t.ChangeTokenState<BeforeAttributeNameState>();
					return;
			}
		}
	}
}
