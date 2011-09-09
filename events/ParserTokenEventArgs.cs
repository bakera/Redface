using System;
using System.Collections.Generic;

namespace Bakera.RedFace{

	public class ParserTokenEventArgs : ParserEventArgs{
		public Token Token{get; private set;}

		public ParserTokenEventArgs(RedFaceParser p, Token t) : base(p){
			this.Token = t;
		}
	}

}



