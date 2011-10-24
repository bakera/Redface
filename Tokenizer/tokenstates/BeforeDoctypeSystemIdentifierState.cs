using System;

namespace Bakera.RedFace{


	public class BeforeDoctypeSystemIdentifierState : TokenizationState{

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();

			switch(c){
				case Chars.CHARACTER_TABULATION:
				case Chars.LINE_FEED:
				case Chars.FORM_FEED:
				case Chars.SPACE:
					return;
				case Chars.QUOTATION_MARK:{
					((DoctypeToken)t.CurrentToken).PublicIdentifier = "";
					t.ChangeTokenState<DoctypeSystemIdentifierState<DoubleQuoted>>();
					return;
				}
				case Chars.APOSTROPHE:
					((DoctypeToken)t.CurrentToken).PublicIdentifier = "";
					t.ChangeTokenState<DoctypeSystemIdentifierState<SingleQuoted>>();
					return;
				case Chars.GREATER_THAN_SIGN:
					OnParseErrorRaised(string.Format("DOCTYPE の SYSTEM キーワードの後に識別子がありません。"));
					((DoctypeToken)t.CurrentToken).ForceQuirks = true;
					t.ChangeTokenState<DataState>();
					t.EmitToken();
					return;
				case null:{
					OnParseErrorRaised(string.Format("DOCTYPEの公開識別子の後で終端に達しました。"));
					((DoctypeToken)t.CurrentToken).ForceQuirks = true;
					t.UnConsume(1);
					t.ChangeTokenState<DataState>();
					t.EmitToken();
					return;
				}
				default:
					OnParseErrorRaised(string.Format("DOCTYPEの解析中に不明な文字を検出しました。"));
					((DoctypeToken)t.CurrentToken).ForceQuirks = true;
					t.ChangeTokenState<BogusDoctypeState>();
					return;
			}
		}
	}
}
