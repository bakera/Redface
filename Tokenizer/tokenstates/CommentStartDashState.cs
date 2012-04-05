using System;

namespace Bakera.RedFace{

	public class CommentStartDashState : TokenizationState{

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();
			switch(c){
				case Chars.HYPHEN_MINUS:
					t.ChangeTokenState<CommentEndState>();
					return;
				case Chars.NULL:
					OnMessageRaised(new NullInCommentError());
					t.CurrentCommentToken.Append(Chars.HYPHEN_MINUS);
					t.CurrentCommentToken.Append(Chars.REPLACEMENT_CHARACTER);

					t.ChangeTokenState<CommentState>();
					return;
				case Chars.GREATER_THAN_SIGN:
					OnMessageRaised(new LessHyphenCommentError());
					t.EmitToken();
					t.ChangeTokenState<DataState>();
					return;
				case null:
					OnMessageRaised(new SuddenlyEndAtCommentError());
					t.EmitToken();
					t.UnConsume(1);
					t.ChangeTokenState<DataState>();
					return;
				default:
					t.CurrentCommentToken.Append(Chars.HYPHEN_MINUS);
					t.CurrentCommentToken.Append(c);
					t.ChangeTokenState<CommentState>();
					return;
			}
		}
	}
}
