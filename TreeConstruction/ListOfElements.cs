using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Bakera.RedFace{

	public class ListOfElements{
/*
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
*/
		private List<ActiveFormatElementItem> myBeforeMarkerList = new List<ActiveFormatElementItem>();
		private List<ActiveFormatElementItem> myAfterMarkerList = new List<ActiveFormatElementItem>();


		public void Push(XmlElement e, TagToken t){

			if(myAfterMarkerList.Count > 3){
				for(int i=0; i < myAfterMarkerList.Count; i++){
					ActiveFormatElementItem afei = myAfterMarkerList[i];
					if(afei.IsSamePairElement(e)){
						myAfterMarkerList.RemoveAt(i);
						break;
					}
				}
			}
			myAfterMarkerList.Add(new ActiveFormatElement(e, t));
		}

		public int Length{
			get{return myBeforeMarkerList.Count + myAfterMarkerList.Count;}
		}

		public ActiveFormatElementItem[] AfterMarkerItems{
			get{return myAfterMarkerList.ToArray();}
		}

		// マーカーの後ろに渡された名前に相当する要素があれば、そのインデクス番号を返します。
		public int GetAfterMarkerIndexByName(string name){
			return myAfterMarkerList.FindLastIndex((e) => e.Element.Name == name);
		}

		public ActiveFormatElementItem GetAfterMarkerByAfterIndex(int index){
			return myAfterMarkerList[index];
		}

		public void RemoveAfterMarkerByAfterIndex(int index){
			myAfterMarkerList.RemoveAt(index);
		}


		public ActiveFormatElementItem this[int i]{
			get{
				if(i <= myBeforeMarkerList.Count - 1){
					return myBeforeMarkerList[i];
				}
				return myAfterMarkerList[i-myBeforeMarkerList.Count];
			}
		}


		public int GetIndexByElement(XmlElement element){
			int afterResult = myAfterMarkerList.FindLastIndex((e) => e.Element == element);
			if(afterResult >= 0) return myBeforeMarkerList.Count + afterResult;
			int beforeResult = myBeforeMarkerList.FindLastIndex((e) => e.Element == element);
			return beforeResult;
		}
	}

}



