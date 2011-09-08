using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		private List<ParserLog> myLogs = new List<ParserLog>();
		private KeyedByTypeCollection<TokenState> myTokenStateManager = new KeyedByTypeCollection<TokenState>();

		private StringBuilder myEmittedToken = new StringBuilder();
		private bool myStopFlag = false;
		private InputStream myInputStream = null;



// プロパティ
		private Stream Stream {get; set;}
		public StreamReader Reader {get; set;}
		public TokenState CurrentTokenState{get; private set;}
		public Line CurrentLine {get; private set;}
		public int Column {get; private set;}
		public DateTime StartTime {get; private set;}
		public DateTime EndTime {get; private set;}

		public long CurrentPosition{
			get{
				if(Stream == null) return 0;
				return Stream.Position;
			}
		}

		public string EmittedToken{
			get{
				return myEmittedToken.ToString();
			}
		}

		public char? CurrentInputChar {
			get{
				return myInputStream.CurrentInputChar;
			}
		}


// コンストラクタ

		public RedFaceParser(){}

		private void Init(){
			ChangeTokenState(typeof(DataState));
			this.CurrentLine = new Line(1);
			this.Column = 0;
			myStopFlag = false;
		}


// メソッド

		public void Parse(){
			StartTime = DateTime.Now;
			this.Init();
			myInputStream = new InputStream(this, Reader);
			while(!myStopFlag && Reader.Peek() >= 0){
				CurrentTokenState.Read();
			}
			EndTime = DateTime.Now;
		}

		public ParserLog[] GetLogs(){
			return myLogs.ToArray();
		}


// パース系

		// パース停止フラグをONにします。
		// 次の文字の読み取りを止めて停止します。
		public void Stop(){
			myStopFlag = true;
		}

		// トークン走査状態を変更します。
		public void ChangeTokenState(Type t){
			if(CurrentTokenState != null && t == CurrentTokenState.GetType()) return;
			if(!myTokenStateManager.Contains(t)){
				myTokenStateManager.Add(TokenState.CreateTokenState(t, this));
			}
			CurrentTokenState = myTokenStateManager[t];
			OnTokenStateChanged();
		}

		// 一つ読み進みます。
		public void ConsumeChar(){
			myInputStream.ConsumeNextInputChar();
		}

		// 指定された文字数の文字を読んで文字列を返します。
		public string ConsumeChar(int n){
			StringBuilder result = new StringBuilder();
			for(int i=0; i < n; i++){
				ConsumeChar();
				result.Append(CurrentInputChar);
			}
			return result.ToString();
		}


		// 読み取った文字を返してUnConsumeします。
		// 返された文字はキューに格納されて再利用されます。
		public void UnConsume(int offset){
			myInputStream.UnConsume(offset);
		}

		// CurrentInputCharをUnConsumeします。
		public void UnConsume(){
			myInputStream.UnConsume(1);
		}


		// 文字を受け取ります。
		public void Emit(){
			Emit(CurrentInputChar);
		}
		// 文字を受け取ります。
		public void Emit(char? c){
			Emit(c.ToString());
		}
		// 文字を受け取ります。
		public void Emit(string s){
			myEmittedToken.Append(s);
		}


// エラー記録

		public void AddError(string message){
			ParserError pe = new ParserError(){Message = message};
			AddError(pe);
		}
		public void AddError(ParserError pe){
			pe.Line = this.CurrentLine;
			pe.ColumnNumber = this.Column;
			myLogs.Add(pe);
		}

// ロード


		// 指定されたファイルからデータを読み取ります。
		public void Load(FileInfo file){
			using(FileStream fs = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read)){
				Stream = new MemoryStream();
				fs.CopyTo(Stream);
			}
			Stream.Position = 0;
			Reader = new StreamReader(Stream);
		}


		// 指定されたストリームを読み取ります。
		public void Load(Stream s){
			if(s.CanSeek){
				Stream = s;
			} else {
				Stream = new MemoryStream();
				s.CopyTo(Stream);
				Stream.Position = 0;
			}
			Reader = new StreamReader(Stream);
		}


	}

}



