using System;

namespace Bakera.RedFace{

/*
 Comment and character tokens have data.
*/

	public abstract class DataToken : Token{

		public string Data{get; set;}

		public DataToken(){}
		public DataToken(string data){
			this.Data = data;
		}

	}
}
