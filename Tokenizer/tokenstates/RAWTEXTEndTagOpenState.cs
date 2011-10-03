using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class RAWTEXTEndTagOpenState : TokenizationState{

			public override void Read(Tokenizer t){
				char? c = t.ConsumeChar();
				if(c.IsLatinCapitalLetter()){
					t.CurrentToken = new EndTagToken(){Name = c.ToLower().ToString()};
					t.TemporaryBuffer += c;
					t.ChangeTokenState<RAWTEXTEndTagNameState>();
					return;
				} else if(c.IsLatinSmallLetter()){
					t.CurrentToken = new EndTagToken(){Name = c.ToString()};
					t.TemporaryBuffer += c;
					t.ChangeTokenState<RAWTEXTEndTagNameState>();
					return;
 				}
				t.EmitToken(Chars.LESS_THAN_SIGN);
				t.EmitToken(Chars.SOLIDUS);
				t.UnConsume(1);
				t.ChangeTokenState<RAWTEXTState>();
				return;
			}
		}
	}
}
