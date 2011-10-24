using System;

namespace Bakera.RedFace{

	public class DoctypeState : TokenizationState{

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();
			switch(c){
				case Chars.CHARACTER_TABULATION:
				case Chars.LINE_FEED:
				case Chars.FORM_FEED:
				case Chars.SPACE:
					t.ChangeTokenState<BeforeDoctypeNameState>();
					return;
				case null:
					OnParseErrorRaised(string.Format("DOCTYPEの解析中に終端に達しました。"));
					t.UnConsume(1);
					t.ChangeTokenState<DataState>();
					t.EmitToken(new DoctypeToken(){ForceQuirks = true});
					return;
				default:
					OnParseErrorRaised(string.Format("文書型宣言のDOCTYPEの後ろにスペースがありません。出現した文字: {0}", c));
					t.UnConsume(1);
					t.ChangeTokenState<BeforeDoctypeNameState>();
					return;
			}
		}
	}
}
