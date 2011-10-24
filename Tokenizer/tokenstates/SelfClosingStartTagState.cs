using System;

namespace Bakera.RedFace{

	public class SelfClosingStartTagState : TokenizationState{

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();
			switch(c){
				case Chars.GREATER_THAN_SIGN:
					((TagToken)t.CurrentToken).SelfClosing = true;
					t.EmitToken();
					t.ChangeTokenState<DataState>();
					return;
				case null:
					OnParseErrorRaised(string.Format("空要素タグの解析中に終端に達しました。"));
					t.UnConsume(1);
					t.ChangeTokenState<DataState>();
					return;
				default:
					OnParseErrorRaised(string.Format("タグの解析中に / に続いて不明な文字を検出しました。"));
					t.UnConsume(1);
					t.ChangeTokenState<BeforeAttributeNameState>();
					return;
			}
		}
	}
}
