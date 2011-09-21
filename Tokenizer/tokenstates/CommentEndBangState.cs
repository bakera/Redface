using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class CommentEndBangState : TokenizationState{

			public override void Read(Tokenizer t){
				char? c = t.ConsumeChar();
				switch(c){
					case Chars.HYPHEN_MINUS:
						t.CurrentCommentToken.Append(Chars.HYPHEN_MINUS);
						t.CurrentCommentToken.Append(Chars.HYPHEN_MINUS);
						t.CurrentCommentToken.Append(Chars.EXCLAMATION_MARK);
						t.ChangeTokenState<CommentEndDashState>();
						return;
					case Chars.GREATER_THAN_SIGN:
						t.EmitToken();
						t.ChangeTokenState<DataState>();
						return;
					case Chars.NULL:
						t.Parser.OnParseErrorRaised(string.Format("コメント終了区切り子の後にNUL文字が出現しました。"));
						t.CurrentCommentToken.Append(Chars.HYPHEN_MINUS);
						t.CurrentCommentToken.Append(Chars.HYPHEN_MINUS);
						t.CurrentCommentToken.Append(Chars.EXCLAMATION_MARK);
						t.CurrentCommentToken.Append(Chars.REPLACEMENT_CHARACTER);
						t.ChangeTokenState<CommentState>();
						return;
					case null:
						t.Parser.OnParseErrorRaised(string.Format("コメント終了区切り子の後に終端に達しました。"));
						t.EmitToken();
						t.UnConsume(1);
						t.ChangeTokenState<DataState>();
						return;
					default:
						t.CurrentCommentToken.Append(Chars.HYPHEN_MINUS);
						t.CurrentCommentToken.Append(Chars.HYPHEN_MINUS);
						t.CurrentCommentToken.Append(Chars.EXCLAMATION_MARK);
						t.CurrentCommentToken.Append(c);
						t.ChangeTokenState<CommentState>();
						return;
				}
			}
		}
	}
}
