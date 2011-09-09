using System;
using System.Reflection;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public abstract class InsertionMode{
			
			protected RedFaceParser myParser = null;

	// コンストラクタ
			public InsertionMode(RedFaceParser p){
				myParser = p;
			}

	// 抽象メソッド
			public virtual void Read(){}

	// ファクトリ

			public static InsertionMode CreateInsertionMode(Type t, RedFaceParser parser){
				if(!(typeof(InsertionMode)).IsAssignableFrom(t)){
					throw new Exception("CreateInsertionModeメソッドはInsertionModeしか作成できません。渡された型 :" + t.ToString());
				}
				ConstructorInfo ci = t.GetConstructor(new Type[]{typeof(RedFaceParser)});
				InsertionMode result = ci.Invoke(new Object[]{parser}) as InsertionMode;
				return result;
			}

	// メソッド


			public abstract void AppendToken(Token t);

		}
	}
}
