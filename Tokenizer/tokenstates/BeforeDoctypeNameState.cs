using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class BeforeDoctypeNameState : TokenizationState{

			public override Token Read(Tokenizer t){
				char? c = t.ConsumeChar();
				t.Parser.Stop();
				return null;
			}

		}
	}
}
