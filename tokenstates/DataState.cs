using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class DataState : TokenState{

			public DataState(RedFaceParser p) : base(p){}

			public override void Read(){
				Parser.ConsumeChar();
				switch(Parser.CurrentInputChar){
					case Chars.AMPERSAND:
						Parser.ChangeTokenState(typeof(CharacterReferenceInDataState));
						break;
					case Chars.LESS_THAN_SIGN:
						Parser.ChangeTokenState(typeof(TagOpenState));
						break;
					case Chars.NULL:
						Parser.OnParseErrorRaised(string.Format("NULL文字が検出されました。"));
						Parser.Emit();
						break;
					case null:
						break;
					default:
						Parser.Emit();
						break;
				}
			}
		}
	}
}
