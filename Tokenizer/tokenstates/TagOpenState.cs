using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class TagOpenState : TokenizationState{


			public override Token Read(Tokenizer t){
				char? c = t.ConsumeChar();
				switch(c){
					case Chars.EXCLAMATION_MARK:
						t.ChangeTokenState<MarkupDeclarationOpenState>();
						break;
					case Chars.SOLIDUS:
					case Chars.QUESTION_MARK :
					default:
						t.ChangeTokenState<DataState>();
						break;
				}
				return null;
			}
		}
	}
}
