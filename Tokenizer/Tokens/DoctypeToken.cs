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
	}
}
