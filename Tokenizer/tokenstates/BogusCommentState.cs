using System;

namespace Bakera.RedFace{

	public class BogusCommentState : TokenizationState{

/*
Consume every character up to and including the first U+003E GREATER-THAN SIGN character (>) or the end of the file (EOF), whichever comes first. Emit a comment token whose data is the concatenation of all the characters starting from and including the character that caused the state machine to switch into the bogus comment state, up to and including the character immediately before the last consumed character (i.e. up to the character just before the U+003E or EOF character), but with any U+0000 NULL characters replaced by U+FFFD REPLACEMENT CHARACTER characters. (If the comment was started by the end of the file (EOF), the token is empty.)

Switch to the data state.

If the end of the file was reached, reconsume the EOF character.
*/

		public override void Read(Tokenizer t){
			if(!(t.CurrentToken is CommentToken)){
				t.CurrentToken = new CommentToken();
				t.CurrentCommentToken.Append(t.CurrentInputChar);
			}
			char? c = t.ConsumeChar();
			switch(c){
				case Chars.GREATER_THAN_SIGN:{
					t.EmitToken();
					t.ChangeTokenState<DataState>();
					return;
				}
				case null:
					t.UnConsume(1);
					t.EmitToken();
					t.ChangeTokenState<DataState>();
					return;
				default:
					t.CurrentCommentToken.Append(c);
					return;
			}
		}
	}
}
