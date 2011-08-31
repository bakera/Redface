using System;
using System.Text;

namespace Bakera.RedFaceLint{

	public class Line{

// フィールド

		private readonly int myNumber;
		private StringBuilder myData = new StringBuilder();


// プロパティ
		public int Number{
			get{return myNumber;}
		}
		public string Data{
			get{return myData.ToString();}
		}


// コンストラクタ
		public Line(int lineNumber){
			myNumber = lineNumber;
		}
		public Line(Line prevLine){
			myNumber = prevLine.Number+1;
		}

// メソッド
		public void AddChar(char? c){
			myData.Append(c);
		}

	}
}



