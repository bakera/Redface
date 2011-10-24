using System;

namespace Bakera.RedFace{

	public class ScriptDataDoubleEscapedDashDashState : TokenizationState{

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();
			switch(c){
				case Chars.HYPHEN_MINUS:
					t.EmitToken(Chars.HYPHEN_MINUS);
					return;
				case Chars.LESS_THAN_SIGN:
					t.EmitToken(Chars.LESS_THAN_SIGN);
					t.ChangeTokenState<ScriptDataDoubleEscapedLessThanSignState>();
					return;
				case Chars.GREATER_THAN_SIGN:
					t.EmitToken(Chars.GREATER_THAN_SIGN);
					t.ChangeTokenState<ScriptDataState>();
					return;
				case Chars.NULL:
					OnParseErrorRaised(string.Format("NULL文字が検出されました。"));
					t.EmitToken(Chars.REPLACEMENT_CHARACTER);
					t.ChangeTokenState<ScriptDataDoubleEscapedState>();
					return;
				case null:
					OnParseErrorRaised(string.Format("script要素の内容を解析中に終端に達しました。"));
					t.UnConsume(1);
					t.ChangeTokenState<DataState>();
					return;
				default:
					t.EmitToken(c);
					t.ChangeTokenState<ScriptDataDoubleEscapedState>();
					return;
			}

		}
	}
}
