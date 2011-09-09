using System;
using System.Reflection;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class InitialMode : InsertionMode{

			public InitialMode(RedFaceParser p) : base(p){}

			public override void AppendToken(Token t){

			}

		}
	}
}
