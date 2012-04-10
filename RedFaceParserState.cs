using System;
using System.Xml;

namespace Bakera.RedFace{


	// InsertionMode と tokenizationStateの共通機能をまとめた抽象クラスです。
	public abstract class RedFaceParserState : ParserEventSender{

		public override string ToString(){
			return this.GetType().Name;
		}



// イベント

		// ImpliedEndTagInsertedイベントを発生します。
		protected virtual void OnImpliedEndTagInserted(XmlElement e, Token t){
			ParserEventArgs args = new ParserEventArgs(EventLevel.Information);
			args.Element = e;
			args.Token = t;
			args.Message = string.Format("省略されている終了タグを補いました。: {0}", e.Name);
			OnParserEventRaised(args);
		}

		// InformationRaisedイベントを発生します。
		protected virtual void OnInformationRaised(string s){
			OnParserEventRaised(new ParserEventArgs(EventLevel.Information){Message = s});
		}

	}

}
