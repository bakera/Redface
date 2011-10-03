using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class ScriptDataEscapedLessThanSignState : TokenizationState{


			public override void Read(Tokenizer t){
				char? c = t.ConsumeChar();

				if(c.IsLatinCapitalLetter()){
					t.TemporaryBuffer = "";
					t.TemporaryBuffer += c.ToLower();
					t.EmitToken(Chars.LESS_THAN_SIGN);
					t.EmitToken(c);
					t.ChangeTokenState<ScriptDataDoubleEscapeStartState>();
					return;
				} else if(c.IsLatinSmallLetter()){
					t.TemporaryBuffer = "";
					t.TemporaryBuffer += c;
					t.EmitToken(Chars.LESS_THAN_SIGN);
					t.EmitToken(c);
					t.ChangeTokenState<ScriptDataDoubleEscapeStartState>();
					return;
 				}

				switch(c){
					case Chars.SOLIDUS:
						t.TemporaryBuffer = "";
						t.ChangeTokenState<ScriptDataEscapedEndTagOpenState>();
						return;
					default:
						t.EmitToken(Chars.LESS_THAN_SIGN);
						t.UnConsume(1);
						t.ChangeTokenState<ScriptDataEscapedState>();
						return;
				}
			}
		}
	}
}
