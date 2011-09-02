using System;
using System.Collections.Generic;

namespace Bakera.RedFace{

	public class CharacterReferencedEventArgs : ParserEventArgs{

		public CharacterReferencedEventArgs(RedFaceParser p) : base(p){}

		public string OriginalString{get;set;}
		public string Result{get;set;}

	}

}



