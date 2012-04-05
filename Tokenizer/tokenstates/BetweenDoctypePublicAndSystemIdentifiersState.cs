using System;

namespace Bakera.RedFace{

	public class BetweenDoctypePublicAndSystemIdentifiersState : TokenizationState{

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();

			switch(c){
				case Chars.CHARACTER_TABULATION:
				case Chars.LINE_FEED:
				case Chars.FORM_FEED:
				case Chars.SPACE:
					return;
				case Chars.GREATER_THAN_SIGN:
					t.ChangeTokenState<DataState>();
					t.EmitToken();
					return;
				case Chars.QUOTATION_MARK:
					((DoctypeToken)t.CurrentToken).SystemIdentifier = "";
					t.ChangeTokenState<DoctypeSystemIdentifierState<DoubleQuoted>>();
					return;
				case Chars.APOSTROPHE:
					((DoctypeToken)t.CurrentToken).SystemIdentifier = "";
					t.ChangeTokenState<DoctypeSystemIdentifierState<SingleQuoted>>();
					return;
				case null:
					OnMessageRaised(new SuddenlyEndAtDoctypeError());
					((DoctypeToken)t.CurrentToken).ForceQuirks = true;
					t.UnConsume(1);
					t.ChangeTokenState<DataState>();
					t.EmitToken();
					return;
				default:
					OnMessageRaised(new UnknownIdentifierInDoctypeError());
					((DoctypeToken)t.CurrentToken).ForceQuirks = true;
					t.ChangeTokenState<BogusDoctypeState>();
					return;
			}
		}
	}
}
