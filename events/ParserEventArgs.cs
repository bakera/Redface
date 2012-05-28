using System;
using System.Xml;

namespace Bakera.RedFace{

	public class ParserEventArgs : EventArgs{

		public ParserEventArgs(ParserMessage message){
			this.Level = message.Level;
			this.Message = message;
		}
		public RedFaceParser Parser{get; set;}
		public XmlElement Element{get; set;}
		public Token Token{get; set;}
		public InsertionMode InsertionMode{get; set;}
		public TokenizationState TokenizationState{get; set;}
		public ParserMessage Message{get; private set;}
		public EventLevel Level{get;set;}
		public ParserEventSender OriginalSender{get;set;}
	}

}



