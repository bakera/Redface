using System;

namespace Bakera.RedFace{

	public class CommentState : TokenizationState{

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();
			switch(c){
				case Chars.HYPHEN_MINUS:
					t.ChangeTokenState<CommentEndDashState>();
					return;
				case Chars.NULL:
					OnMessageRaised(new NullInCommentError());
					t.CurrentCommentToken.Append(Chars.REPLACEMENT_CHARACTER);

					return;
				case null:
					OnMessageRaised(new SuddenlyEndAtCommentError());
					t.EmitToken();
					t.UnConsume(1);
					t.ChangeTokenState<DataState>();
					return;
				default:
					t.CurrentCommentToken.Append(c);
					return;
			}
		}
	}
}
