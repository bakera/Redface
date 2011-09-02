using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		private List<ParserLog> myLogs = new List<ParserLog>();
		private Dictionary<Type, TokenState> myTokenStateManager = new Dictionary<Type, TokenState>();

		private long myUnConsumePosition = 0; // UnConsumeしたときの戻り先
		private StringBuilder myEmittedToken = new StringBuilder();


// プロパティ
		private Stream Stream {get; set;}
		public StreamReader Reader {get; set;}
		public TokenState CurrentTokenState{get; private set;}
		public char? CurrentInputChar {get; set;}
		public char? NextInputChar {get; set;}
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
			ConsumeChar();
		}


// メソッド

		public void Parse(){
			StartTime = DateTime.Now;
			this.Init();
			while(Reader.Peek() >= 0){
				CurrentTokenState.Read();
			}
			EndTime = DateTime.Now;
		}

		public ParserLog[] GetLogs(){
			return myLogs.ToArray();
		}


// パース系

		// トークン走査状態を変更します。
		public void ChangeTokenState(Type t){
			if(CurrentTokenState != null && t == CurrentTokenState.GetType()) return;
			if(!myTokenStateManager.ContainsKey(t)){
				myTokenStateManager[t] = TokenState.CreateTokenState(t, this);
			}
			CurrentTokenState = myTokenStateManager[t];
			OnTokenStateChanged();
		}

		// 一つ読み進みます。
		public void ConsumeChar(){
			if(IsNewLine()){
				this.CurrentLine = new Line(this.CurrentLine);
				this.Column = 0;
			} else {
				this.CurrentLine.AddChar(CurrentInputChar);
				this.Column++;
			}
			CurrentInputChar = NextInputChar;
			int charNum = Reader.Read();
			if(charNum == 0) {
				NextInputChar = null;
			} else {
				NextInputChar = (char)charNum;
			}
			return;
		}

		// 指定された数だけ読み進みます。
		public void ConsumeChar(int count){
			for(int i=0; i < count; i++) ConsumeChar();
		}

		// UnConsumeできるように現在の位置を記憶します。
		public void SaveUnConsumePosition(){
			myUnConsumePosition = CurrentPosition;
		}

		// UnConsumeします。
		public void UnConsume(){
			Stream.Seek(myUnConsumePosition, SeekOrigin.Begin);
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

		// CurentInputCharが改行かどうかを調べます。
		// 自身が LF = 改行
		// 自身が CR = 次がLFならまだ改行しない (次のLFで改行)
		private bool IsNewLine(){
			if(CurrentInputChar == Chars.LINE_FEED){
				return true;
			}
			if(CurrentInputChar == Chars.CARRIAGE_RETURN && NextInputChar != Chars.LINE_FEED){
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



