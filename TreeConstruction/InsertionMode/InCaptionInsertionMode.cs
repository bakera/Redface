using System;
using System.Reflection;
using System.Xml;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class InCaptionInsertionMode : TableRelatedInsertionMode{

			public override void AppendToken(TreeConstruction tree, Token token){

				Console.WriteLine("========\nnot implemented: {0} - {1}", this.Name, token);
				tree.Parser.Stop();
				return;

			}

		}
	}
}
