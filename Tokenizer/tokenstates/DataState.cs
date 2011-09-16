using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class DataState : TokenizationState{

			public override void Read(Tokenizer t){
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
						t.EmitToken(new CharacterToken(c));
						return;
					case null:
						t.EmitToken(new EndOfFileToken());
						return;
					default:
						t.EmitToken(new CharacterToken(c));
						return;
				}
			}

		}
	}
}
