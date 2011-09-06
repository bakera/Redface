using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class TagOpenState : TokenState{

			public TagOpenState(RedFaceParser p) : base(p){}

			public override void Read(){
				Parser.ConsumeChar();
				switch(Parser.CurrentInputChar){
					case Chars.EXCLAMATION_MARK:
						Parser.ChangeTokenState(typeof(MarkupDeclarationOpenState));
						break;
					case Chars.SOLIDUS:
					case Chars.QUESTION_MARK :
					default:
						Parser.ChangeTokenState(typeof(DataState));
						break;
				}
			}
		}
	}
}
