using System;

namespace Bakera.RedFace{

	public class CommentEndState : TokenizationState{

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();
			switch(c){
				case Chars.GREATER_THAN_SIGN:
					t.EmitToken();
					t.ChangeTokenState<DataState>();
					return;
				case Chars.NULL:
					OnMessageRaised(new NullInCommentError());
					t.CurrentCommentToken.Append(Chars.HYPHEN_MINUS);
					t.CurrentCommentToken.Append(Chars.HYPHEN_MINUS);
					t.CurrentCommentToken.Append(Chars.REPLACEMENT_CHARACTER);
					t.ChangeTokenState<CommentState>();
					return;
				case Chars.EXCLAMATION_MARK:
					t.ChangeTokenState<CommentEndBangState>();
					return;
				case Chars.HYPHEN_MINUS:
					OnMessageRaised(new HyphenTooMuchCommentError());
					t.CurrentCommentToken.Append(Chars.HYPHEN_MINUS);
					return;
				case null:
					OnMessageRaised(new SuddenlyEndAtCommentError());
					t.EmitToken();
					t.UnConsume(1);
					t.ChangeTokenState<DataState>();
					return;
				default:
					OnMessageRaised(new DoubleHyphenInCommentError());
					t.CurrentCommentToken.Append(Chars.HYPHEN_MINUS);
					t.CurrentCommentToken.Append(Chars.HYPHEN_MINUS);
					t.CurrentCommentToken.Append(c);
					t.ChangeTokenState<CommentState>();
					return;
			}
		}
	}
}
