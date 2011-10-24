using System;

namespace Bakera.RedFace{

	public class CDATASectionState : TokenizationState{

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();
			switch(c){
				case Chars.RIGHT_SQUARE_BRACKET:
					if(IsStringMatch(t, CDATASectionEndId)){
						t.ChangeTokenState<DataState>();
						return;
					}
					break;
				case null:
					t.UnConsume(1);
					t.ChangeTokenState<DataState>();
					return;
			}
			t.EmitToken(c);
		}

	}
}
