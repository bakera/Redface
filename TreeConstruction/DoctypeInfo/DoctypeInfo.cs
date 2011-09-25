using System;
using System.Xml;

namespace Bakera.RedFace{

	public abstract class DoctypeInfo{

		public const string HTML401FramesetPublicIdentifierPrefix = "-//W3C//DTD HTML 4.01 Frameset//";
		public const string HTML401TransitionalPublicIdentifierPrefix = "-//W3C//DTD HTML 4.01 Transitional//";
		public const string XHTML10FramesetPublicIdentifierPrefix = "-//W3C//DTD XHTML 1.0 Frameset//";
		public const string XHTML10TransitionalPublicIdentifierPrefix = "-//W3C//DTD XHTML 1.0 Transitional//";

		public abstract DocumentMode DocumentMode{
			get;
		}

		public static DoctypeInfo CreateDoctypeInfo(DoctypeToken t){
			if(HTML5Doctype.IsMatch(t)) return new HTML5Doctype();
			if(HTML40StrictDoctype.IsMatch(t)) return new HTML40StrictDoctype();
			if(HTML401StrictDoctype.IsMatch(t)) return new HTML401StrictDoctype();
			if(XHTML10StrictDoctype.IsMatch(t)) return new XHTML10StrictDoctype();
			if(XHTML11Doctype.IsMatch(t)) return new XHTML11Doctype();
			if(QuirksDoctype.IsMatch(t)) return new QuirksDoctype();
			if(LimitedQuirksDoctype.IsMatch(t)) return new LimitedQuirksDoctype();
			return new UnKnownDoctype();
		}


		public static bool IsHtml(DoctypeToken t){
			return t.Name.Equals("html", StringComparison.InvariantCulture);
		}

	}

}



