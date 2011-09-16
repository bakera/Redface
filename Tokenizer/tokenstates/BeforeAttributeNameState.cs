using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class BeforeAttributeNameState : TokenizationState{

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
					case Chars.NULL:
						t.Parser.OnParseErrorRaised(string.Format("属性名にNUL文字が含まれています。"));
						((TagToken)t.CurrentToken).CreateAttribute(Chars.REPLACEMENT_CHARACTER);
						t.ChangeTokenState<AttributeNameState>();
						return;
					case null:{
						t.Parser.OnParseErrorRaised(string.Format("属性名の解析中に終端に達しました。"));
						t.UnConsume(1);
						t.ChangeTokenState<DataState>();
						return;
					}
					case Chars.QUOTATION_MARK:
					case Chars.APOSTROPHE:
					case Chars.LESS_THAN_SIGN:
					case Chars.EQUALS_SIGN:
						t.Parser.OnParseErrorRaised(string.Format("属性名に使用できない文字です。{0}", c));
						goto default;
					default:{
						if(c.IsLatinCapitalLetter()){
							((TagToken)t.CurrentToken).CreateAttribute(c.ToLower());
						} else {
							((TagToken)t.CurrentToken).CreateAttribute(c);
						}
						t.ChangeTokenState<AttributeNameState>();
						return;
					}
				}
			}

		}
	}
}
