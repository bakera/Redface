using System;
using System.IO;
using System.Text;

namespace Bakera.RedFace{


	public class InputStream : IDisposable{

		private StringBuilder myConsumedChars = new StringBuilder(); // Consumeされた文字の履歴
		private int myOffset = 0;
		private TextReader myTextReader = null;

		public EncodingConfidence EncodingConfidence{get; set;}

// コンストラクタ
		public InputStream(RedFaceParser parser, TextReader reader){
			myTextReader = reader;
		}


// プロパティ
		public char? CurrentInputChar{
			get {
				char? result = GetCharByPosition(CurrentPosition);
				return result;
			}
		}

		public char? NextInputChar{
			get {
				if(Offset > 0) return GetCharByPosition(CurrentPosition + 1);
				if(Offset == 0) {
					if(ConsumeNextInputChar()){
						myOffset--;
						return GetCharByPosition(CurrentPosition + 1);
					}
				}
				return null;
			}
		}

		// UnConsumeされた文字があるとき、オフセットを示す。EOFに達すると-1
		public int Offset{
			get {return myOffset;}
		}

		public int CurrentPosition{
			get{
				return myConsumedChars.Length - myOffset;
			}
		}



// メソッド
		public void Dispose(){
			if(myTextReader != null) myTextReader.Dispose();
		}


		// 1文字読みます。
		// 終端に達していたらfalseを返します。
		public bool ConsumeNextInputChar(){
			if(myOffset < 0) return false;
			if(myOffset > 0){
				myOffset--;
				return true;
			}
			bool result = ReadCharFromStream();
			if(!result) myOffset = -1;
			return result;
		}

		// Unconsumeします。
		public void UnConsume(int offset){
			myOffset += offset;
		}


// private メソッド

		// ストリームから文字を読み取ってバッファに追加します。
		// 終端に達していたらfalseを返します。
		private bool ReadCharFromStream(){
			for(;;){
				int charNum = myTextReader.Read();
				if(charNum < 0){
					return false;
				}

				// ZWNBSは無視する (willful violation)
				// BOMはTextReaderによって既に無視されているはず
				if(charNum == Chars.BOM){
					OnWillfulViolationRaised(string.Format("文中に U+FEFF (BYTE ORDER MARK / ZERO WIDTH NO BREAK SPACE) を検出しましたが、無視します。"));
					continue;
				}

				// noncharactersはパースエラー
				// HTML5 spec では処理が未定義だがとりあえず無視する (バッファに取り込まない)
				if(Chars.IsErrorChar(charNum)){
					OnParseErrorRaised(string.Format("非Unicode文字 (noncharacters) が含まれています。: {0}", charNum));
					continue;
				}

				if(charNum == Chars.CARRIAGE_RETURN){
					// CR+LFの場合、LFのみバッファに入れる
					// CR+何かの場合、LF+何かをバッファに入れてOffsetを+1する
					myConsumedChars.Append(Chars.LINE_FEED);
					int nextCharNum = myTextReader.Read();
					if(nextCharNum == Chars.LINE_FEED) return true;
					myConsumedChars.Append((char)nextCharNum);
					myOffset++;
					return true;
				}
				myConsumedChars.Append((char)charNum);
				return true;
			}
		}

		private char? GetCharByPosition(int position){
			int index = position - 1;
			if(index < 0) return null;
			if(index >= myConsumedChars.Length) return null;
			return myConsumedChars[index];
		}

// イベント
		public event EventHandler<ParserEventArgs> ParseErrorRaised;
		public event EventHandler<ParserEventArgs> WillfulViolationRaised;

		// ParseErrorRaisedイベントを発生します。
		protected virtual void OnParseErrorRaised(string s){
			if(ParseErrorRaised != null){
				ParseErrorRaised(this, new ParserEventArgs(){Message = s});
			}
		}
		// WillfulViolationRaisedイベントを発生します。
		protected virtual void OnWillfulViolationRaised(string s){
			if(WillfulViolationRaised != null){
				WillfulViolationRaised(this, new ParserEventArgs(){Message = s});
			}
		}


	} //  class InputStream
}



