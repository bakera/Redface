using System;

namespace Bakera.RedFace{

	public class DoctypePublicIdentifierState<T> : TokenizationState where T : Quoted, new(){

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();
			char quote = (new T()).Quote;

			if(c == quote){
				t.ChangeTokenState<AfterDoctypePublicIdentifierState>();
				return;
			}

			switch(c){
				case Chars.NULL:
					OnParseErrorRaised(string.Format("DOCTYPE 公開識別子の解析中にNULL文字を検出しました。"));
					((DoctypeToken)t.CurrentToken).PublicIdentifier += Chars.REPLACEMENT_CHARACTER;
					return;
				case Chars.GREATER_THAN_SIGN:
					OnParseErrorRaised(string.Format("DOCTYPE 公開識別子の解析中に U+003E GREATER THAN SIGN を検出しました。"));
					((DoctypeToken)t.CurrentToken).ForceQuirks = true;
					t.ChangeTokenState<DataState>();
					t.EmitToken();
					return;
				case null:
					OnMessageRaised(new SuddenlyEndAtDoctypeError());
					((DoctypeToken)t.CurrentToken).ForceQuirks = true;
					t.UnConsume(1);
					t.ChangeTokenState<DataState>();
					t.EmitToken();
					return;
				default:
					((DoctypeToken)t.CurrentToken).PublicIdentifier += c;
					return;
			}
		}
	}
}
