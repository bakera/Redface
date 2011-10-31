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
		private static readonly ElementInfo[] FormattingElements = new ElementInfo[]{
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
		private static readonly ElementInfo[] TableRealtedElements = new ElementInfo[]{
			new HtmlElementInfo("table"),
			new HtmlElementInfo("tbody"),
			new HtmlElementInfo("tfoot"),
			new HtmlElementInfo("thead"),
			new HtmlElementInfo("tr"),
		};
		private static readonly ElementInfo[] TbodyClearElements = new ElementInfo[]{
			new HtmlElementInfo("html"),
			new HtmlElementInfo("tbody"),
			new HtmlElementInfo("tfoot"),
			new HtmlElementInfo("thead"),
		};
		private static readonly ElementInfo[] TableRowClearElements = new ElementInfo[]{
			new HtmlElementInfo("tr"),
			new HtmlElementInfo("html"),
		};
		private static readonly ElementInfo TableElement = new HtmlElementInfo("table");
		private static readonly ElementInfo HtmlRootElement = new HtmlElementInfo("html");

		// 上からn番目の要素を取得します。
		public XmlElement this[int n]{
			get{
				XmlElement[] elements = this.ToArray();
				int index = elements.Length -1 - n;
				return elements[index];
			}
		}


		public override string ToString(){
			XmlElement[] path = this.ToArray();
			string result = "(document)";
			for(int i = 0; i < path.Length; i++){
				XmlElement e = path[path.Length - i - 1];
				result += string.Format(" > {0}", e.Name);
			}
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
				if(!IsNameMatch(e, elementNames)) return e.Name;
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
		// XmlElementがFormattingElementに属する要素ならtrueを返します。
		public bool IsFormattingElement(XmlElement e){
			return IsElementInElementInfos(e, FormattingElements);
		}
		// XmlElementがTableRealtedElementsに属する要素ならtrueを返します。
		public bool IsTableRealtedElement(XmlElement e){
			return IsElementInElementInfos(e, TableRealtedElements);
		}

		// 渡されたXmlElementがStackに含まれていればtrueを返します。
		public bool IsInclude(XmlElement e){
			XmlElement[] elements = this.ToArray();
			foreach(XmlElement e2 in elements){
				if(e == e2) return true;
			}
			return false;
		}



// Get 

		// 渡された要素よりも浅い階層にあるspecialに属する要素を取得します。
		public XmlElement GetFurthestBlock(XmlElement e){
			XmlElement[] elements = this.ToArray();
			List<XmlElement> result = new List<XmlElement>();

			for(int i = 0; i < elements.Length; i++){
				if(elements[i] == e) break;
				if(IsElementInElementInfos(elements[i], SpecialElements)) result.Add(elements[i]);
			}
			if(result.Count == 0) return null;
			return result[result.Count-1];
		}

		// 渡されたXmlElementの親を取得します。
		public XmlElement GetAncestor(XmlElement e){
			XmlElement[] elements = this.ToArray();
			for(int i=0; i < elements.Length-1; i++){
				if(e == elements[i]) return elements[i+1];
			}
			return null;
		}

		// CurrentNodeの直前の親を取得します。
		public XmlElement GetImmediatelyBeforeCurrentNode(){
			XmlElement[] elements = this.ToArray();
			if(elements.Length < 2) return null;
			return elements[1];
		}

		// FosterParentを実行します。
		// 渡された要素を、FosterParentElementのtable要素の直前に挿入します。
		public void FosterParent(XmlNode node){
			XmlElement[] elements = this.ToArray();
			for(int i=0; i < elements.Length-1; i++){
				if(TableElement.IsMatch(elements[i])){
					XmlElement table = elements[i];
					XmlElement fosterParentElement = elements[i+1];
					fosterParentElement.InsertBefore(node, table);
				}
			}
		}

// Remove・Replace

		// 指定されたnodeをRemoveし、その上の要素を返します。
		// 指定された要素が含まれていないときは例外が発生します。
		// if node is no longer in the stack of open elements (e.g. because it got removed by the next step), the element that was immediately above node in the stack of open elements before node was removed.
		public XmlElement Remove(XmlElement e){
			Stack<XmlElement> tempStack = new Stack<XmlElement>();
			XmlElement result = null;
			while(this.Count > 0){
				XmlElement x = this.Pop();
				if(x == e){
					if(this.Count == 0){
						throw new Exception(string.Format("Stack最上位の要素をRemoveしようとしました。Element: {0}", e.Name));
					}
					result = this.Peek();
					break;
				}
				tempStack.Push(x);
			}
			while(tempStack.Count > 0){
				this.Push(tempStack.Pop());
			}
			return result;
		}


		// 要素を置換します。
		public void Replace(XmlElement oldElement, XmlElement newElement){
			Stack<XmlElement> tempStack = new Stack<XmlElement>();
			while(this.Count > 0){
				XmlElement x = this.Pop();
				if(x == oldElement) break;
				tempStack.Push(x);
			}
			this.Push(newElement);
			while(tempStack.Count > 0){
				this.Push(tempStack.Pop());
			}
		}

		// 最初に指定された要素のすぐ下に、次に指定された要素を挿入します。
		public void InsertBelow(XmlElement parentElement, XmlElement insertedElement){
			Stack<XmlElement> tempStack = new Stack<XmlElement>();
			while(this.Count > 0){
				XmlElement x = this.Peek();
				if(x == parentElement){
					this.Push(insertedElement);
					break;
				}
				tempStack.Push(this.Pop());
			}
			while(tempStack.Count > 0){
				this.Push(tempStack.Pop());
			}
		}

		public void ClearBackToTable(){
			XmlElement x = this.Peek();
			while(!HtmlRootElement.IsMatch(x) && !TableElement.IsMatch(x)){
				x = this.Pop();
			}
		}

		public void ClearBackToTableBody(){
			XmlElement x = this.Peek();
			while(!IsElementInElementInfos(x, TbodyClearElements)){
				x = this.Pop();
			}
		}

		public void ClearBackToTableRow(){
			XmlElement x = this.Peek();
			while(!IsElementInElementInfos(x, TableRowClearElements)){
				x = this.Pop();
			}
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



