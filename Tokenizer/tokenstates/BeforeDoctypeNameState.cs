using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class BeforeDoctypeNameState : TokenState{

			public BeforeDoctypeNameState(Tokenizer t) : base(t){}

			public override Token Read(){
				Parser.Stop();
				return null;
			}

		}
	}
}
