using System;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public delegate void ParserEventHandler(Object sender, EventArgs e);
		public event ParserEventHandler TokenStateChanged;
		public event ParserEventHandler ParseErrorRaised;
		public event ParserEventHandler CharacterReferenced;

		// TokenStateChangedイベントを発生します。
		protected virtual void OnTokenStateChanged(){
			if(TokenStateChanged != null){
				TokenStateChanged(this, new ParserEventArgs(this));
			}
		}

		// ParseErrorRaisedイベントを発生します。
		protected virtual void OnParseErrorRaised(string s){
			if(ParseErrorRaised != null){
				ParseErrorRaised(this, new ParseErrorEventArgs(this){Message = s});
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


