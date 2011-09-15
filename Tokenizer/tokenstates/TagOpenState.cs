using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class TagOpenState : TokenizationState{


			public override Token Read(Tokenizer t){
				char? c = t.ConsumeChar();
				switch(c){
					case Chars.EXCLAMATION_MARK:
						t.ChangeTokenState<MarkupDeclarationOpenState>();
						return null;
					case Chars.SOLIDUS:
						// ToDo: end tag open
						break;
					case Chars.QUESTION_MARK:
						// ToDo: bogus comment
						// 処理命令は bogus comment として扱われる
						break;
				}
				if(c.IsLatinCapitalLetter()){
					t.CurrentToken = new DoctypeToken(){Name = char.ToLower((char)c).ToString()};
					t.ChangeTokenState<TagNameState>();
					return null;
				} else if(c.IsLatinSmallLetter()){
					t.CurrentToken = new DoctypeToken(){Name = c.ToString()};
					t.ChangeTokenState<TagNameState>();
					return null;
 				}
				t.Parser.OnParseErrorRaised(string.Format("LESS THAN SIGNの後の文字がTag Nameではありません。"));
				t.UnConsume(1);
				t.ChangeTokenState<DataState>();
				return new CharacterToken(Chars.LESS_THAN_SIGN);
			}
		}
	}
}
