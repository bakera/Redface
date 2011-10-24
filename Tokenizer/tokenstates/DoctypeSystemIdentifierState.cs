using System;

namespace Bakera.RedFace{
	public class DoctypeSystemIdentifierState<T> : TokenizationState where T : Quoted, new(){

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();
			char quote = (new T()).Quote;

			if(c == quote){
				t.ChangeTokenState<AfterDoctypeSystemIdentifierState>();
				return;
			}

			switch(c){
				case Chars.NULL:
					OnParseErrorRaised(string.Format("DOCTYPE システム識別子の解析中にNULL文字を検出しました。"));
					((DoctypeToken)t.CurrentToken).SystemIdentifier += Chars.REPLACEMENT_CHARACTER;
					return;
				case Chars.GREATER_THAN_SIGN:
					OnParseErrorRaised(string.Format("DOCTYPE システム識別子の解析中に U+003E GREATER THAN SIGN を検出しました。"));
					((DoctypeToken)t.CurrentToken).ForceQuirks = true;
					t.ChangeTokenState<DataState>();
					t.EmitToken();
					return;
				case null:
					OnParseErrorRaised(string.Format("DOCTYPE システム識別子の解析中に終端に達しました。"));
					((DoctypeToken)t.CurrentToken).ForceQuirks = true;
					t.UnConsume(1);
					t.ChangeTokenState<DataState>();
					t.EmitToken();
					return;
				default:
					((DoctypeToken)t.CurrentToken).SystemIdentifier += c;
					return;
			}
		}
	}
}
