using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class DoctypeState : TokenState{

			public DoctypeState(Tokenizer t) : base(t){}

			public override Token Read(){
				ConsumeChar();
				switch(CurrentInputChar){
					case Chars.CHARACTER_TABULATION:
					case Chars.LINE_FEED:
					case Chars.FORM_FEED:
					case Chars.SPACE:
						ChangeTokenState(typeof(BeforeDoctypeNameState));
						break;
					case null:
						Parser.OnParseErrorRaised(string.Format("DOCTYPEの解析中に終端に達しました。"));
						UnConsume();
						ChangeTokenState(typeof(DataState));
						// ToDo:
						// Parse error. Create a new DOCTYPE token.
						// Set its force-quirks flag to on.
						// Emit the token. Reconsume the EOF character in the data state.
						break;
					default:
						Parser.OnParseErrorRaised(string.Format("DOCTYPEの解析中に不明な文字を検出しました: {0}", CurrentInputChar));
						UnConsume();
						ChangeTokenState(typeof(BeforeDoctypeNameState));
						break;
				}
				return null;
			}



		}
	}
}
