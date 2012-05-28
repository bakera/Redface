using System;
using System.Xml;

namespace Bakera.RedFace{


	// InsertionMode と tokenizationStateの共通機能をまとめた抽象クラスです。
	public abstract class RedFaceParserState : ParserEventSender{

		public override string ToString(){
			return this.GetType().Name;
		}

	}

}
