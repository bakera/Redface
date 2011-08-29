using System;
using System.IO;

namespace Bakera.RedFaceLint{

	public abstract class TokenizationState{
		
		protected Html5Parser Parser{get; set;}
		public const char NULL              = '\x00';
		public const char LESS_THAN_SIGN    = '<';
		public const char GREATER_THAN_SIGN = '>';
		public const char EXCLAMATION_MARK  = '!';
		public const char SOLIDUS		    = '/';
		public const char QUESTION_MARK     = '?';
		public const char CHARACTER_TABULATION = '\x09';
		public const char LINE_FEED = '\x0a';
		public const char FORM_FEED = '\x0c';
		public const char HYPHEN_MINUS = '-';
		public const char EQUALS_SIGN = '=';
		public const char QUOTATION_MARK = '\x22';
		public const char APOSTROPHE = '\x27';
		public const char AMPERSAND  = '&';
		public const char NUMBER_SIGN  = '#';


// プロパティ


// コンストラクタ
		public TokenizationState(Html5Parser p){
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
