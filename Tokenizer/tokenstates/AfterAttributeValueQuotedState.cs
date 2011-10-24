using System;

namespace Bakera.RedFace{

	public class AfterAttributeValueQuotedState : TokenizationState{

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();

			switch(c){
				case Chars.CHARACTER_TABULATION:
				case Chars.LINE_FEED:
				case Chars.FORM_FEED:
				case Chars.SPACE:
					t.ChangeTokenState<BeforeAttributeNameState>();
					return;
				case Chars.SOLIDUS:
					t.ChangeTokenState<SelfClosingStartTagState>();
					return;
				case Chars.GREATER_THAN_SIGN:
					t.ChangeTokenState<DataState>();
					t.EmitToken();
					return;
				case null:
					OnParseErrorRaised(string.Format("属性値の解析中に終端に達しました。"));
					t.UnConsume(1);
					t.ChangeTokenState<DataState>();
					return;
				default:
					OnParseErrorRaised(string.Format("属性値の解析中に不明な文字を検出しました。: {0}", c));
					t.UnConsume(1);
					t.ChangeTokenState<BeforeAttributeNameState>();
					return;
			}
		}
	}
}
