using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class PLAINTEXTState : TokenizationState{

			public override void Read(Tokenizer t){
				char? c = t.ConsumeChar();
				switch(c){
					case Chars.NULL:
						t.Parser.OnParseErrorRaised(string.Format("NULL文字が検出されました。"));
						t.EmitToken(new CharacterToken(Chars.REPLACEMENT_CHARACTER));
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
