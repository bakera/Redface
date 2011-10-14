using System;
using System.Xml;

namespace Bakera.RedFace{

	public class ParserElementEventArgs : ParserTokenEventArgs{
		public XmlElement Element{get; private set;}

		public ParserElementEventArgs(RedFaceParser p, Token t, XmlElement e) : base(p, t){
			this.Element = e;
		}
	}

}



