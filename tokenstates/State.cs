using System;
using System.IO;

namespace Bakera.RedFaceLint{

	public abstract class TokenizationState{
		
		protected RedFaceParser Parser{get; set;}


// プロパティ


// コンストラクタ
		public TokenizationState(RedFaceParser p){
			Parser = p;
		}

// 抽象メソッド
		public virtual void Read(){
		}


// メソッド

		// 1文字読み進めます。
		protected char ReadChar(){
			char c = (char)Parser.Reader.Read();
			return c;
		}

		// その場で次の1文字を読みます。
		protected char PeekChar(){
			char c = (char)Parser.Reader.Peek();
			return c;
		}


	}

}
