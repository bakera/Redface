using System;
using System.Xml;

namespace Bakera.RedFace{

	public class InSelectInsertionMode : InsertionMode{

		public override void AppendToken(TreeConstruction tree, Token token){
			Console.WriteLine("========\nnot implemented: {0} - {1}", this.Name, token);
			tree.Parser.Stop();
			return;
		}

	}
}
