using System;

namespace Bakera.RedFace{
	public class EndOfFileToken : Token{

		public override void AppendTo(TreeConstruction tree, InsertionMode mode){
			mode.AppendEndOfFileToken(tree, this);
		}

	}
}
