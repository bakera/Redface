using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class DoctypeSystemIdentifierState<T> : TokenizationState where T : Quoted, new(){

			public override Token Read(Tokenizer t){
				char? c = t.ConsumeChar();
				char quote = (new T()).Quote;

				if(c == quote){
					t.ChangeTokenState<AfterDoctypeSystemIdentifierState>();
					return null;
				}

				switch(c){
					case Chars.NULL:{
						t.Parser.OnParseErrorRaised(string.Format("DOCTYPE システム識別子の解析中にNULL文字を検出しました。"));
						DoctypeToken result = (DoctypeToken)t.CurrentToken;
						result.SystemIdentifier += Chars.REPLACEMENT_CHARACTER;
						return null;
					}
					case Chars.GREATER_THAN_SIGN:{
						t.Parser.OnParseErrorRaised(string.Format("DOCTYPE システム識別子の解析中に U+003E GREATER THAN SIGN を検出しました。"));
						DoctypeToken result = (DoctypeToken)t.CurrentToken;
						result.ForceQuirks = true;
						t.ChangeTokenState<DataState>();
						return t.CurrentToken;
					}
					case null:{
						t.Parser.OnParseErrorRaised(string.Format("DOCTYPE システム識別子の解析中に終端に達しました。"));
						DoctypeToken result = (DoctypeToken)t.CurrentToken;
						result.ForceQuirks = true;
						t.UnConsume(1);
						t.ChangeTokenState<DataState>();
						return result;
					}
					default:{
						DoctypeToken result = (DoctypeToken)t.CurrentToken;
						result.SystemIdentifier += c;
						return null;
					}
				}
			}
		}
	}
}
