using System;

namespace Bakera.RedFace{

	public class ScriptDataEscapedState : TokenizationState{

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();
			switch(c){
				case Chars.HYPHEN_MINUS:
					t.EmitToken(Chars.HYPHEN_MINUS);
					t.ChangeTokenState<ScriptDataEscapedDashState>();
					return;
				case Chars.LESS_THAN_SIGN:
					t.ChangeTokenState<ScriptDataEscapedLessThanSignState>();
					return;
				case Chars.NULL:
					OnParseErrorRaised(string.Format("NULL文字が検出されました。"));
					t.EmitToken(Chars.REPLACEMENT_CHARACTER);
					return;
				case null:
					OnParseErrorRaised(string.Format("script要素の内容を解析中に終端に達しました。"));
					t.UnConsume(1);
					t.ChangeTokenState<DataState>();
					return;
				default:
					t.EmitToken(c);
					return;
			}

		}
	}
}
