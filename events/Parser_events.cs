using System;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public delegate void ParserEventHandler(ParserEventArgs e);
		public event ParserEventHandler TokenStateChanged;
		public event ParserEventHandler ParseErrorRaised;

		// TokenStateChangeイベントを発生します。
		protected virtual void OnTokenStateChange(){
			if(TokenStateChanged != null){
				TokenStateChanged(new ParserEventArgs(this));
			}
		}

		// ParseErrorイベントを発生します。
		protected virtual void OnParseErrorRaised(string s){
			if(ParseErrorRaised != null){
				ParseErrorRaised(new ParserErrorEventArgs(this){Message = s});
			}
		}
	}
}


