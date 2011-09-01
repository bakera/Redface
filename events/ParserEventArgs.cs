using System;
using System.Collections.Generic;

namespace Bakera.RedFace{

	public class ParserEventArgs : EventArgs{

		public ParserEventArgs(RedFaceParser p){
			this.Parser = p;
		}

		public RedFaceParser Parser{get; private set;}
		public string Message{get; set;}
	}

}



