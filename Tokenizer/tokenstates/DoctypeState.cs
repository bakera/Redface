using System;

namespace Bakera.RedFace{

	public class DoctypeState : TokenizationState{

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();
			switch(c){
				case Chars.CHARACTER_TABULATION:
				case Chars.LINE_FEED:
				case Chars.FORM_FEED:
				case Chars.SPACE:
					t.ChangeTokenState<BeforeDoctypeNameState>();
					return;
				case null:
					OnMessageRaised(new SuddenlyEndAtAttributeError());
					t.UnConsume(1);
					t.ChangeTokenState<DataState>();
					t.EmitToken(new DoctypeToken(){ForceQuirks = true});
					return;
				default:
					OnMessageRaised(new MissingSpaceBeforeDoctypeIdentifierError());
					t.UnConsume(1);
					t.ChangeTokenState<BeforeDoctypeNameState>();
					return;
			}
		}
	}
}
