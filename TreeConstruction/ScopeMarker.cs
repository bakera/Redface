using System;
using System.Xml;

namespace Bakera.RedFace{
	public class ScopeMarker : ActiveFormatElementItem{
		public override bool IsMarker{get{return true;}}

		public ScopeMarker(XmlElement e, TagToken t) : base(e, t){}

	}
}

