using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class MarkupDeclarationOpenState : TokenizationState{

			public override Token Read(Tokenizer t){
				char? c = t.ConsumeChar();
				if(c == Chars.HYPHEN_MINUS){
					// ToDo: Comment
				} else if(IsStringMatch(t, DoctypeId)){
					t.ChangeTokenState<DoctypeState>();
				} else {
					t.Parser.Stop();
				}
				return null;
			}


		}

	}
}
