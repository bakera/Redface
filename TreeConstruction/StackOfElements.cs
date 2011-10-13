using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Bakera.RedFace{

	public class StackOfElements : Stack<XmlElement>{

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
		private static readonly ElementInfo[] SpecialElements = new ElementInfo[]{
			new HtmlElementInfo("address"),
			new HtmlElementInfo("applet"),
			new HtmlElementInfo("area"),
			new HtmlElementInfo("article"),
			new HtmlElementInfo("aside"),
			new HtmlElementInfo("base"),
			new HtmlElementInfo("basefont"),
			new HtmlElementInfo("bgsound"),
			new HtmlElementInfo("blockquote"),
			new HtmlElementInfo("body"),
			new HtmlElementInfo("br"),
			new HtmlElementInfo("button"),
			new HtmlElementInfo("caption"),
			new HtmlElementInfo("center"),
			new HtmlElementInfo("col"),
			new HtmlElementInfo("colgroup"),
			new HtmlElementInfo("command"),
			new HtmlElementInfo("dd"),
			new HtmlElementInfo("details"),
			new HtmlElementInfo("dir"),
			new HtmlElementInfo("div"),
			new HtmlElementInfo("dl"),
			new HtmlElementInfo("dt"),
			new HtmlElementInfo("embed"),
			new HtmlElementInfo("fieldset"),
			new HtmlElementInfo("figcaption"),
			new HtmlElementInfo("figure"),
			new HtmlElementInfo("footer"),
			new HtmlElementInfo("form"),
			new HtmlElementInfo("frame"),
			new HtmlElementInfo("frameset"),
			new HtmlElementInfo("h1"),
			new HtmlElementInfo("h2"),
			new HtmlElementInfo("h3"),
			new HtmlElementInfo("h4"),
			new HtmlElementInfo("h5"),
			new HtmlElementInfo("h6"),
			new HtmlElementInfo("head"),
			new HtmlElementInfo("header"),
			new HtmlElementInfo("hgroup"),
			new HtmlElementInfo("hr"),
			new HtmlElementInfo("html"),
			new HtmlElementInfo("iframe"),
			new HtmlElementInfo("img"),
			new HtmlElementInfo("input"),
			new HtmlElementInfo("isindex"),
			new HtmlElementInfo("li"),
			new HtmlElementInfo("link"),
			new HtmlElementInfo("listing"),
			new HtmlElementInfo("marquee"),
			new HtmlElementInfo("menu"),
			new HtmlElementInfo("meta"),
			new HtmlElementInfo("nav"),
			new HtmlElementInfo("noembed"),
			new HtmlElementInfo("noframes"),
			new HtmlElementInfo("noscript"),
			new HtmlElementInfo("object"),
			new HtmlElementInfo("ol"),
			new HtmlElementInfo("p"),
			new HtmlElementInfo("param"),
			new HtmlElementInfo("plaintext"),
			new HtmlElementInfo("pre"),
			new HtmlElementInfo("script"),
			new HtmlElementInfo("section"),
			new HtmlElementInfo("select"),
			new HtmlElementInfo("style"),
			new HtmlElementInfo("summary"),
			new HtmlElementInfo("table"),
			new HtmlElementInfo("tbody"),
			new HtmlElementInfo("td"),
			new HtmlElementInfo("textarea"),
			new HtmlElementInfo("tfoot"),
			new HtmlElementInfo("th"),
			new HtmlElementInfo("thead"),
			new HtmlElementInfo("title"),
			new HtmlElementInfo("tr"),
			new HtmlElementInfo("ul"),
			new HtmlElementInfo("wbr"),
			new HtmlElementInfo("xmp"),
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
		private static readonly ElementInfo[] ImpliedEndTagElements = new ElementInfo[]{
			new HtmlElementInfo("dd"),
			new HtmlElementInfo("dt"),
			new HtmlElementInfo("li"),
			new HtmlElementInfo("option"),
			new HtmlElementInfo("optgroup"),
			new HtmlElementInfo("p"),
			new HtmlElementInfo("rp"),
			new HtmlElementInfo("rt"),
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

		public void PopUntilSameTagName(params string[] tagName){
			while(!IsCurrentNameMatch(tagName)){
				Pop();
			}
			Pop();
		}
		public void PopUntilSameElement(XmlElement e){
			while(Peek() != e){
				Pop();
			}
			Pop();
		}


// Include?

		// 渡された名前にマッチしない要素があったらその要素名を返します。
		// なければnullを返します。
		public string NotEither(params string[] elementNames){
			foreach(XmlElement e in this){
				if(IsNameMatch(e, elementNames)) return e.Name;
			}
			return null;
		}

		// CurrentNodeの名前が渡された名前にマッチすればtrueを返します。
		public bool IsCurrentNameMatch(params string[] elementNames){
			return IsNameMatch(this.Peek(), elementNames);
		}
		// CurrentNodeがImpliedEndTagElementsに属する要素ならtrueを返します。
		public bool IsImpliedEndTagElement(){
			return IsImpliedEndTagElement(this.Peek());
		}

		// XmlElementの名前が渡された名前にマッチすればtrueを返します。
		public static bool IsNameMatch(XmlElement e, params string[] elementNames){
			return Array.IndexOf(elementNames, e.Name) >= 0;
		}

		// XmlElementがSpecialに属する要素ならtrueを返します。
		public static bool IsSpecialElement(XmlElement e){
			return IsElementInElementInfos(e, SpecialElements);
		}
		// XmlElementがImpliedEndTagElementsに属する要素ならtrueを返します。
		public static bool IsImpliedEndTagElement(XmlElement e){
			return IsElementInElementInfos(e, ImpliedEndTagElements);
		}


		// 渡されたXmlElementがStackに含まれていればtrueを返します。
		public bool IsInclude(XmlElement e){
			XmlElement[] elements = this.ToArray();
			foreach(XmlElement e2 in elements){
				if(e == e2) return true;
			}
			return false;
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


// private
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

		private static bool IsElementInElementInfos(XmlElement e, ElementInfo[] elementInfos){
			foreach(ElementInfo ei in elementInfos){
				if(ei.IsMatch(e)) return true;
			}
			return false;
		}

	}

}



