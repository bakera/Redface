using System;
using System.Xml;

namespace Bakera.RedFace{

	public class SVGElementInfo : ElementInfo{

		public SVGElementInfo(string name){
			Name = name;
		}

		public override string Namespace{
			get{return Document.SVGNamespace;}
		}
	}

}



