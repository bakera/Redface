using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class AttributeValueUnQuotedState : TokenizationState{

			public override void Read(Tokenizer t){
				throw new Exception("not implemented");


			}
		}
	}
}
