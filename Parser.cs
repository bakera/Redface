using System;
using System.IO;
using System.Collections.Generic;

namespace Bakera.RedFaceLint{

	public class RedFaceParser{

// フィールド

		private List<ParserLog> myLogs = new List<ParserLog>();


// プロパティ
		private MemoryStream Stream {get; set;}
		public StreamReader Reader {get; set;}
		private TokenizationState State{get; set;}
		public char? CurrentInputChar {get; set;}
		public char? NextInputChar {get; set;}
		public Line CurrentLine {get; private set;}
		public int Column {get; private set;}



// コンストラクタ

		public RedFaceParser(){
			this.State = new DataState(this);
			this.CurrentLine = new Line(1);
			this.Column = 0;
		}


// メソッド

		public void Parse(){
			while(Reader.Peek() >= 0){
				State.Read();
			}
		}

		public ParserLog[] GetLogs(){
			return myLogs.ToArray();
		}




		public void AddError(string message){
			ParserError pe = new ParserError(){Message = message};
			AddError(pe);
		}
		public void AddError(ParserError pe){
			pe.Line = this.CurrentLine;
			pe.ColumnNumber = this.Column;
			myLogs.Add(pe);
		}


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

	}

}



