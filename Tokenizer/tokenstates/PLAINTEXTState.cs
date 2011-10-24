using System;

namespace Bakera.RedFace{

	public class PLAINTEXTState : TokenizationState{

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();
			switch(c){
				case Chars.NULL:
					OnParseErrorRaised(string.Format("NULL文字が検出されました。"));
					t.EmitToken(Chars.REPLACEMENT_CHARACTER);
					return;
				case null:
					t.EmitToken(new EndOfFileToken());
					return;
				default:
					t.EmitToken(c);
					return;
			}
		}

	}
}
