using System;
using System.Xml;

namespace Bakera.RedFace{

	public class HTML40StrictDoctype : DoctypeInfo{

		public const string PublicIdentifier = "-//W3C//DTD HTML 4.0//EN";
		public const string SystemIdentifier = "http://www.w3.org/TR/REC-html40/strict.dtd";

		public override DocumentMode DocumentMode{
			get{return DocumentMode.NoQuirks;}
		}

		public static bool IsMatch(DoctypeToken t){
			if(!IsHtml(t)) return false;
			if(t.PublicIdentifier == null) return false;
			if(!t.PublicIdentifier.Equals(PublicIdentifier, StringComparison.InvariantCulture)) return false;
			if(t.SystemIdentifier != null && !t.SystemIdentifier.Equals(SystemIdentifier, StringComparison.InvariantCulture)) return false;
			return true;
		}


	}

}



