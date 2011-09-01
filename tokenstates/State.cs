using System;
using System.IO;
using System.Reflection;

namespace Bakera.RedFaceLint{

	public abstract class TokenState{
		
		protected RedFaceParser Parser{get; set;}


// プロパティ


// コンストラクタ
		public TokenState(RedFaceParser p){
			Parser = p;
		}

// 抽象メソッド
		public virtual void Read(){
		}

// ファクトリ

		public static TokenState CreateTokenState(Type t, RedFaceParser parser){
			if(!(typeof(TokenState)).IsAssignableFrom(t)){
				throw new Exception("CreateTokenStateメソッドはTokenStateしか作成できません。渡された型 :" + t.ToString());
			}
			ConstructorInfo ci = t.GetConstructor(new Type[]{typeof(RedFaceParser)});
			TokenState result = ci.Invoke(new Object[]{parser}) as TokenState;
			return result;
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
