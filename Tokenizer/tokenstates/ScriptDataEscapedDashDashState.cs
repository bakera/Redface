using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class ScriptDataEscapedDashDashState : TokenizationState{

			public override void Read(Tokenizer t){
				char? c = t.ConsumeChar();
				switch(c){
					case Chars.HYPHEN_MINUS:
						t.EmitToken(Chars.HYPHEN_MINUS);
						return;
					case Chars.LESS_THAN_SIGN:
						t.ChangeTokenState<ScriptDataEscapedLessThanSignState>();
						return;
					case Chars.GREATER_THAN_SIGN:
						t.EmitToken(Chars.GREATER_THAN_SIGN);
						t.ChangeTokenState<ScriptDataState>();
						return;
					case Chars.NULL:
						t.Parser.OnParseErrorRaised(string.Format("NULL文字が検出されました。"));
						t.EmitToken(Chars.REPLACEMENT_CHARACTER);
						t.ChangeTokenState<ScriptDataEscapedState>();
						return;
					case null:
						t.Parser.OnParseErrorRaised(string.Format("script要素の内容を解析中に終端に達しました。"));
						t.UnConsume(1);
						t.ChangeTokenState<DataState>();
						return;
					default:
						t.EmitToken(c);
						t.ChangeTokenState<ScriptDataEscapedState>();
						return;
				}

			}
		}
	}
}
