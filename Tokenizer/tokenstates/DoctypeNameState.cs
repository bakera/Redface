using System;
namespace Bakera.RedFace{

	public class DoctypeNameState : TokenizationState{

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();

			switch(c){
				case Chars.CHARACTER_TABULATION:
				case Chars.LINE_FEED:
				case Chars.FORM_FEED:
				case Chars.SPACE:
					t.ChangeTokenState<AfterDoctypeNameState>();
					return;
				case Chars.GREATER_THAN_SIGN:
					t.ChangeTokenState<DataState>();
					t.EmitToken();
					return;
				case Chars.NULL:
					OnMessageRaised(new NullInDoctypeError());
					((DoctypeToken)t.CurrentToken).Name += Chars.REPLACEMENT_CHARACTER;
					return;
				case null:
					OnMessageRaised(new SuddenlyEndAtDoctypeError());
					((DoctypeToken)t.CurrentToken).ForceQuirks = true;
					t.UnConsume(1);
					t.ChangeTokenState<DataState>();
					t.EmitToken();
					return;
				default:{
					DoctypeToken result = (DoctypeToken)t.CurrentToken;
					if(c.IsLatinCapitalLetter()){
						result.Name += c.ToLower();
					} else {
						result.Name += c;
					}
					return;
				}
			}
		}
	}
}
