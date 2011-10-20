using System;
using System.Xml;

namespace Bakera.RedFace{

	public class MathMLElementInfo : ElementInfo{

		public MathMLElementInfo(string name){
			Name = name;
		}

		public override string Namespace{
			get{return Document.MathMLNamespace;}
		}

	}

}



