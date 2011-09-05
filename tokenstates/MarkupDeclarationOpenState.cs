using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public const string DoctypeString = "DOCTYPE";
		public class MarkupDeclarationOpenState : TokenState{

			public MarkupDeclarationOpenState(RedFaceParser p) : base(p){}

			public override void Read(){
				Parser.ConsumeChar();
				switch(Parser.CurrentInputChar){
					case Chars.HYPHEN_MINUS:
						// ToDo: Comment
						break;
					case 'D':
					case 'd':
						string s = Parser.CurrentInputChar + Parser.ConsumeChar(DoctypeString.Length - 1);
						if(s.Equals(DoctypeString, StringComparison.InvariantCultureIgnoreCase)){
							
						}

						// ToDo: DOCTYPE
						break;
					case '[':
						// ToDo: CDATA
					default:
						break;
				}
				Parser.ChangeTokenState(typeof(DataState));
			}



		}
	}
}
