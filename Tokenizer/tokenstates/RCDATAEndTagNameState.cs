using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class RCDATAEndTagNameState : TokenizationState{

			public override void Read(Tokenizer t){
				char? c = t.ConsumeChar();

				switch(c){
					case Chars.CHARACTER_TABULATION:
					case Chars.LINE_FEED:
					case Chars.FORM_FEED:
					case Chars.SPACE:
						if(t.IsAppropriateEndTagToken){
							t.ChangeTokenState<BeforeAttributeNameState>();
							return;
						}
						goto default;
					case Chars.SOLIDUS:
						if(t.IsAppropriateEndTagToken){
							t.ChangeTokenState<SelfClosingStartTagState>();
							return;
						}
						goto default;
					case Chars.GREATER_THAN_SIGN:
						if(t.IsAppropriateEndTagToken){
							t.EmitToken();
							t.ChangeTokenState<DataState>();
							return;
						}
						goto default;
					default:{
						t.EmitToken(new CharacterToken("</" + t.TemporaryBuffer));
						t.UnConsume(1);
						t.ChangeTokenState<RCDATAState>();
						return;
					}
				}
			}
		}
	}
}
