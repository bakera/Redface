using System;
using System.Xml;

namespace Bakera.RedFace{

	public class ParserEventArgs : EventArgs{

		public ParserEventArgs(EventLevel level){
			this.Level = level;
		}
		public RedFaceParser Parser{get; set;}
		public XmlElement Element{get; set;}
		public Token Token{get; set;}
		public InsertionMode InsertionMode{get; set;}
		public TokenizationState TokenizationState{get; set;}
		public string Message{get; set;}
		public EventLevel Level{get;set;}
	}

}



