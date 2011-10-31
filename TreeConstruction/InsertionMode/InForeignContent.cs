using System;
using System.Xml;

namespace Bakera.RedFace{

	public class InForeignContent : InsertionMode{

		public override void AppendAnythingElse(TreeConstruction tree, Token token){
			throw new Exception(string.Format("このモードでの処理が定義されていないトークンです。モード: {0} トークン: {1}", this.Name, token));
		}

	}
}
