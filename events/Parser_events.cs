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
				ParserEventRaised(this, e);
			}
		}

		// TokenStateChangedイベントを発生します。
		protected virtual void OnTokenStateChanged(){
			OnParserEventRaised(new ParserEventArgs());
		}

		// InsertionModeChangedイベントを発生します。
		protected virtual void OnInsertionModeChanged(){
			OnParserEventRaised(new ParserEventArgs());
		}

		// TokenCreatedイベントを発生します。
		protected virtual void OnTokenCreated(Token t){
			OnParserEventRaised(new ParserEventArgs(){Token=t});
		}

		// ParseErrorRaisedイベントを発生します。
		protected virtual void OnParseErrorRaised(string s){
			OnParserEventRaised(new ParserEventArgs(){Message = s});
		}

		// WillfulViolationRaisedイベントを発生します。
		protected virtual void OnWillfulViolationRaised(string s){
			OnParserEventRaised(new ParserEventArgs(){Message = s});
		}

		// DocumentModeChangedイベントを発生します。
		protected virtual void OnDocumentModeChanged(){
			OnParserEventRaised(new ParserEventArgs());
		}

		// ElementInsertedイベントを発生します。
		protected virtual void OnElementInserted(){
			OnParserEventRaised(new ParserEventArgs());
		}

		// ImpliedEndTagInsertedイベントを発生します。
		protected virtual void OnImpliedEndTagInserted(XmlElement e, Token t){
			OnParserEventRaised(new ParserEventArgs(){Token=t,Element=e});
		}

		// DeepBreathイベントを発生します。
		protected virtual void OnDeepBreath(){
			OnParserEventRaised(new ParserEventArgs());
		}

	}
}


