using System;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public delegate void ParserEventHandler(Object sender, EventArgs e);
		public event ParserEventHandler TokenStateChanged;
		public event ParserEventHandler InsertionModeChanged;
		public event ParserEventHandler ParseErrorRaised;
		public event ParserEventHandler WillfulViolationRaised;
		public event ParserEventHandler CharacterReferenced;
		public event ParserEventHandler TokenCreated;

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

		// CharacterReferencedイベントを発生します。
		protected virtual void OnCharacterReferenced(string original, string result){
			if(CharacterReferenced != null){
				CharacterReferenced(this, new CharacterReferencedEventArgs(this){OriginalString=original, Result = result});
			}
		}


	}
}

