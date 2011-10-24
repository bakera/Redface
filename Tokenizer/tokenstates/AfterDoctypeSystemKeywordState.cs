using System;

namespace Bakera.RedFace{


	public class AfterDoctypeSystemKeywordState : TokenizationState{

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();

			switch(c){
				case Chars.CHARACTER_TABULATION:
				case Chars.LINE_FEED:
				case Chars.FORM_FEED:
				case Chars.SPACE:
					t.ChangeTokenState<BeforeDoctypeSystemIdentifierState>();
					return;
				case Chars.QUOTATION_MARK:
					OnParseErrorRaised(string.Format("DOCTYPE の SYSTEM キーワードの後にスペースがありません。"));
					((DoctypeToken)t.CurrentToken).SystemIdentifier = "";
					t.ChangeTokenState<DoctypeSystemIdentifierState<DoubleQuoted>>();
					return;
				case Chars.APOSTROPHE:
					OnParseErrorRaised(string.Format("DOCTYPE の SYSTEM キーワードの後にスペースがありません。"));
					((DoctypeToken)t.CurrentToken).SystemIdentifier = "";
					t.ChangeTokenState<DoctypePublicIdentifierState<SingleQuoted>>();
					return;
				case Chars.GREATER_THAN_SIGN:
					OnParseErrorRaised(string.Format("DOCTYPE の SYSTEM キーワードの後に識別子がありません。"));
					((DoctypeToken)t.CurrentToken).ForceQuirks = true;
					t.ChangeTokenState<DataState>();
					t.EmitToken();
					return;
				case null:
					OnParseErrorRaised(string.Format("DOCTYPE の SYSTEM キーワードの後で終端に達しました。"));
					((DoctypeToken)t.CurrentToken).ForceQuirks = true;
					t.UnConsume(1);
					t.ChangeTokenState<DataState>();
					t.EmitToken();
					return;
				default:
					OnParseErrorRaised(string.Format("DOCTYPE の SYSTEM キーワードの後に不明な文字があります。"));
					((DoctypeToken)t.CurrentToken).ForceQuirks = true;
					t.ChangeTokenState<BogusDoctypeState>();
					return;
			}
		}
	}
}
