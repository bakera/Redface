using System;
using System.Xml;

namespace Bakera.RedFace{

	public abstract class RedFaceParserState{

// イベント
		public event EventHandler<ParserEventArgs> ParserEventRaised;

		// ParseErrorRaisedイベントを発生します。
		protected virtual void OnParseErrorRaised(string message){
			if(ParserEventRaised != null){
				ParserEventRaised(this, new ParserEventArgs(){Message = message});
			}
		}

		// イベントを発生します。
		protected virtual void OnParserEventRaised(ParserEventArgs e){
			if(ParserEventRaised != null){
				ParserEventRaised(this, e);
			}
		}

		// ImpliedEndTagInsertedイベントを発生します。
		protected virtual void OnImpliedEndTagInserted(XmlElement e, Token t){
			ParserEventArgs args = new ParserEventArgs();
			args.Element = e;
			args.Token = t;
			OnParserEventRaised(args);
			
		}

		// DeepBreathイベントを発生します。
		protected virtual void OnDeepBreath(){
			OnParserEventRaised(new ParserEventArgs());
		}

		// DocumentModeChangedイベントを発生します。
		protected virtual void OnDocumentModeChanged(){
			OnParserEventRaised(new ParserEventArgs());
		}

	}

}
