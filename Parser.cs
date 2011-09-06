using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		private List<ParserLog> myLogs = new List<ParserLog>();
		private KeyedByTypeCollection<TokenState> myTokenStateManager = new KeyedByTypeCollection<TokenState>();

		private StringBuilder myEmittedToken = new StringBuilder();
		private Queue<char> myUnConsumedQueue = new Queue<char>(); // UnConsumeされた文字をためておくキュー
		private bool myStopFlag = false;


// プロパティ
		private Stream Stream {get; set;}
		public StreamReader Reader {get; set;}
		public TokenState CurrentTokenState{get; private set;}
		public char? CurrentInputChar {get; set;}
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


// コンストラクタ

		public RedFaceParser(){}

		private void Init(){
			ChangeTokenState(typeof(DataState));
			this.CurrentLine = new Line(1);
			this.Column = 0;
			myUnConsumedQueue.Clear();
			myStopFlag = false;
		}


// メソッド

		public void Parse(){
			StartTime = DateTime.Now;
			this.Init();
			// ToDo: ラスト付近でUnConsumeが発生するとうまく動作しない可能性がある
			// Reader.Peekを見ないようにする必要がある
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
		public char? ConsumeChar(){
			CheckNewLine();
			CurrentInputChar = ReadChar();
			return CurrentInputChar;
		}

		// 指定された数だけ読み進みます。
		public string ConsumeChar(int count){
			StringBuilder result = new StringBuilder();
			for(int i=0; i < count; i++){
				result.Append(ConsumeChar());
			}
			return result.ToString();
		}

		// 読み取った文字を返してUnConsumeします。
		// 返された文字はキューに格納されて再利用されます。
		public void UnConsume(string s){
			if(string.IsNullOrEmpty(s)) return;
			Console.WriteLine("unconsume: {0}", s);
			foreach(char c in s){
				myUnConsumedQueue.Enqueue(c);
			}
			CurrentInputChar = s[0];
		}

		// 読み取った文字を返してUnConsumeします。
		// 返された文字はキューに格納されて再利用されます。
		public void UnConsume(char? c){
			if(c == null) return;
			Console.WriteLine("unconsume: {0}", c);
			myUnConsumedQueue.Enqueue((char)c);
			CurrentInputChar = c;
		}

		// CurrentInputCharをUnConsumeします。
		public void UnConsume(){
			UnConsume(CurrentInputChar);
		}

		// 次の1文字をこっそり読み取ります。
		public char? GetNextInputChar(){
			if(myUnConsumedQueue.Count > 0) return myUnConsumedQueue.Peek();
			int charNum = Reader.Peek();
			if(charNum < 0) return null;
			return (char)charNum;
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

		// 文字を読み取ります
		// キューがあればキューから、なければStreamから読み取ります。
		// EOFならnullを返します。
		private char? ReadChar(){
			if(myUnConsumedQueue.Count > 0) return myUnConsumedQueue.Dequeue();
			int charNum = Reader.Read();
			if(charNum < 0) return null;
			return (char)charNum;
		}

		// 改行かどうかを調べ、現在の行数をカウントします。
		// Todo: UnConsumeが発生すると二重にカウントされる問題が発生?
		private void CheckNewLine(){
			if(IsNewLine()){
				this.CurrentLine = new Line(this.CurrentLine);
				this.Column = 0;
			} else {
				this.CurrentLine.AddChar(CurrentInputChar);
				this.Column++;
			}
		}

		// CurentInputCharが改行かどうかを調べます。
		// 自身が LF = 改行
		// 自身が CR = 次がLFならまだ改行しない (次のLFで改行)
		private bool IsNewLine(){
			if(CurrentInputChar == Chars.LINE_FEED){
				return true;
			}
			if(CurrentInputChar == Chars.CARRIAGE_RETURN && GetNextInputChar() != Chars.LINE_FEED){
				return true;
			}
			return false;
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



