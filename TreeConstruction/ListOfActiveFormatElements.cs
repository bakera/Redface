using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Bakera.RedFace{

	public class ListOfElements : List<ActiveFormatElementItem>{
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


		public void Push(XmlElement e, TagToken t){

			// Note: This is the Noah's Ark clause. But with three per family instead of two.
//			Console.WriteLine("Pushed: {0}", e.Name);

//			Document.IsSamePairElement();
			Add(new ActiveFormatElement(e, t));


		}

		public void Reconstruct(XmlElement e, Token t){
			// ToDo: Reconstruct の仕組みを作る
		}


		// 渡された名前にマッチし、スコープマーカーよりも後ろにある最後の要素を取得する
		// 見つからなければ null を返す
		// Let the formatting element be the last element in the list of active formatting elements that:
		//  is between the end of the list and the last scope marker in the list, if any, or the start of the list otherwise, and
		//  has the same tag name as the token.
		public XmlElement GetLastElement(string name){
			return null;
		}

		// リストの最後のマーカーの位置を返します。
		public int GetLastMarkerIndex(){
			return this.FindLastIndex( i => i.IsMarker );
		
		}

		// リストの最後のマーカーから後ろにある要素を取得します。
		public int GetItemsAfterMarker(){
			return this.FindLastIndex( i => i.IsMarker );
		
		}

	}

}



