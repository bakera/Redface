using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class AfterAttributeNameState : TokenizationState{

			public override void Read(Tokenizer t){
				char? c = t.ConsumeChar();

				switch(c){
					case Chars.CHARACTER_TABULATION:
					case Chars.LINE_FEED:
					case Chars.FORM_FEED:
					case Chars.SPACE:
						return;
					case Chars.SOLIDUS:
						t.ChangeTokenState<SelfClosingStartTagState>();
						return;
					case Chars.GREATER_THAN_SIGN:
						t.ChangeTokenState<DataState>();
						t.EmitToken();
						return;
					case null:
						t.Parser.OnParseErrorRaised(string.Format("属性名の後にNUL文字を検出しました。"));
						((TagToken)t.CurrentToken).CreateAttribute(Chars.REPLACEMENT_CHARACTER, "");
						t.ChangeTokenState<AttributeNameState>();
						return;
					default:
						if(IsStringMatch(t, DoctypePublicId)){
							t.ChangeTokenState<AfterDoctypePublicKeywordState>();
							return;
						}
						if(IsStringMatch(t, DoctypeSystemId)){
							t.ChangeTokenState<AfterDoctypeSystemKeywordState>();
							return;
						} else {
							t.Parser.OnParseErrorRaised(string.Format("DOCTYPE に PUBLIC, SYSTEM 以外の識別子が含まれています。"));
							((DoctypeToken)t.CurrentToken).ForceQuirks = true;
							t.ChangeTokenState<BogusDoctypeState>();
							return;
						}
				}
			}
		}
	}
}
