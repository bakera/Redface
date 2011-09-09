using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class BeforeDoctypeNameState : TokenizationState{

			public BeforeDoctypeNameState(Tokenizer t) : base(t){}

			public override Token Read(){
				ConsumeChar();
				Parser.Stop();
				return null;
			}

		}
	}
}
