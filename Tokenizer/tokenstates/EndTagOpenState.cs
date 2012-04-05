using System;

namespace Bakera.RedFace{

	public class EndTagOpenState : TokenizationState{


		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();
			switch(c){
				case Chars.GREATER_THAN_SIGN:
					OnMessageRaised(new EmptyEndTagError());
					t.ChangeTokenState<DataState>();
					return;
				case null:
					OnMessageRaised(new SuddenlyEndAtTagError());
					t.EmitToken(Chars.LESS_THAN_SIGN);
					t.EmitToken(Chars.SOLIDUS);
					t.UnConsume(1);
					t.ChangeTokenState<DataState>();
					return;
			}
			if(c.IsLatinCapitalLetter()){
				t.CurrentToken = new EndTagToken(){Name = c.ToLower().ToString()};
				t.ChangeTokenState<TagNameState>();
				return;
			} else if(c.IsLatinSmallLetter()){
				t.CurrentToken = new EndTagToken(){Name = c.ToString()};
				t.ChangeTokenState<TagNameState>();
				return;
			}
			OnMessageRaised(new UnknownEndTagError());
			t.ChangeTokenState<BogusCommentState>();
			return;
		}
	}
}
