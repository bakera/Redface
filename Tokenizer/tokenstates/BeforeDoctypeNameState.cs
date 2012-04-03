using System;

namespace Bakera.RedFace{

	public class BeforeDoctypeNameState : TokenizationState{

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();

			switch(c){
				case Chars.CHARACTER_TABULATION:
				case Chars.LINE_FEED:
				case Chars.FORM_FEED:
				case Chars.SPACE:
					return;
				case Chars.NULL:
					OnMessageRaised(new NullInDoctypeError());
					t.ChangeTokenState<DoctypeNameState>();
					t.CurrentToken = new DoctypeToken(){Name = Chars.REPLACEMENT_CHARACTER.ToString()};
					return;
				case Chars.GREATER_THAN_SIGN:
					OnMessageRaised(new EmptyDoctypeError());
					t.ChangeTokenState<DataState>();
					t.EmitToken(new DoctypeToken(){ForceQuirks = true});
					return;
				case null:{
					OnMessageRaised(new SuddenlyEndAtDoctypeError());
					t.UnConsume(1);
					t.ChangeTokenState<DataState>();
					t.EmitToken(new DoctypeToken(){ForceQuirks = true});
					return;
				}
				default:{
					DoctypeToken result = new DoctypeToken();
					if(c.IsLatinCapitalLetter()){
						result.Name = c.ToLower().ToString();
					} else {
						result.Name = c.ToString();
					}
					t.CurrentToken = result;
					t.ChangeTokenState<DoctypeNameState>();
					return;
				}
			}
		}

	}
}
