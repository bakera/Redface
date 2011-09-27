using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class ScriptDataEscapedEndTagOpenState : TokenizationState{

			public override void Read(Tokenizer t){
				char? c = t.ConsumeChar();
				if(c.IsLatinCapitalLetter()){
					t.CurrentToken = new EndTagToken(){Name = c.ToLower().ToString()};
					t.TemporaryBuffer += c;
					t.ChangeTokenState<ScriptDataEscapedEndTagNameState>();
					return;
				} else if(c.IsLatinSmallLetter()){
					t.CurrentToken = new EndTagToken(){Name = c.ToString()};
					t.TemporaryBuffer += c;
					t.ChangeTokenState<ScriptDataEscapedEndTagNameState>();
					return;
 				}
				t.EmitToken(new CharacterToken("</"));
				t.UnConsume(1);
				t.ChangeTokenState<ScriptDataEscapedState>();
				return;
			}
		}
	}
}
