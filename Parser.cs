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



// プロパティ
		public DateTime StartTime {get; private set;}
		public DateTime EndTime {get; private set;}
		public string CurrentTokenState{
			get{
				return myTokenizer.CurrentTokenState.Name;
			}
		}

		public bool IsStopped{
			get{
				return myStopFlag;
			}
		}

		public XmlDocument Tree{
			get{
				return myTreeConstruction.Document;
			}
		}



// コンストラクタ

		public RedFaceParser(){}

		private void Init(){}



// メソッド

		public ParserLog[] GetLogs(){
			return myLogs.ToArray();
		}



// パース系

		public void Parse(Stream s){
			StartTime = DateTime.Now;
			this.Init();

			TextReader tr = new StreamReader(s);
			InputStream stream = new InputStream(this, tr);
			myTokenizer = new Tokenizer(this, stream);
			myTreeConstruction = new TreeConstruction(this);

			while(!myStopFlag){
				Token t = myTokenizer.GetToken();
				if(t == null) continue;
				OnTokenCreated(t);
				myTreeConstruction.AppendToken(t);
				if(t is EndOfFileToken) break;
			}
			EndTime = DateTime.Now;
		}

		// パース停止フラグをONにします。
		// 次の文字の読み取りを止めて停止します。
		public void Stop(){
			myStopFlag = true;
		}



// エラー記録

		public void AddError(string message){
			ParserError pe = new ParserError(){Message = message};
			AddError(pe);
		}
		public void AddError(ParserError pe){
			myLogs.Add(pe);
		}



	}

}



