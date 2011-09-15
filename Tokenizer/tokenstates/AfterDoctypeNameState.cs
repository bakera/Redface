using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class AfterDoctypeNameState : TokenizationState{

			public override Token Read(Tokenizer t){
				char? c = t.ConsumeChar();

				switch(c){
					case Chars.CHARACTER_TABULATION:
					case Chars.LINE_FEED:
					case Chars.FORM_FEED:
					case Chars.SPACE:
						return null;
					case Chars.GREATER_THAN_SIGN:
						t.ChangeTokenState<DataState>();
						return t.CurrentToken;
					case null:{
						t.Parser.OnParseErrorRaised(string.Format("DOCTYPE nameの解析中に終端に達しました。"));
						DoctypeToken result = (DoctypeToken)t.CurrentToken;
						result.ForceQuirks = true;
						t.UnConsume(1);
						t.ChangeTokenState<DataState>();
						return result;
					}
					default:
						if(IsStringMatch(t, DoctypePublicId)){
							t.ChangeTokenState<AfterDoctypePublicKeywordState>();
							return null;
						}
						if(IsStringMatch(t, DoctypeSystemId)){
							t.ChangeTokenState<AfterDoctypeSystemKeywordState>();
							return null;
						} else {
							t.Parser.OnParseErrorRaised(string.Format("DOCTYPE に PUBLIC, SYSTEM 以外の識別子が含まれています。"));
							DoctypeToken result = (DoctypeToken)t.CurrentToken;
							result.ForceQuirks = true;
							t.ChangeTokenState<BogusDoctypeState>();
							return null;
						}
				}
			}
		}
	}
}
