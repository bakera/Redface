using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class TagOpenState : TokenState{

			public TagOpenState(RedFaceParser p) : base(p){}

			public override void Read(){
				Parser.ConsumeChar();
				switch(Parser.NextInputChar){
					case Chars.AMPERSAND:
					default:
						break;
				}
				Parser.ChangeTokenState(typeof(DataState));
			}
		}
	}
}
