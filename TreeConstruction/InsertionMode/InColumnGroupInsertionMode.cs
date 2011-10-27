using System;
using System.Xml;

namespace Bakera.RedFace{

	public class InColumnGroupInsertionMode : TableRelatedInsertionMode{

		public override void AppendDoctypeToken(TreeConstruction tree, DoctypeToken token){
			OnParseErrorRaised(string.Format("先頭以外の箇所に文書型宣言があります。"));
		}

		public override void AppendCommentToken(TreeConstruction tree, CommentToken token){
			tree.AppendCommentForToken(token);
		}

		public override void AppendCharacterToken(TreeConstruction tree, CharacterToken token){
		}

		public override void AppendStartTagToken(TreeConstruction tree, StartTagToken token){
		}

		public override void AppendAnythingElse(TreeConstruction tree, Token token){
			Console.WriteLine("========\nnot implemented: {0} - {1}", this.Name, token);
			tree.Parser.Stop();
			return;
		}
	}
}
