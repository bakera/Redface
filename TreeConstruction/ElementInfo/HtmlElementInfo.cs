using System;
using System.Xml;

namespace Bakera.RedFace{

	public class HtmlElementInfo : ElementInfo{

		public HtmlElementInfo(string name){
			Name = name;
		}

		public override string Namespace{
			get{return Document.HtmlNamespace;}
		}
	}

}



