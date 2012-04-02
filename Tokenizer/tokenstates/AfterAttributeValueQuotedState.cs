using System;

namespace Bakera.RedFace{

	public class AfterAttributeValueQuotedState : TokenizationState{

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();

			switch(c){
				case Chars.CHARACTER_TABULATION:
				case Chars.LINE_FEED:
				case Chars.FORM_FEED:
				case Chars.SPACE:
					t.ChangeTokenState<BeforeAttributeNameState>();
					return;
				case Chars.SOLIDUS:
					t.ChangeTokenState<SelfClosingStartTagState>();
					return;
				case Chars.GREATER_THAN_SIGN:
					t.ChangeTokenState<DataState>();
					t.EmitToken();
					return;
				case null:
					OnMessageRaised(new SuddenlyEndAtAttributeError());
					t.UnConsume(1);
					t.ChangeTokenState<DataState>();
					return;
				default:
					OnMessageRaised(new MissingSpaceAfterAttributeValueError(c));
					t.UnConsume(1);
					t.ChangeTokenState<BeforeAttributeNameState>();
					return;
			}
		}
	}
}
