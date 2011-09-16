using System;

namespace Bakera.RedFace{

/*
 Comment and character tokens have data.
*/

	public class ReferencedCharacterToken : CharacterToken{
		public ReferencedCharacterToken(): base(){}
		public ReferencedCharacterToken(string data) : base(data){}

		public string OriginalString{get; set;}


		public override string ToString(){
			string result = base.ToString();
			result += string.Format("\n OriginalString: {0}", OriginalString);
			return result;
		}
	}
}
