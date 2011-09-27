using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class ScriptDataDoubleEscapeEndState : TokenizationState{

			public override void Read(Tokenizer t){
				char? c = t.ConsumeChar();

				if(c.IsLatinCapitalLetter()){
					t.TemporaryBuffer += c.ToLower();
					t.EmitToken(new CharacterToken(c));
					return;
				} else if(c.IsLatinSmallLetter()){
					t.TemporaryBuffer += c;
					t.EmitToken(new CharacterToken(c));
					return;
 				}

				switch(c){
					case Chars.CHARACTER_TABULATION:
					case Chars.LINE_FEED:
					case Chars.FORM_FEED:
					case Chars.SPACE:
					case Chars.SOLIDUS:
					case Chars.GREATER_THAN_SIGN:
						if(t.TemporaryBuffer.Equals("script", StringComparison.InvariantCulture)){
							t.ChangeTokenState<ScriptDataEscapedState>();
						} else {
							t.ChangeTokenState<ScriptDataDoubleEscapedState>();
						}
						t.EmitToken(new CharacterToken(c));
						return;
					default:
						t.UnConsume(1);
						t.ChangeTokenState<ScriptDataDoubleEscapedState>();
						return;
				}
			}
		}
	}
}
