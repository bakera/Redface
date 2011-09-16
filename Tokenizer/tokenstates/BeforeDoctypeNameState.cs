using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class BeforeDoctypeNameState : TokenizationState{

			public override void Read(Tokenizer t){
				char? c = t.ConsumeChar();

				switch(c){
					case Chars.CHARACTER_TABULATION:
					case Chars.LINE_FEED:
					case Chars.FORM_FEED:
					case Chars.SPACE:
						return;
					case Chars.NULL:
						t.Parser.OnParseErrorRaised(string.Format("DOCTYPEの解析中にNULL文字を検出しました。"));
						t.ChangeTokenState<DoctypeNameState>();
						t.CurrentToken = new DoctypeToken(){Name = Chars.REPLACEMENT_CHARACTER.ToString()};
						return;
					case Chars.GREATER_THAN_SIGN:
						t.Parser.OnParseErrorRaised(string.Format("DOCTYPEのNameが空です。"));
						t.ChangeTokenState<DataState>();
						t.EmitToken(new DoctypeToken(){ForceQuirks = true});
						return;
					case null:{
						t.Parser.OnParseErrorRaised(string.Format("DOCTYPEの解析中に終端に達しました。"));
						t.UnConsume(1);
						t.ChangeTokenState<DataState>();
						t.EmitToken(new DoctypeToken(){ForceQuirks = true});
						return;
					}
					default:{
						DoctypeToken result = new DoctypeToken();
						if(c.IsLatinCapitalLetter()){
							result.Name = c.ToLower().ToString();
						} else {
							result.Name = c.ToString();
						}
						t.CurrentToken = result;
						t.ChangeTokenState<DoctypeNameState>();
						return;
					}
				}
			}

		}
	}
}
