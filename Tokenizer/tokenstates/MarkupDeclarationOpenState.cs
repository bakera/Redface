using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class MarkupDeclarationOpenState : TokenizationState{

			public override void Read(Tokenizer t){
				char? c = t.ConsumeChar();
				if(IsStringMatch(t, CommentId)){
					// ToDo: Comment
					t.CurrentToken = new CommentToken();
					t.ChangeTokenState<CommentStartState>();
					return;
				}
				if(IsStringMatch(t, DoctypeId)){
					t.ChangeTokenState<DoctypeState>();
					return;
				}
				if(IsStringMatchCaseSensitive(t, CDATASectionId)){
					// toDo: CDATA;
					return;
				}
				t.Parser.OnParseErrorRaised(string.Format("マーク宣言開始区切り子の後に識別子でない文字が出現しました。"));
				t.ChangeTokenState<BogusCommentState>();
				return;
			}
		}
	}
}
