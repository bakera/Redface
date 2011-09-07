using System;
using System.IO;
using System.Text;

namespace Bakera.RedFace{

	public class InputStream : IDisposable{

		private StringBuilder myConsumedChars = null; // Consumeされた文字の履歴
		private int myOffset = 0;
		private TextReader myTextReader = null;


// コンストラクタ
		public InputStream(TextReader reader){
			myTextReader = reader;
			if(reader is StreamReader){
				myConsumedChars = new StringBuilder((int)((StreamReader)reader).BaseStream.Length);
			} else {
				myConsumedChars = new StringBuilder();
			}
		}


// プロパティ
		public char? CurrentInputChar{
			get {return GetCharByOffset(myOffset);}
		}

		public char? NextInputChar{
			get {return GetCharByOffset(myOffset + 1);}
		}

		// UnConsumeされた文字があるとき、オフセットを示す。EOFに達すると-1
		public int Offset{
			get {return myOffset;}
		}


// メソッド
		void IDisposable.Dispose(){
			if(myTextReader != null) myTextReader.Dispose();
		}


// private メソッド
		private void ConsumeNextInputChar(){
			if(myOffset < 0){
				return;
			}
			if(myOffset > 0){
				myOffset--;
				return;
			}
			int charNum = myTextReader.Read();
			if(charNum < 0){
				myOffset = -1;
			} else {
				myConsumedChars.Append((char)charNum);
			}
		}

		private char? GetCharByOffset(int offset){
			if(offset < 0) return null;
			if(offset > myConsumedChars.Length) return null;
			return myConsumedChars[myConsumedChars.Length - 1 - offset];
		}

	}

}



