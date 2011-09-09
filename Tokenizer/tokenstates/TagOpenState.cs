using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class TagOpenState : TokenizationState{

			public TagOpenState(Tokenizer t) : base(t){}

			public override Token Read(){
				ConsumeChar();
				switch(CurrentInputChar){
					case Chars.EXCLAMATION_MARK:
						ChangeTokenState(typeof(MarkupDeclarationOpenState));
						break;
					case Chars.SOLIDUS:
					case Chars.QUESTION_MARK :
					default:
						ChangeTokenState(typeof(DataState));
						break;
				}
				return null;
			}
		}
	}
}
