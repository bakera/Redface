using System;

namespace Bakera.RedFace{


	public class BogusDoctypeState : TokenizationState{

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();
			switch(c){
				case Chars.GREATER_THAN_SIGN:{
					t.ChangeTokenState<DataState>();
					return;
				}
				case null:
					t.UnConsume(1);
					t.ChangeTokenState<DataState>();
					t.EmitToken();
					return;
				default:
					return;
			}
		}
	}
}
