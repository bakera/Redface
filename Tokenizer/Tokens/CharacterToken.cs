using System;

namespace Bakera.RedFace{

/*
 Comment and character tokens have data.
*/

	public class CharacterToken : DataToken{
		public CharacterToken(): base(){}
		public CharacterToken(char? data) : this(data.ToString()){}
		public CharacterToken(string data) : base(data){}
	}
}
