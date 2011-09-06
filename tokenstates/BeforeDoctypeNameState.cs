using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class BeforeDoctypeNameState : TokenState{

			public BeforeDoctypeNameState(RedFaceParser p) : base(p){}

			public override void Read(){
				Parser.Stop();
			}

		}
	}
}
