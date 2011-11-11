using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bakera.RedFace{


	public class InputStream : IDisposable{

		private StringBuilder myConsumedChars = new StringBuilder(); // Consumeされた文字の履歴
		private int myOffset = 0;
		private Stream myStream = null;
		private TextReader myTextReader = null;

		private const int SniffEncodingBufferSize = 1024;

// コンストラクタ
		public InputStream(RedFaceParser parser, Encoding defaultEncoding, Stream s){
			myStream = s;
			this.Encoding = defaultEncoding;
			this.EncodingConfidence = EncodingConfidence.Tentative;
		}


// プロパティ
		public EncodingConfidence EncodingConfidence{get; set;}
		public Encoding Encoding{get; set;}

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
				// HTML5 spec ではエラー後の処理が未定義だがとりあえず無視する (バッファに取り込まない)
				if(Chars.IsErrorChar(charNum)){
					OnParseErrorRaised(string.Format("非Unicode文字 (noncharacters) が含まれています。: {0}", charNum));
					continue;
				}

				if(charNum == Chars.CARRIAGE_RETURN){
					// CR+LFの場合、LFのみバッファに入れる
					// CR+終端の場合、LFのみバッファに入れる
					// CR+何かの場合、LF+何かをバッファに入れてOffsetを+1する
					myConsumedChars.Append(Chars.LINE_FEED);
					int nextCharNum = myTextReader.Read();
					if(nextCharNum < 0 || nextCharNum == Chars.LINE_FEED) return true;
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
		public event EventHandler<ParserEventArgs> ParseEventRaised;

		// ParseErrorRaisedイベントを発生します。
		protected virtual void OnParseErrorRaised(string s){
			if(ParseEventRaised != null){
				ParseEventRaised(this, new ParserEventArgs(EventLevel.ParseError){Message = s});
			}
		}
		// WillfulViolationRaisedイベントを発生します。
		protected virtual void OnWillfulViolationRaised(string s){
			if(ParseEventRaised != null){
				ParseEventRaised(this, new ParserEventArgs(EventLevel.Information){Message = s});
			}
		}
		protected virtual void OnInformationRaised(string s){
			if(ParseEventRaised != null){
				ParseEventRaised(this, new ParserEventArgs(EventLevel.Information){Message = s});
			}
		}


// エンコード



		// Encoding と EncodingConfidence をセットし、textReaderを初期化します。
		public void SetEncoding(Encoding enc, EncodingConfidence conf){
			if(myTextReader != null){
				//ToDo:
				throw new Exception("Encodingをあとから変更することはできません。InputStreamを初期化してください。");
			}
			this.Encoding = enc;
			this.EncodingConfidence = conf;
			OnInformationRaised(string.Format("文字符号化方式を判別しました。: {0} / {1}", enc.EncodingName, conf));
			myTextReader = new StreamReader(myStream, this.Encoding);
		}


		// バイナリStreamの先頭最大1024バイトを読み取ってEncodingを判別します。
		public void SniffEncoding(){
			OnInformationRaised(string.Format("文字符号化方式の読み取りを開始します。"));
			int length = myStream.Length > SniffEncodingBufferSize ? SniffEncodingBufferSize : (int)myStream.Length;

			byte[] buffer = new byte[length];
			myStream.Read(buffer, 0, length);
			myStream.Position = 0;

			if(length < 2) return;
			if(buffer[0] == 0xfe && buffer[1] == 0xff){
				SetEncoding(new UnicodeEncoding(true, true), EncodingConfidence.Certain);
				return;
			} else if(buffer[0] == 0xff && buffer[1] == 0xfe){
				SetEncoding(Encoding.Unicode, EncodingConfidence.Certain);
				return;
			}
			if(length < 3) return;
			if(buffer[0] == 0xEF && buffer[1] == 0xBB && buffer[2] == 0xBF){
				SetEncoding(Encoding.UTF8, EncodingConfidence.Certain);
				return;
			}

			EncodingSniffer es = new EncodingSniffer();
			Encoding result = es.SniffEncoding(buffer);
			if(result != null){
				SetEncoding(result, EncodingConfidence.Tentative);
				return;
			}
			OnInformationRaised(string.Format("文字符号化方式の読み取りに失敗しました。"));
		}



	} //  class InputStream
}



