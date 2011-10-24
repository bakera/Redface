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
					OnParseErrorRaised(string.Format("コメント終了区切り子の後にNUL文字が出現しました。"));
					t.CurrentCommentToken.Append(Chars.HYPHEN_MINUS);
					t.CurrentCommentToken.Append(Chars.HYPHEN_MINUS);
					t.CurrentCommentToken.Append(Chars.REPLACEMENT_CHARACTER);
					t.ChangeTokenState<CommentState>();
					return;
				case Chars.EXCLAMATION_MARK:
					t.ChangeTokenState<CommentEndBangState>();
					return;
				case Chars.HYPHEN_MINUS:
					OnParseErrorRaised(string.Format("コメント終了区切り子の後に - が出現しました。"));
					t.CurrentCommentToken.Append(Chars.HYPHEN_MINUS);
					return;
				case null:
					OnParseErrorRaised(string.Format("コメント終了区切り子の後に終端に達しました。"));
					t.EmitToken();
					t.UnConsume(1);
					t.ChangeTokenState<DataState>();
					return;
				default:
					OnParseErrorRaised(string.Format("コメント終了区切り子の後に文字が出現しました。"));
					t.CurrentCommentToken.Append(Chars.HYPHEN_MINUS);
					t.CurrentCommentToken.Append(Chars.HYPHEN_MINUS);
					t.CurrentCommentToken.Append(c);
					t.ChangeTokenState<CommentState>();
					return;
			}
		}
	}
}
