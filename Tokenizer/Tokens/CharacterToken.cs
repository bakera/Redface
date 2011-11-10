using System;

namespace Bakera.RedFace{

/*
 Comment and character tokens have data.
*/

	public class CharacterToken : Token{

		public string Data{get; set;}
		public override string Name{get{return Data;}}

		public CharacterToken(char? data) : this(data.ToString()){}
		public CharacterToken(string data){
			this.Data = data;
		}

		public override string ToString(){
			string result = this.GetType().Name;
			if(this.IsSpaceCharacter){
				result += " (white space)";
			} else {
				result += string.Format(" / Data: \"{0}\"", this.Data);
			}
			return result;
		}

		// CharacterTokenでかつ文字が空白類文字の場合true
		// A character token that is one of U+0009 CHARACTER TABULATION, U+000A LINE FEED (LF), U+000C FORM FEED (FF), U+000D CARRIAGE RETURN (CR), or U+0020 SPACE
		// The space characters, for the purposes of this specification, are U+0020 SPACE, U+0009 CHARACTER TABULATION (tab), U+000A LINE FEED (LF), U+000C FORM FEED (FF), and U+000D CARRIAGE RETURN (CR).
		public override bool IsSpaceCharacter{
			get{
				foreach(char c in this.Data){
					if(!c.IsSpaceCharacter()) return false;
				}
				return true;
			}
		}

		// CharacterTokenでかつNUL文字の場合true
		public override bool IsNULL{
			get{
				foreach(char c in this.Data){
					if(c == Chars.NULL) return true;
				}
				return false;
			}
		}

		// CharacterTokenでかつLFの場合true
		public override bool IsLineFeed{
			get{
				return this.Data.Length == 1 && this.Data[0] == Chars.LINE_FEED;
			}
		}

		public override void AppendTo(TreeConstruction tree, InsertionMode mode){
			mode.AppendCharacterToken(tree, this);
		}


	}
}
