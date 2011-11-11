using System;
using System.Xml;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public event EventHandler<ParserEventArgs> ParserEventRaised;

		// イベントを発生します。
		protected virtual void OnParserEventRaised(ParserEventArgs e){
			OnParserEventRaised(this, e);
		}
		protected virtual void OnParserEventRaised(Object sender, ParserEventArgs e){
			if(ParserEventRaised != null){
				e.Parser = this;
				ParserEventRaised(this, e);
			}
		}

		// InformationRaisedイベントを発生します。
		protected virtual void OnInformationRaised(string s){
			OnParserEventRaised(new ParserEventArgs(EventLevel.Verbose){Message = s});
		}

		// TokenStateChangedイベントを発生します。
		protected virtual void OnTokenStateChanged(){
			OnParserEventRaised(new ParserEventArgs(EventLevel.Verbose));
		}

		// InsertionModeChangedイベントを発生します。
		protected virtual void OnInsertionModeChanged(){
			OnParserEventRaised(new ParserEventArgs(EventLevel.Verbose));
		}

		// TokenCreatedイベントを発生します。
		protected virtual void OnTokenCreated(Token t){
			OnParserEventRaised(new ParserEventArgs(EventLevel.Verbose));
		}

		// ParseErrorRaisedイベントを発生します。
		protected virtual void OnParseErrorRaised(string s){
			OnParserEventRaised(new ParserEventArgs(EventLevel.ParseError){Message = s});
		}

		// WillfulViolationRaisedイベントを発生します。
		protected virtual void OnWillfulViolationRaised(string s){
			OnParserEventRaised(new ParserEventArgs(EventLevel.Information){Message = s});
		}

		// DocumentModeChangedイベントを発生します。
		protected virtual void OnDocumentModeChanged(){
			OnParserEventRaised(new ParserEventArgs(EventLevel.Verbose));
		}

		// ElementInsertedイベントを発生します。
		protected virtual void OnElementInserted(){
			OnParserEventRaised(new ParserEventArgs(EventLevel.Verbose));
		}

		// ImpliedEndTagInsertedイベントを発生します。
		protected virtual void OnImpliedEndTagInserted(XmlElement e, Token t){
			OnParserEventRaised(new ParserEventArgs(EventLevel.Information){Token=t,Element=e});
		}

		// DeepBreathイベントを発生します。
		protected virtual void OnDeepBreath(){
			OnParserEventRaised(new ParserEventArgs(EventLevel.Warning));
		}

		// EncodingSniffedイベントを発生します。
		protected virtual void OnEncodingSniffed(string s){
			OnParserEventRaised(new ParserEventArgs(EventLevel.Information){Message = s});
		}

	}
}


