using System;
using System.Xml;

namespace Bakera.RedFace{

	public class LimitedQuirksDoctype : DoctypeInfo{

		public override DocumentMode DocumentMode{
			get{return DocumentMode.LimitedQuirks;}
		}

		public static bool IsMatch(DoctypeToken t){
			if(!IsHtml(t)) return false;
			if(t.PublicIdentifier.StartsWith(XHTML10TransitionalPublicIdentifierPrefix, StringComparison.InvariantCultureIgnoreCase)) return true;
			if(t.PublicIdentifier.StartsWith(XHTML10FramesetPublicIdentifierPrefix, StringComparison.InvariantCultureIgnoreCase)) return true;
			if(t.SystemIdentifier != null && t.PublicIdentifier.StartsWith(HTML401TransitionalPublicIdentifierPrefix, StringComparison.InvariantCultureIgnoreCase)) return true;
			if(t.SystemIdentifier != null && t.PublicIdentifier.StartsWith(HTML401FramesetPublicIdentifierPrefix, StringComparison.InvariantCultureIgnoreCase)) return true;
			return false;
		}


	}

}

