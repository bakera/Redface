using System;

namespace Bakera.RedFace{

	public class MarkupDeclarationOpenState : TokenizationState{

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();
			if(IsStringMatch(t, CommentId)){
				t.CurrentToken = new CommentToken();
				t.ChangeTokenState<CommentStartState>();
				return;
			}
			if(IsStringMatch(t, DoctypeId)){
				t.ChangeTokenState<DoctypeState>();
				return;
			}
			if(IsStringMatchCaseSensitive(t, CDATASectionStartId)){
				t.ChangeTokenState<CDATASectionState>();
				return;
			}
			OnMessageRaised(new UnknownMarkupDeclarationError());
			t.ChangeTokenState<BogusCommentState>();
			return;
		}
	}
}
