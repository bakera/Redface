using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class BeforeDoctypeNameState : TokenizationState{

			public override Token Read(Tokenizer t){
				char? c = t.ConsumeChar();

				switch(c){
					case Chars.CHARACTER_TABULATION:
					case Chars.LINE_FEED:
					case Chars.FORM_FEED:
					case Chars.SPACE:
						return null;
					case Chars.NULL:
						t.Parser.OnParseErrorRaised(string.Format("DOCTYPEの解析中にNULL文字を検出しました。"));
						t.ChangeTokenState<DoctypeNameState>();
						t.CurrentToken = new DoctypeToken(){Name = Chars.REPLACEMENT_CHARACTER.ToString()};
						return null;
					case Chars.GREATER_THAN_SIGN:
						t.Parser.OnParseErrorRaised(string.Format("DOCTYPEのNameが空です。"));
						t.ChangeTokenState<DataState>();
						return new DoctypeToken(){ForceQuirks = true};
					case null:{
						t.Parser.OnParseErrorRaised(string.Format("DOCTYPEの解析中に終端に達しました。"));
						DoctypeToken result = new DoctypeToken(){ForceQuirks = true};
						t.UnConsume(1);
						t.ChangeTokenState<DataState>();
						return result;
					}
					default:{
						DoctypeToken result = new DoctypeToken();
						if(c.IsLatinCapitalLetter()){
							result.Name = char.ToLower((char)c).ToString();
						} else {
							result.Name = c.ToString();
						}
						t.CurrentToken = result;
						t.ChangeTokenState<DoctypeNameState>();
						return null;
					}
				}
			}

		}
	}
}
