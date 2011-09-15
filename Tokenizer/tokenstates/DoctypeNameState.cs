using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class DoctypeNameState : TokenizationState{

			public override Token Read(Tokenizer t){
				char? c = t.ConsumeChar();

				switch(c){
					case Chars.CHARACTER_TABULATION:
					case Chars.LINE_FEED:
					case Chars.FORM_FEED:
					case Chars.SPACE:
						t.ChangeTokenState<AfterDoctypeNameState>();
						return null;
					case Chars.GREATER_THAN_SIGN:
						t.ChangeTokenState<DataState>();
						return t.CurrentToken;
					case Chars.NULL:{
						t.Parser.OnParseErrorRaised(string.Format("DOCTYPE nameの解析中にNULL文字を検出しました。"));
						DoctypeToken result = (DoctypeToken)t.CurrentToken;
						result.Name += Chars.REPLACEMENT_CHARACTER.ToString();
						return null;
					}
					case null:{
						t.Parser.OnParseErrorRaised(string.Format("DOCTYPE nameの解析中に終端に達しました。"));
						DoctypeToken result = (DoctypeToken)t.CurrentToken;
						result.ForceQuirks = true;
						t.UnConsume(1);
						t.ChangeTokenState<DataState>();
						return result;
					}
					default:{
						DoctypeToken result = (DoctypeToken)t.CurrentToken;
						if(c.IsLatinCapitalLetter()){
							result.Name += char.ToLower((char)c).ToString();
						} else {
							result.Name += c.ToString();
						}
						return null;
					}
				}
			}
		}
	}
}
