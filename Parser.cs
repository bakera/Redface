using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Bakera.RedFace{


	public partial class RedFaceParser{

		private List<ParserLog> myLogs = new List<ParserLog>();

		private bool myStopFlag = false;
		private Tokenizer myTokenizer = null;
		private TreeConstruction myTreeConstruction = null;
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

// コンストラクタ



// メソッド

		public ParserLog[] GetLogs(){
			return myLogs.ToArray();
		}




// パース系


		public void Parse(Stream s){
			StartTime = DateTime.Now;
			InputStream stream = new InputStream(this, myDefaultEncoding, s);
			stream.ParseEventRaised += OnParserEventRaised;
			myTokenizer = new Tokenizer(this, stream);
			myTokenizer.ParserEventRaised += OnParserEventRaised;
			myTreeConstruction = new TreeConstruction(this);
			myTreeConstruction.ParserEventRaised += OnParserEventRaised;

			if(myForceEncoding != null){
				stream.SetEncoding(myForceEncoding, EncodingConfidence.Certain);
			} else {
				stream.SniffEncoding();
			}
			TreeConstruct();

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
				OnTokenCreated(t);
				// AppendTokenを実行。
				// 実行後にReprocessフラグが立てられている場合は同じトークンを再処理する
				do{
					myTreeConstruction.ReprocessFlag = false;
					myTreeConstruction.AppendToken(t);
				} while(myTreeConstruction.ReprocessFlag);

				if(t is EndOfFileToken) break;
			}
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



