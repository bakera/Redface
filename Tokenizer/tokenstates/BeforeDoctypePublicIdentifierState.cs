using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class BeforeDoctypePublicIdentifierState : TokenizationState{

			public override Token Read(Tokenizer t){
				char? c = t.ConsumeChar();

				switch(c){
					case Chars.CHARACTER_TABULATION:
					case Chars.LINE_FEED:
					case Chars.FORM_FEED:
					case Chars.SPACE:
						return null;
					case Chars.QUOTATION_MARK:
						t.ChangeTokenState<DoctypePublicIdentifierState<DoubleQuoted>>();
						return null;
					case Chars.APOSTROPHE:
						t.ChangeTokenState<DoctypePublicIdentifierState<SingleQuoted>>();
						return null;
					case Chars.GREATER_THAN_SIGN:{
						t.Parser.OnParseErrorRaised(string.Format("DOCTYPE の PUBLIC キーワードの後に識別子がありません。"));
						DoctypeToken result = (DoctypeToken)t.CurrentToken;
						result.ForceQuirks = true;
						t.ChangeTokenState<DataState>();
						return t.CurrentToken;
					}
					case null:{
						t.Parser.OnParseErrorRaised(string.Format("DOCTYPE の PUBLIC キーワードの後で終端に達しました。"));
						DoctypeToken result = (DoctypeToken)t.CurrentToken;
						result.ForceQuirks = true;
						t.UnConsume(1);
						t.ChangeTokenState<DataState>();
						return result;
					}
					default:{
						t.Parser.OnParseErrorRaised(string.Format("DOCTYPEの解析中に不明な文字を検出しました。"));
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
