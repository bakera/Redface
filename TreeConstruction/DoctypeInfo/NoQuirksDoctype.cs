using System;
using System.Xml;

namespace Bakera.RedFace{

	public abstract class NoQuirksDoctype : DoctypeInfo{

		public override DocumentMode DocumentMode{
			get{return DocumentMode.NoQuirks;}
		}

	}

}



