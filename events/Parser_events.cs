using System;
using System.Xml;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public delegate void ParserEventHandler(Object sender, EventArgs e);
		public event ParserEventHandler TokenStateChanged;
		public event ParserEventHandler InsertionModeChanged;
		public event ParserEventHandler ParseErrorRaised;
		public event ParserEventHandler WillfulViolationRaised;
		public event ParserEventHandler TokenCreated;
		public event ParserEventHandler DocumentModeChanged;
		public event ParserEventHandler ElementInserted;
		public event ParserEventHandler ImpliedEndTagInserted;
		public event ParserEventHandler DeepBreath;

		// TokenStateChangedイベントを発生します。
		protected virtual void OnTokenStateChanged(){
			if(TokenStateChanged != null){
				TokenStateChanged(this, new ParserEventArgs(this));
			}
		}

		// InsertionModeChangedイベントを発生します。
		protected virtual void OnInsertionModeChanged(){
			if(InsertionModeChanged != null){
				InsertionModeChanged(this, new ParserEventArgs(this));
			}
		}

		// TokenCreatedイベントを発生します。
		protected virtual void OnTokenCreated(Token t){
			if(TokenCreated != null){
				TokenCreated(this, new ParserTokenEventArgs(this, t));
			}
		}

		// ParseErrorRaisedイベントを発生します。
		protected virtual void OnParseErrorRaised(string s){
			if(ParseErrorRaised != null){
				ParseErrorRaised(this, new ParseErrorEventArgs(this){Message = s});
			}
		}

		// WillfulViolationRaisedイベントを発生します。
		protected virtual void OnWillfulViolationRaised(string s){
			if(WillfulViolationRaised != null){
				WillfulViolationRaised(this, new ParseErrorEventArgs(this){Message = s});
			}
		}

		// DocumentModeChangedイベントを発生します。
		protected virtual void OnDocumentModeChanged(){
			if(DocumentModeChanged != null){
				DocumentModeChanged(this, new ParseErrorEventArgs(this));
			}
		}

		// ElementInsertedイベントを発生します。
		protected virtual void OnElementInserted(){
			if(ElementInserted != null){
				ElementInserted(this, new ParserEventArgs(this));
			}
		}

		// ImpliedEndTagInsertedイベントを発生します。
		protected virtual void OnImpliedEndTagInserted(XmlElement e, Token t){
			if(ImpliedEndTagInserted != null){
				ImpliedEndTagInserted(this, new ParserElementEventArgs(this, t, e));
			}
		}

		// DeepBreathイベントを発生します。
		protected virtual void OnDeepBreath(){
			if(DeepBreath != null){
				DeepBreath(this, new ParserEventArgs(this));
			}
		}

	}
}


