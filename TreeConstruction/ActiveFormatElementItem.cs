using System;
using System.Xml;

namespace Bakera.RedFace{
	public abstract class ActiveFormatElementItem{

		public XmlElement Element{get; private set;}
		public TagToken Token{get; private set;}
		public virtual bool IsMarker{get{return false;}}

		public ActiveFormatElementItem(XmlElement e, TagToken t){
			
		}


		// 渡されたXmlElementが同じ名前、名前空間、属性を持っていればtrueを返します。
		// same tag name, namespace, and attributes as element
		public bool IsSamePairElement(XmlElement e2){
			XmlElement e1 = this.Element;
			if(e1 == null) return e2 == null;

			if(e1.Name != e2.Name) return false;
			if(e1.Attributes.Count != e2.Attributes.Count) return false;
			foreach(XmlAttribute attr1 in e1.Attributes){
				XmlAttribute attr2 = e2.Attributes[attr1.Name, attr1.NamespaceURI];
				if(attr2 == null) return false;
				if(attr1.Value != attr2.Value) return false;
			}
			return true;
		}


	}
}

