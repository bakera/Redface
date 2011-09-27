using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class ScriptDataEscapeStartDashState : TokenizationState{

			public override void Read(Tokenizer t){
				char? c = t.ConsumeChar();
				switch(c){
					case Chars.HYPHEN_MINUS:
						t.EmitToken(new CharacterToken(Chars.HYPHEN_MINUS));
						t.ChangeTokenState<ScriptDataEscapedDashDashState>();
						return;
					default:
						t.UnConsume(1);
						t.ChangeTokenState<ScriptDataState>();
						return;
				}

			}
		}
	}
}
