using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class AttributeValueState<T> : TokenizationState where T : Quoted, new(){

			public override void Read(Tokenizer t){

				char? c = t.ConsumeChar();
				char quote = (new T()).Quote;

				if(c == quote){
					t.ChangeTokenState<AfterAttributeValueQuotedState>();
					return;
				}

				switch(c){
					case Chars.AMPERSAND:
//						t.ChangeTokenState<CharacterReferenceInAttributeState>();
						return;
					case Chars.NULL:
						t.Parser.OnParseErrorRaised(string.Format("属性値にNUL文字が含まれています。"));
						t.CurrentTagToken.CurrentAttribute.Value += Chars.REPLACEMENT_CHARACTER;
						return;
					case null:
						t.Parser.OnParseErrorRaised(string.Format("属性値の解析中に終端に達しました。"));
						t.UnConsume(1);
						t.ChangeTokenState<DataState>();
						return;
					default:
						t.CurrentTagToken.CurrentAttribute.Value += c;
						return;
				}
			}
		}
	}
}
