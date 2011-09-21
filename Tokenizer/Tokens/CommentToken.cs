using System;
using System.Text;

namespace Bakera.RedFace{

/*
 Comment and character tokens have data.
*/

	public class CommentToken : Token{

		private StringBuilder myData = new StringBuilder();

		public CommentToken(){}
		public CommentToken(char? c){
			Append(c);
		}

		public string Data{
			get{return myData.ToString();}
		}

		public void Append(char? c){
			myData.Append(c);
		}
		public void Append(string s){
			myData.Append(s);
		}

		public override string ToString(){
			string result = string.Format("{0} / Data: \"{1}\"", this.GetType().Name, this.Data);
			return result;
		}

	}
}
