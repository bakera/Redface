using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class DoctypeState : TokenizationState{

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
						return new DoctypeToken(){ForceQuirks = true};
					default:
						Parser.OnParseErrorRaised(string.Format("文書型宣言のDOCTYPEの後ろにスペースがありません。出現した文字: {0}", CurrentInputChar));
						UnConsume();
						ChangeTokenState(typeof(BeforeDoctypeNameState));
						break;
				}
				return null;
			}



		}
	}
}
