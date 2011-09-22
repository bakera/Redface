using System;

namespace Bakera.RedFace{

	public class DoctypeToken : Token{

/*
 DOCTYPE tokens have a name, a public identifier, a system identifier, and a force-quirks flag.
*/

		public string Name{get; set;}
		public string PublicIdentifier{get; set;}
		public string SystemIdentifier{get; set;}
		public bool ForceQuirks{get; set;}

/*
 When a DOCTYPE token is created, its name, public identifier, and system identifier must be marked as missing (which is a distinct state from the empty string), and the force-quirks flag must be set to off (its other state is on).
*/
		public DoctypeToken(){
			this.Name = null;
			this.PublicIdentifier = null;
			this.SystemIdentifier = null;
			this.ForceQuirks = false;
		}

		public override string ToString(){
			string result = string.Format("{0} / Name: \"{1}\"", this.GetType().Name, this.Name);
			if(this.PublicIdentifier != null){
				result += string.Format("\n PublicIdentifier: \"{0}\"", this.PublicIdentifier);
			}
			if(this.SystemIdentifier != null){
				result += string.Format("\n SystemIdentifier: \"{0}\"", this.SystemIdentifier);
			}
			if(this.ForceQuirks){
				result += "\n Force-quirks Flag: ON";
			}
			return result;
		}

		public DocumentMode GetDocumentMode(){
			/* HTML5, HTML4.0strict, HTML4.01strict, XHTML1.0 strict, XHTML1.1 */

			// HTML5

			return DocumentMode.UnKnown;
		}


// Doctype判定
		public bool IsHtml{
			get{
				return this.Name.Equals("html", StringComparison.InvariantCulture);
			}
		}

		public bool IsHtml5{
			get{
				if(!IsHtml) return false;
				if(this.PublicIdentifier != null) return false;
				if(this.SystemIdentifier != null && !this.SystemIdentifier.Equals("about:legacy-compat", StringComparison.InvariantCulture)) return false;
				return true;
			}
		}

		public bool IsHtml40Strict{
			get{
				if(!IsHtml) return false;
				if(this.PublicIdentifier == null) return false;
				if(!this.PublicIdentifier.Equals("-//W3C//DTD HTML 4.0//EN", StringComparison.InvariantCulture)) return false;
				if(this.SystemIdentifier != null && !this.SystemIdentifier.Equals("http://www.w3.org/TR/REC-html40/strict.dtd", StringComparison.InvariantCulture)) return false;
				return true;
			}
		}

		public bool IsHtml401Strict{
			get{
				if(!IsHtml) return false;
				if(this.PublicIdentifier == null) return false;
				if(!this.PublicIdentifier.Equals("-//W3C//DTD HTML 4.01//EN", StringComparison.InvariantCulture)) return false;
				if(this.SystemIdentifier != null && !this.SystemIdentifier.Equals("http://www.w3.org/TR/html4/strict.dtd", StringComparison.InvariantCulture)) return false;
				return true;
			}
		}

		public bool IsXhtml10Strict{
			get{
				if(!IsHtml) return false;
				if(this.PublicIdentifier == null) return false;
				if(!this.PublicIdentifier.Equals("-//W3C//DTD HTML 4.01//EN", StringComparison.InvariantCulture)) return false;
				if(this.SystemIdentifier != null && !this.SystemIdentifier.Equals("http://www.w3.org/TR/html4/strict.dtd", StringComparison.InvariantCulture)) return false;
				return true;
			}
		}




	}
}
