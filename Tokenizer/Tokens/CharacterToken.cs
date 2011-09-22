using System;

namespace Bakera.RedFace{

/*
 Comment and character tokens have data.
*/

	public class CharacterToken : Token{

		public string Data{get; set;}

		public CharacterToken(char? data) : this(data.ToString()){}
		public CharacterToken(string data){
			this.Data = data;
		}

		public override string ToString(){
			string result = this.GetType().Name;
			if(this.IsWhiteSpace){
				result += " (white space)";
			} else {
				result = string.Format(" / Data: \"{0}\"", this.Data);
			}
			return result;
		}

		// CharacterTokenでかつ文字が空白類文字の場合true
		// A character token that is one of U+0009 CHARACTER TABULATION, U+000A LINE FEED (LF), U+000C FORM FEED (FF), U+000D CARRIAGE RETURN (CR), or U+0020 SPACE
		public override bool IsWhiteSpace{
			get{
				foreach(char c in this.Data){
					switch(c){
						case Chars.CHARACTER_TABULATION:
						case Chars.LINE_FEED:
						case Chars.FORM_FEED:
						case Chars.CARRIAGE_RETURN:
						case Chars.SPACE:
							break;
						default:
							return false;
					}
				}
				return true;
			}
		}

	}
}
