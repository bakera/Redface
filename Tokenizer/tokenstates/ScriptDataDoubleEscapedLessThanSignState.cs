using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class ScriptDataDoubleEscapedLessThanSignState : TokenizationState{

			public override void Read(Tokenizer t){
				char? c = t.ConsumeChar();

				switch(c){
					case Chars.SOLIDUS:
						t.TemporaryBuffer = "";
						t.ChangeTokenState<ScriptDataDoubleEscapeEndState>();
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
