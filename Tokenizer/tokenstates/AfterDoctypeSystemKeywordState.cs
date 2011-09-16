using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

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
						t.Parser.OnParseErrorRaised(string.Format("DOCTYPE の SYSTEM キーワードの後にスペースがありません。"));
						((DoctypeToken)t.CurrentToken).SystemIdentifier = "";
						t.ChangeTokenState<DoctypeSystemIdentifierState<DoubleQuoted>>();
						return;
					case Chars.APOSTROPHE:
						t.Parser.OnParseErrorRaised(string.Format("DOCTYPE の SYSTEM キーワードの後にスペースがありません。"));
						((DoctypeToken)t.CurrentToken).SystemIdentifier = "";
						t.ChangeTokenState<DoctypePublicIdentifierState<SingleQuoted>>();
						return;
					case Chars.GREATER_THAN_SIGN:
						t.Parser.OnParseErrorRaised(string.Format("DOCTYPE の SYSTEM キーワードの後に識別子がありません。"));
						((DoctypeToken)t.CurrentToken).ForceQuirks = true;
						t.ChangeTokenState<DataState>();
						t.EmitToken();
						return;
					case null:
						t.Parser.OnParseErrorRaised(string.Format("DOCTYPE の SYSTEM キーワードの後で終端に達しました。"));
						((DoctypeToken)t.CurrentToken).ForceQuirks = true;
						t.UnConsume(1);
						t.ChangeTokenState<DataState>();
						t.EmitToken();
						return;
					default:
						t.Parser.OnParseErrorRaised(string.Format("DOCTYPE の SYSTEM キーワードの後に不明な文字があります。"));
						((DoctypeToken)t.CurrentToken).ForceQuirks = true;
						t.ChangeTokenState<BogusDoctypeState>();
						return;
				}
			}
		}
	}
}
