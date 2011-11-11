using System;
using System.Xml;

namespace Bakera.RedFace{

	public abstract class RedFaceParserState{

// イベント
		public event EventHandler<ParserEventArgs> ParserEventRaised;

		// ParseErrorRaisedイベントを発生します。
		protected virtual void OnParseErrorRaised(string message){
			if(ParserEventRaised != null){
				ParserEventRaised(this, new ParserEventArgs(EventLevel.ParseError){Message = message});
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
			ParserEventArgs args = new ParserEventArgs(EventLevel.Information);
			args.Element = e;
			args.Token = t;
			OnParserEventRaised(args);
		}

		// DeepBreathイベントを発生します。
		protected virtual void OnDeepBreath(){
			OnParserEventRaised(new ParserEventArgs(EventLevel.Warning));
		}

		// DocumentModeChangedイベントを発生します。
		protected virtual void OnDocumentModeChanged(){
			OnParserEventRaised(new ParserEventArgs(EventLevel.Verbose));
		}

		// InformationRaisedイベントを発生します。
		protected virtual void OnInformationRaised(string s){
			OnParserEventRaised(new ParserEventArgs(EventLevel.Information){Message = s});
		}

	}

}
