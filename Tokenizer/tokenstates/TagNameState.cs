using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class TagNameState : TokenizationState{

			public override void Read(Tokenizer t){
				char? c = t.ConsumeChar();

				switch(c){
					case Chars.CHARACTER_TABULATION:
					case Chars.LINE_FEED:
					case Chars.FORM_FEED:
					case Chars.SPACE:
						t.ChangeTokenState<BeforeAttributeNameState>();
						return;
					case Chars.SOLIDUS:
						t.ChangeTokenState<SelfClosingStartTagState>();
						return;
					case Chars.GREATER_THAN_SIGN:
						t.ChangeTokenState<DataState>();
						t.EmitToken();
						return;
					case Chars.NULL:
						t.Parser.OnParseErrorRaised(string.Format("開始タグの解析中にNULL文字を検出しました。"));
						((TagToken)t.CurrentToken).Name += Chars.REPLACEMENT_CHARACTER;
						return;
					case null:
						t.Parser.OnParseErrorRaised(string.Format("開始タグの解析中に終端に達しました。"));
						t.UnConsume(1);
						t.ChangeTokenState<DataState>();
						return;
					default:{
						TagToken result = (TagToken)t.CurrentToken;
						if(c.IsLatinCapitalLetter()){
							result.Name += char.ToLower((char)c);
						} else {
							result.Name += c;
						}
						return;
					}
				}
			}
		}
	}
}
