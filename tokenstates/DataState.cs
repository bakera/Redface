using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class DataState : TokenState{

			public DataState(RedFaceParser p) : base(p){}

			public override void Read(){
				Parser.ConsumeChar();
				switch(Parser.NextInputChar){
					case Chars.AMPERSAND:
						Parser.ChangeTokenState(typeof(CharacterReferenceInDataState));
						break;
					case Chars.LESS_THAN_SIGN:
						Parser.ChangeTokenState(typeof(TagOpenState));
						break;
					case Chars.NULL:
						Parser.AddError("NULL文字が含まれています。");
						break;
					case null:
						Console.WriteLine("end");
						break;
					default:
						break;
				}
			}
		}
	}
}
