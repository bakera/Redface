using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class DataState : TokenizationState{

			public override Token Read(Tokenizer t){
				char? c = t.ConsumeChar();
				switch(c){
					case Chars.AMPERSAND:
						t.ChangeTokenState<CharacterReferenceInDataState>();
						break;
					case Chars.LESS_THAN_SIGN:
						t.ChangeTokenState<TagOpenState>();
						break;
					case Chars.NULL:
						t.Parser.OnParseErrorRaised(string.Format("NULL文字が検出されました。"));
						return new CharacterToken(c);
					case null:
						return new EndOfFileToken();
					default:
						return new CharacterToken(c);
				}
				return null;
			}
		}
	}
}
