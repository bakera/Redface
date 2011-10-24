using System;

namespace Bakera.RedFace{

	public class AttributeNameState : TokenizationState{

		public override void Read(Tokenizer t){
			char? c = t.ConsumeChar();

			switch(c){
				case Chars.CHARACTER_TABULATION:
				case Chars.LINE_FEED:
				case Chars.FORM_FEED:
				case Chars.SPACE:
					CheckDuplicateAttribute(t);
					t.ChangeTokenState<AfterAttributeNameState>();
					return;
				case Chars.SOLIDUS:
					CheckDuplicateAttribute(t);
					t.ChangeTokenState<SelfClosingStartTagState>();
					return;
				case Chars.EQUALS_SIGN:
					CheckDuplicateAttribute(t);
					t.ChangeTokenState<BeforeAttributeValueState>();
					return;
				case Chars.GREATER_THAN_SIGN:
					CheckDuplicateAttribute(t);
					t.ChangeTokenState<DataState>();
					t.EmitToken();
					return;
				case Chars.NULL:
					OnParseErrorRaised(string.Format("属性名の解析中にNULL文字を検出しました。"));
					t.CurrentTagToken.CurrentAttribute.Name += Chars.REPLACEMENT_CHARACTER;
					return;
				case Chars.QUOTATION_MARK:
				case Chars.APOSTROPHE:
				case Chars.LESS_THAN_SIGN:
					OnParseErrorRaised(string.Format("属性名として不正な文字を検出しました。: {0}", c));
					goto default;
				case null:
					OnParseErrorRaised(string.Format("属性名の解析中に終端に達しました。"));
					t.UnConsume(1);
					CheckDuplicateAttribute(t);
					t.ChangeTokenState<DataState>();
					return;
				default:{
					if(c.IsLatinCapitalLetter()){
						t.CurrentTagToken.CurrentAttribute.Name += c.ToLower();
					} else {
						t.CurrentTagToken.CurrentAttribute.Name += c;
					}
					return;
				}
			}
		}


		private void CheckDuplicateAttribute(Tokenizer t){
			if(t.CurrentTagToken.IsDuplicateAttribute){
				t.CurrentTagToken.DropAttribute();
				OnParseErrorRaised(string.Format("属性名が重複しています。: {0}", t.CurrentTagToken.CurrentAttribute.Name));
			}
		}

	}
}
