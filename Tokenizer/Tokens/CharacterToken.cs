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
			string result = string.Format("{0} / Data: \"{1}\"", this.GetType().Name, this.Data);
			return result;
		}

	}
}
