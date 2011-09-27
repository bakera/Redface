using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class ScriptDataDoubleEscapedState : TokenizationState{

			public override void Read(Tokenizer t){
				char? c = t.ConsumeChar();

				switch(c){
					case Chars.HYPHEN_MINUS:
					// ToDo:
//						t.ChangeTokenState<ScriptDataDoubleEscapedDashState>();
						t.EmitToken(new CharacterToken(Chars.HYPHEN_MINUS));
						return;
					case Chars.LESS_THAN_SIGN:
					// ToDo:
//						t.ChangeTokenState<ScriptDataDoubleEscapedLessThanSignState>();
						t.EmitToken(new CharacterToken(Chars.LESS_THAN_SIGN));
						return;
					case Chars.NULL:
						t.Parser.OnParseErrorRaised(string.Format("NULL文字が検出されました。"));
						t.EmitToken(new CharacterToken(Chars.REPLACEMENT_CHARACTER));
						return;
					case null:
						t.Parser.OnParseErrorRaised(string.Format("script要素の内容を解析中に終端に達しました。"));
						t.UnConsume(1);
						t.ChangeTokenState<DataState>();
						return;
					default:
						t.EmitToken(new CharacterToken(c));
						return;
				}
			}
		}
	}
}
