using System;
using System.Xml;

namespace Bakera.RedFace{

	public class HTML5Doctype : NoQuirksDoctype{

		public const string LegacyCompatSystemIdentifier = "about:legacy-compat";

		public static bool IsMatch(DoctypeToken t){
			if(!IsHtml(t)) return false;
			if(t.PublicIdentifier != null) return false;
			if(t.SystemIdentifier != null && !t.SystemIdentifier.Equals(LegacyCompatSystemIdentifier, StringComparison.InvariantCulture)) return false;
			return true;
		}


	}

}



