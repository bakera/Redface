using System;
using System.Xml;

namespace Bakera.RedFace{

	public class UnKnownDoctype : DoctypeInfo{

		public override DocumentMode DocumentMode{
			get{return DocumentMode.Quirks;}
		}

	}

}

