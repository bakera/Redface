using System;
using System.Xml;

namespace Bakera.RedFace{

	public class QuirksDoctype : DoctypeInfo{

		public static readonly string[] QuirksPublicIdentifiersPrefix = new string[]{
			"+//Silmaril//dtd html Pro v0r11 19970101//",
			"-//AdvaSoft Ltd//DTD HTML 3.0 asWedit + extensions//",
			"-//AS//DTD HTML 3.0 asWedit + extensions//",
			"-//IETF//DTD HTML 2.0 Level 1//",
			"-//IETF//DTD HTML 2.0 Level 2//",
			"-//IETF//DTD HTML 2.0 Strict Level 1//",
			"-//IETF//DTD HTML 2.0 Strict Level 2//",
			"-//IETF//DTD HTML 2.0 Strict//",
			"-//IETF//DTD HTML 2.0//",
			"-//IETF//DTD HTML 2.1E//",
			"-//IETF//DTD HTML 3.0//",
			"-//IETF//DTD HTML 3.2 Final//",
			"-//IETF//DTD HTML 3.2//",
			"-//IETF//DTD HTML 3//",
			"-//IETF//DTD HTML Level 0//",
			"-//IETF//DTD HTML Level 1//",
			"-//IETF//DTD HTML Level 2//",
			"-//IETF//DTD HTML Level 3//",
			"-//IETF//DTD HTML Strict Level 0//",
			"-//IETF//DTD HTML Strict Level 1//",
			"-//IETF//DTD HTML Strict Level 2//",
			"-//IETF//DTD HTML Strict Level 3//",
			"-//IETF//DTD HTML Strict//",
			"-//IETF//DTD HTML//",
			"-//Metrius//DTD Metrius Presentational//",
			"-//Microsoft//DTD Internet Explorer 2.0 HTML Strict//",
			"-//Microsoft//DTD Internet Explorer 2.0 HTML//",
			"-//Microsoft//DTD Internet Explorer 2.0 Tables//",
			"-//Microsoft//DTD Internet Explorer 3.0 HTML Strict//",
			"-//Microsoft//DTD Internet Explorer 3.0 HTML//",
			"-//Microsoft//DTD Internet Explorer 3.0 Tables//",
			"-//Netscape Comm. Corp.//DTD HTML//",
			"-//Netscape Comm. Corp.//DTD Strict HTML//",
			"-//O'Reilly and Associates//DTD HTML 2.0//",
			"-//O'Reilly and Associates//DTD HTML Extended 1.0//",
			"-//O'Reilly and Associates//DTD HTML Extended Relaxed 1.0//",
			"-//SoftQuad Software//DTD HoTMetaL PRO 6.0::19990601::extensions to HTML 4.0//",
			"-//SoftQuad//DTD HoTMetaL PRO 4.0::19971010::extensions to HTML 4.0//",
			"-//Spyglass//DTD HTML 2.0 Extended//",
			"-//SQ//DTD HTML 2.0 HoTMetaL + extensions//",
			"-//Sun Microsystems Corp.//DTD HotJava HTML//",
			"-//Sun Microsystems Corp.//DTD HotJava Strict HTML//",
			"-//W3C//DTD HTML 3 1995-03-24//",
			"-//W3C//DTD HTML 3.2 Draft//",
			"-//W3C//DTD HTML 3.2 Final//",
			"-//W3C//DTD HTML 3.2//",
			"-//W3C//DTD HTML 3.2S Draft//",
			"-//W3C//DTD HTML 4.0 Frameset//",
			"-//W3C//DTD HTML 4.0 Transitional//",
			"-//W3C//DTD HTML Experimental 19960712//",
			"-//W3C//DTD HTML Experimental 970421//",
			"-//W3C//DTD W3 HTML//",
			"-//W3O//DTD W3 HTML 3.0//",
			"-//WebTechs//DTD Mozilla HTML 2.0//",
			"-//WebTechs//DTD Mozilla HTML//",
		};

		public static readonly string[] QuirksPublicIdentifiers = new string[]{
			"-//W3O//DTD W3 HTML Strict 3.0//EN//",
			"-/W3C/DTD HTML 4.0 Transitional/EN",
			"HTML",
		};

		public static readonly string[] QuirksSystemIdentifiers = new string[]{
			"http://www.ibm.com/data/dtd/v11/ibmxhtml1-transitional.dtd",
		};

		public override DocumentMode DocumentMode{
			get{return DocumentMode.Quirks;}
		}


		public static bool IsMatch(DoctypeToken t){
			if(t.ForceQuirks) return true;
			if(!IsHtml(t)) return true;
			foreach(string s in QuirksPublicIdentifiersPrefix){
				if(t.PublicIdentifier.StartsWith(s, StringComparison.InvariantCultureIgnoreCase)) return true;
			}
			foreach(string s in QuirksPublicIdentifiers){
				if(t.PublicIdentifier.Equals(s, StringComparison.InvariantCultureIgnoreCase)) return true;
			}
			foreach(string s in QuirksSystemIdentifiers){
				if(t.SystemIdentifier.StartsWith(s, StringComparison.InvariantCultureIgnoreCase)) return true;
			}
			if(t.SystemIdentifier == null && t.PublicIdentifier.StartsWith(HTML401TransitionalPublicIdentifierPrefix, StringComparison.InvariantCultureIgnoreCase)) return true;
			if(t.SystemIdentifier == null && t.PublicIdentifier.StartsWith(HTML401FramesetPublicIdentifierPrefix, StringComparison.InvariantCultureIgnoreCase)) return true;
			return false;
		}


	}

}

