using System;
using System.Xml;

namespace Bakera.RedFace{

	public class ParserEventArgs : EventArgs{

		public ParserEventArgs(){}
		public ParserEventArgs(RedFaceParser p){
			this.Parser = p;
		}

		public RedFaceParser Parser{get; private set;}
		public XmlElement Element{get; set;}
		public Token Token{get; set;}
		public string Message{get; set;}
	}

}



