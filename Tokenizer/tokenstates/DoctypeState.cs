using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class DoctypeState : TokenizationState{

			public override Token Read(Tokenizer t){
				char? c = t.ConsumeChar();
				switch(c){
					case Chars.CHARACTER_TABULATION:
					case Chars.LINE_FEED:
					case Chars.FORM_FEED:
					case Chars.SPACE:
						return null;
					case null:
						t.Parser.OnParseErrorRaised(string.Format("DOCTYPEの解析中に終端に達しました。"));
						t.UnConsume(1);
						t.ChangeTokenState<DataState>();
						return new DoctypeToken(){ForceQuirks = true};
					default:
						t.Parser.OnParseErrorRaised(string.Format("文書型宣言のDOCTYPEの後ろにスペースがありません。出現した文字: {0}", c));
						t.UnConsume(1);
						t.ChangeTokenState<BeforeDoctypeNameState>();
						break;
				}
				return null;
			}



		}
	}
}
