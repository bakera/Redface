using System;
using System.IO;
using System.Collections.Generic;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		private List<ParserLog> myLogs = new List<ParserLog>();
		private Dictionary<Type, TokenState> myTokenStateManager = new Dictionary<Type, TokenState>();

		private long myUnConsumePosition = 0; // UnConsumeしたときの戻り先


// プロパティ
		private MemoryStream Stream {get; set;}
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



// コンストラクタ

		public RedFaceParser(){}

		private void Init(){
			ChangeTokenState(typeof(DataState));
			this.CurrentLine = new Line(1);
			this.Column = 0;
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
			OnTokenStateChange();
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
		public void Emit(string s){
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

		// 指定されたファイル名のファイルを読み取り、メモリに格納します。
		public void Load(string filename){
			FileInfo f = new FileInfo(filename);
			Load(f);
		}

		// 指定されたファイルを読み取り、メモリに格納します。
		public void Load(FileInfo file){
			using(FileStream fs = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read)){
				Stream = new MemoryStream();
				fs.CopyTo(Stream);
			}
			Stream.Position = 0;
			Reader = new StreamReader(Stream);
		}


	}

}



