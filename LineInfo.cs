using System;
using System.Text;

namespace Bakera.RedFace{

	// InputStreamの現在の行に関する情報を保持するクラスです。
	public class LineInfo{

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
		public LineInfo(int lineNumber){
			myNumber = lineNumber;
		}
		public LineInfo(LineInfo prevLine){
			myNumber = prevLine.Number+1;
		}

// メソッド
		public void AddChar(char? c){
			myData.Append(c);
		}

	}
}



