using System;
using System.IO;

namespace Bakera.RedFaceLint{

	public class Html5Parser{

// プロパティ

		private MemoryStream Stream {get; set;}
		public StreamReader Reader {get; set;}
		private TokenizationState State{get; set;}
		public char CurrentInputChar {get; set;}
		public char NextInputChar {get; set;}

		// ストリームの末端かどうかを調べ、末端ならtrueを返します。
		public bool IsEOF{
			get {return this.Reader.Peek() < 0;}
		}


// コンストラクタ

		public Html5Parser(){
			State = new DataState(this);
		}


// メソッド

		public void Parse(){
			while(Reader.Peek() >= 0){
				State.Read();
			}
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

		//
		public char ConsumeChar(){
			CurrentInputChar = NextInputChar;
			NextInputChar = (char)Reader.Read();
			return NextInputChar;
		}



	}

}



