using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Bakera.RedFace{

	public class ListOfElements : List<XmlElement>{
		private static readonly ElementInfo[] FormatElements = new ElementInfo[]{
			new HtmlElementInfo("a"),
			new HtmlElementInfo("b"),
			new HtmlElementInfo("big"),
			new HtmlElementInfo("code"),
			new HtmlElementInfo("em"),
			new HtmlElementInfo("font"),
			new HtmlElementInfo("i"),
			new HtmlElementInfo("nobr"),
			new HtmlElementInfo("s"),
			new HtmlElementInfo("small"),
			new HtmlElementInfo("strike"),
			new HtmlElementInfo("strong"),
			new HtmlElementInfo("tt"),
			new HtmlElementInfo("u"),
		};


		public void Push(XmlElement e, Token t){
			// ToDo: Push の仕組みを作る
		}

	}

}



