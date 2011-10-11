using System;
using System.Xml;

namespace Bakera.RedFace{
	public abstract class ActiveFormatElementItem{

		public XmlElement Element{get; private set;}
		public TagToken Token{get; private set;}
		public virtual bool IsMarker{get{return false;}}

		public ActiveFormatElementItem(XmlElement e, TagToken t){
			
		}

	}
}

