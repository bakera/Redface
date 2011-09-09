using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class DataState : TokenState{

			public DataState(Tokenizer t) : base(t){}

			public override Token Read(){
				ConsumeChar();
				switch(CurrentInputChar){
					case Chars.AMPERSAND:
						ChangeTokenState(typeof(CharacterReferenceInDataState));
						break;
					case Chars.LESS_THAN_SIGN:
						ChangeTokenState(typeof(TagOpenState));
						break;
					case Chars.NULL:
						Parser.OnParseErrorRaised(string.Format("NULL文字が検出されました。"));
						return new CharacterToken(CurrentInputChar);
					case null:
						return new EndOfFileToken();
					default:
						return new CharacterToken(CurrentInputChar);
				}
				return null;
			}
		}
	}
}
