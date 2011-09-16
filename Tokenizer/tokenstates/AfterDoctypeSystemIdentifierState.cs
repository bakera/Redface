using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class AfterDoctypeSystemIdentifierState : TokenizationState{

			public override void Read(Tokenizer t){
				char? c = t.ConsumeChar();

				switch(c){
					case Chars.CHARACTER_TABULATION:
					case Chars.LINE_FEED:
					case Chars.FORM_FEED:
					case Chars.SPACE:
						return;
					case Chars.GREATER_THAN_SIGN:{
						t.ChangeTokenState<DataState>();
						t.EmitToken();
						return;
					}
					case null:
						t.Parser.OnParseErrorRaised(string.Format("DOCTYPEのシステム識別子の後で終端に達しました。"));
						((DoctypeToken)t.CurrentToken).ForceQuirks = true;
						t.UnConsume(1);
						t.ChangeTokenState<DataState>();
						t.EmitToken();
						return;
					default:
						t.Parser.OnParseErrorRaised(string.Format("DOCTYPEの解析中に不明な文字を検出しました。"));
						t.ChangeTokenState<BogusDoctypeState>();
						return;
				}
			}
		}
	}
}
