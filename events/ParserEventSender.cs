using System;
using System.Xml;

namespace Bakera.RedFace{


	// ParserEventArgsを伴うイベントを発生させるクラスを表す抽象クラスです。
	public abstract class ParserEventSender{

		public event EventHandler<ParserEventArgs> ParserEventRaised;


		// イベントを発生します。
		protected virtual void OnParserEventRaised(ParserEventArgs e){
			OnParserEventRaised(this, e);
		}
		protected virtual void OnParserEventRaised(Object sender, ParserEventArgs e){
			if(ParserEventRaised != null){
				ParserEventRaised(this, e);
			}
		}

		// イベントレベルを指定して、メッセージを通知します。
		protected virtual void OnMessageRaised(EventLevel level, string s = null){
			ParserEventArgs args = new ParserEventArgs(level);
			if(s != null) args.Message = s;
			OnParserEventRaised(args);
		}

		// ParseErrorRaisedイベントを発生します。
		protected virtual void OnParseErrorRaised(string message){
			OnParserEventRaised(this, new ParserEventArgs(EventLevel.ParseError){Message = message});
		}


	}
}

