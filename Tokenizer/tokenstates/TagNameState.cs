using System;

namespace Bakera.RedFace{

	public class TagNameState : TokenizationState{

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
				case Chars.NULL:
					OnMessageRaised(new NullInElementNameError());
					t.CurrentTagToken.Name += Chars.REPLACEMENT_CHARACTER;
					return;
				case null:
					OnMessageRaised(new SuddenlyEndAtTagError());
					t.UnConsume(1);
					t.ChangeTokenState<DataState>();
					return;
				default:{
					TagToken result = (TagToken)t.CurrentToken;
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
