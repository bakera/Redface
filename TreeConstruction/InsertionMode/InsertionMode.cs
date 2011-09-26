using System;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public abstract class InsertionMode : RedFaceParserState{
			

	// メソッド
			public abstract void AppendToken(TreeConstruction tc, Token t);

	// プロパティ
			public virtual string Name{
				get{return this.GetType().Name;}
			}

		}
	}
}
