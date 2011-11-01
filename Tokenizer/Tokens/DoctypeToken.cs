using System;

namespace Bakera.RedFace{

	public partial class DoctypeToken : Token{

/*
 DOCTYPE tokens have a name, a public identifier, a system identifier, and a force-quirks flag.
*/

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

		public override void AppendTo(TreeConstruction tree, InsertionMode mode){
			mode.AppendDoctypeToken(tree, this);
		}



	}
}
