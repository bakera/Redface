using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Bakera.RedFace{

	public class StackOfOpenElements : Stack<XmlElement>{

		private static readonly ElementInfo[] DefaultScopeFilterElements = new ElementInfo[]{
			new HtmlElementInfo("applet"),
			new HtmlElementInfo("caption"),
			new HtmlElementInfo("html"),
			new HtmlElementInfo("table"),
			new HtmlElementInfo("tr"),
			new HtmlElementInfo("td"),
			new HtmlElementInfo("marquee"),
			new HtmlElementInfo("object"),
			new MathMLElementInfo("mi"),
			new MathMLElementInfo("mo"),
			new MathMLElementInfo("mn"),
			new MathMLElementInfo("ms"),
			new MathMLElementInfo("mtext"),
			new MathMLElementInfo("annotation-xml"),
			new SVGElementInfo("foreignobject"),
			new SVGElementInfo("desc"),
			new SVGElementInfo("title"),
		};
		private static readonly ElementInfo[] ListItemScopeFilterElements = new ElementInfo[]{
			new HtmlElementInfo("ol"),
			new HtmlElementInfo("ul"),
		};
		private static readonly ElementInfo[] ButtonScopeFilterElements = new ElementInfo[]{
			new HtmlElementInfo("button"),
		};
		private static readonly ElementInfo[] TableScopeFilterElements = new ElementInfo[]{
			new HtmlElementInfo("html"),
			new HtmlElementInfo("table"),
		};
		private static readonly ElementInfo[] SelectScopeNoFilterElements = new ElementInfo[]{
			new HtmlElementInfo("optgroup"),
			new HtmlElementInfo("option"),
		};


		// 上からn番目の要素を取得します。
		public XmlElement this[int n]{
			get{
				XmlElement[] elements = this.ToArray();
				int index = elements.Length -1 - n;
				return elements[index];
			}
		}


		public override string ToString(){
			if(this.Count == 0) return null;
			XmlElement[] path = this.ToArray();
			string result = "";
			for(int i = 1; i < path.Length; i++){
				XmlElement e = path[path.Length - i];
				result += string.Format("{0} > ", e.Name);
			}
			result += path[0].Name;
			return result;
		}



// Scope

		public bool HaveElementInScope(string elementName){
			return IsMatchElementInfos(elementName, DefaultScopeFilterElements);
		}

		public bool HaveElementInListItemScope(string elementName){
			return IsMatchElementInfos(elementName, DefaultScopeFilterElements, ListItemScopeFilterElements);
		}

		public bool HaveElementInButtonScope(string elementName){
			return IsMatchElementInfos(elementName, DefaultScopeFilterElements, ButtonScopeFilterElements);
		}
		public bool HaveElementInTableScope(string elementName){
			return IsMatchElementInfos(elementName, TableScopeFilterElements);
		}
		public bool HaveElementInSelectScope(string elementName){
			XmlElement[] elements = this.ToArray();
			foreach(XmlElement e in elements){
				if(e.Name == elementName) return true;
				if(!IsElementInElementInfos(e, SelectScopeNoFilterElements)) return false;
			}
			throw new Exception("This will never fail, since the loop will always terminate in the previous step if the top of the stack — an html element — is reached.");
		}



		private bool IsMatchElementInfos(string elementName, ElementInfo[] elementInfos){
			return IsMatchElementInfos(elementName, elementInfos, new ElementInfo[0]);
		}
		private bool IsMatchElementInfos(string elementName, ElementInfo[] elementInfos1, ElementInfo[] elementInfos2){
			XmlElement[] elements = this.ToArray();
			foreach(XmlElement e in elements){
				if(e.Name == elementName) return true;
				if(IsElementInElementInfos(e, elementInfos1)) return false;
				if(IsElementInElementInfos(e, elementInfos2)) return false;
			}
			throw new Exception("This will never fail, since the loop will always terminate in the previous step if the top of the stack — an html element — is reached.");
		}

		private bool IsElementInElementInfos(XmlElement e, ElementInfo[] elementInfos){
			foreach(ElementInfo ei in elementInfos){
				if(ei.IsMatch(e)) return true;
			}
			return false;
		}

	}

}



