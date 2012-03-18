using System;

namespace Bakera.RedFace{

	public class TagOpenState : TokenizationState{


		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();
			switch(c){
				case Chars.EXCLAMATION_MARK:
					t.ChangeTokenState<MarkupDeclarationOpenState>();
					return;
				case Chars.SOLIDUS:
					t.ChangeTokenState<EndTagOpenState>();
					return;
				case Chars.QUESTION_MARK:
					OnMessageRaised(new ProcessingInstructionError());
					t.ChangeTokenState<BogusCommentState>();
					return;
			}
			if(c.IsLatinCapitalLetter()){
				t.CurrentToken = new StartTagToken(){Name = c.ToLower().ToString()};
				t.ChangeTokenState<TagNameState>();
				return;
			} else if(c.IsLatinSmallLetter()){
				t.CurrentToken = new StartTagToken(){Name = c.ToString()};
				t.ChangeTokenState<TagNameState>();
				return;
			}
			OnParseErrorRaised(string.Format("LESS THAN SIGNの後の文字がTag Nameではありません。"));
			t.UnConsume(1);
			t.ChangeTokenState<DataState>();
			t.EmitToken(Chars.LESS_THAN_SIGN);
			return;
		}
	}
}

