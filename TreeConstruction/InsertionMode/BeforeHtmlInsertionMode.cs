using System;
using System.Reflection;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class BeforeHtmlInsertionMode : InsertionMode{

			public override void AppendToken(TreeConstruction tree, Token token){
				Console.WriteLine("========");
				Console.WriteLine(token);
				throw new Exception("not implemented:" + this.GetType().Name);
			}

		}
	}
}
