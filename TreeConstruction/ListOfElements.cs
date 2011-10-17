using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Bakera.RedFace{

	public class ListOfElements{
		private List<XmlElement> myBeforeMarkerList = new List<XmlElement>();
		private List<XmlElement> myAfterMarkerList = new List<XmlElement>();


		public void Push(XmlElement e, TagToken t){
			if(myAfterMarkerList.Count > 3){
				for(int i=0; i < myAfterMarkerList.Count; i++){
					XmlElement afei = myAfterMarkerList[i];
					if(Document.IsSamePairElement(afei, e)){
						myAfterMarkerList.RemoveAt(i);
						break;
					}
				}
			}
			myAfterMarkerList.Add(e);
		}

		public int Length{
			get{return myBeforeMarkerList.Count + myAfterMarkerList.Count;}
		}

		public XmlElement[] AfterMarkerItems{
			get{return myAfterMarkerList.ToArray();}
		}

		// マーカーの後ろに渡された名前に相当する要素があれば、そのインデクス番号を返します。
		public int GetAfterMarkerIndexByName(string name){
			return myAfterMarkerList.FindLastIndex((e) => e.Name == name);
		}

		public XmlElement GetAfterMarkerByAfterIndex(int index){
			return myAfterMarkerList[index];
		}

		public void RemoveAfterMarkerByAfterIndex(int index){
			myAfterMarkerList.RemoveAt(index);
		}


		public XmlElement this[int i]{
			get{
				if(i <= myBeforeMarkerList.Count - 1){
					return myBeforeMarkerList[i];
				}
				return myAfterMarkerList[i-myBeforeMarkerList.Count];
			}
			set{
				if(i <= myBeforeMarkerList.Count - 1){
					myBeforeMarkerList[i] = value;
					return;
				}
				myAfterMarkerList[i-myBeforeMarkerList.Count] = value;
			}
		}

		public int GetIndexByElement(XmlElement element){
			int afterResult = myAfterMarkerList.LastIndexOf(element);
			if(afterResult >= 0) return myBeforeMarkerList.Count + afterResult;
			int beforeResult = myBeforeMarkerList.LastIndexOf(element);
			return beforeResult;
		}

		public bool Remove(XmlElement e){
			bool result = myAfterMarkerList.Remove(e);
			if(result) return true;
			return myBeforeMarkerList.Remove(e);
		}

		public void Insert(int i, XmlElement e){
			if(i <= myBeforeMarkerList.Count - 1){
				myBeforeMarkerList.Insert(i, e);
				return;
			}
			myAfterMarkerList.Insert(i-myBeforeMarkerList.Count, e);
		}


	}

}



