using System;

namespace Bakera.RedFace{

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
					OnParseErrorRaised(string.Format("NULL文字が検出されました。"));
					t.EmitToken(c);
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
