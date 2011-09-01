using System;
using System.IO;

namespace Bakera.RedFace{

	public abstract class ParserLog{
		public string Message {get; set;}
		public Line Line{get; set;}
		public int ColumnNumber{get; set;}
	}

}
