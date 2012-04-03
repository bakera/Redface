using System;
using System.IO;

namespace Bakera.RedFace{

	public class AfterDoctypeNameState : TokenizationState{

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
				case null:{
					OnMessageRaised(new SuddenlyEndAtDoctypeError());
					DoctypeToken result = (DoctypeToken)t.CurrentToken;
					result.ForceQuirks = true;
					t.UnConsume(1);
					t.ChangeTokenState<DataState>();
					t.EmitToken();
					return;
				}
				default:
					if(IsStringMatch(t, DoctypePublicId)){
						t.ChangeTokenState<AfterDoctypePublicKeywordState>();
						return;
					}
					if(IsStringMatch(t, DoctypeSystemId)){
						t.ChangeTokenState<AfterDoctypeSystemKeywordState>();
						return;
					} else {
						OnMessageRaised(new UnknownIdentifierInDoctypeError());
						((DoctypeToken)t.CurrentToken).ForceQuirks = true;
						t.ChangeTokenState<BogusDoctypeState>();
						return;
					}
			}
		}
	}
}
