using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Bakera.RedFace{


	public class RedFaceParser : ParserEventSender{

		private List<ParserLog> myLogs = new List<ParserLog>();

		private bool myStopFlag = false;
		private bool myEncodingChangingFlag = true;
		private Tokenizer myTokenizer = null;
		private TreeConstruction myTreeConstruction = null;
		private Stream myStream = null;
		private InputStream myInputStream = null;
		private bool myFramesetOK = true;
		private Encoding myDefaultEncoding = Encoding.UTF8;
		private Encoding myForceEncoding = null;



// プロパティ
		public DateTime StartTime {get; private set;}
		public DateTime EndTime {get; private set;}
		public string CurrentTokenState{
			get{
				return myTokenizer.CurrentTokenState.Name;
			}
		}
		public string CurrentInsertionMode{
			get{
				return myTreeConstruction.CurrentInsertionMode.Name;
			}
		}

		public bool IsStopped{
			get{
				return myStopFlag;
			}
		}

		public Document Document{
			get{
				return myTreeConstruction.Document;
			}
		}

		public bool FramesetOK{
			get{return myFramesetOK;}
			set{myFramesetOK = value;}
		}

		public bool Scripting{
			get{return false;}
		}

		public StackOfElements StackOfOpenElements{
			get{
				return myTreeConstruction.StackOfOpenElements;
			}
		}

		public EncodingConfidence EncodingConfidence{
			get{return myTokenizer.InputStream.EncodingConfidence;}
		}
		public InputStream InputStream{
			get{return myInputStream;}
		}

// コンストラクタ



// メソッド

		public ParserLog[] GetLogs(){
			return myLogs.ToArray();
		}




// パース系


		public void Parse(Stream s){
			myStream = s;
			Initialize();
			StartTime = DateTime.Now;

			if(myForceEncoding != null){
				myInputStream.SetEncoding(myForceEncoding, EncodingConfidence.Certain);
			} else {
				Encoding enc = myInputStream.SniffEncoding();
				if(enc == null){
					OnMessageRaised(EventLevel.Information, string.Format("文字符号化方式の読み取りに失敗しました。既定の文字符号化方式を使用します。: {0}", myDefaultEncoding.EncodingName));
					myInputStream.SetEncoding(myDefaultEncoding, EncodingConfidence.Tentative);
				} else {
					myInputStream.SetEncoding(enc, EncodingConfidence.Tentative);
				}
			}
			OnMessageRaised(EventLevel.Information, string.Format("構文解析を開始します。"));

			// パースする
			TreeConstruct();
			// EncodingChangedイベントで停止した場合、一度だけ再実行
			if(myEncodingChangingFlag){
				OnMessageRaised(EventLevel.Warning, string.Format("構文解析の途中で文字符号化方式が判明したため、構文解析をやり直します。判明した文字符号化方式: {0}", myForceEncoding.EncodingName));
				myStopFlag = false;
				myEncodingChangingFlag = false;
				Initialize();
				myInputStream.SetEncoding(myForceEncoding, EncodingConfidence.Certain);
				TreeConstruct();
			}

			EndTime = DateTime.Now;
		}

		// パース停止フラグをONにします。
		// 次のトークンの読み取りを止めて停止します。
		public void Stop(){
			// ToDo: Stop処理実装
			myStopFlag = true;
		}

		public void ChangeTokenState<T>() where T : TokenizationState, new(){
			myTokenizer.ChangeTokenState<T>();
		}

		private void TreeConstruct(){
			while(!myStopFlag){
				Token t = myTokenizer.GetToken();
				if(t == null) continue;
				OnMessageRaised(EventLevel.Verbose, string.Format("Tokenを作成しました。: {0}", t));
				// AppendTokenを実行。
				// 実行後にReprocessフラグが立てられている場合は同じトークンを再処理する
				do{
					myTreeConstruction.ReprocessFlag = false;
					myTreeConstruction.AppendToken(t);
				} while(myTreeConstruction.ReprocessFlag);

				if(t is EndOfFileToken) break;
			}
		}

		private void EncodeChanged(Object sender, EncodingChangedEventArgs e){
			myStopFlag = true;
			myEncodingChangingFlag = true;
			myForceEncoding = e.Encoding;
		}

		private void Initialize(){
			myStream.Position = 0;
			myInputStream = new InputStream(myStream);
			myInputStream.ParserEventRaised += OnParserEventRaised;
			myInputStream.EncodingChanged += EncodeChanged;
			myTokenizer = new Tokenizer(this);
			myTokenizer.ParserEventRaised += OnParserEventRaised;
			myTreeConstruction = new TreeConstruction(this);
			myTreeConstruction.ParserEventRaised += OnParserEventRaised;
		}


// エンコード

		// charsetを明示的に指定します。
		// encoding判定は行われず、ここで指定したEncodingが強制的に使用されるようになります。
		public void SetForceEncoding(string s){
			myForceEncoding = EncodingSniffer.GetEncodingByName(s);
		}

		// デフォルトのcharsetを指定します。
		// encoding判定に失敗した際に、ここで指定したEncodingが使用されるようになります。
		// このメソッドを呼ばない場合のデフォルトは UTF-8 です。
		public void SetDefaultEncoding(string s){
			myDefaultEncoding = EncodingSniffer.GetEncodingByName(s);
		}



	}

}



