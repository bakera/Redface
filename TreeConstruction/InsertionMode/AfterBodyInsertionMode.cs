using System;
using System.Reflection;
using System.Xml;

namespace Bakera.RedFace{

	public class AfterBodyInsertionMode : InsertionMode{

		public override void AppendCommentToken(TreeConstruction tree, CommentToken token){
			XmlComment comment = tree.CreateCommentForToken(token);
			tree.StackOfOpenElements[0].AppendChild(comment);
			return;
		}

		public override void AppendDoctypeToken(TreeConstruction tree, DoctypeToken token){
			OnParseErrorRaised(string.Format("先頭以外の箇所に文書型宣言があります。"));
			return;
		}

		public override void AppendCharacterToken(TreeConstruction tree, CharacterToken token){
			if(token.IsWhiteSpace){
				tree.AppendToken<InBodyInsertionMode>(token);
				return;
			}
			AppendAnythingElse(tree, token);
		}

		public override void AppendStartTagToken(TreeConstruction tree, StartTagToken token){
			switch(token.Name){
			case "html":
				tree.AppendToken<InBodyInsertionMode>(token);
				return;
			}
			AppendAnythingElse(tree, token);
		}

		public override void AppendEndTagToken(TreeConstruction tree, EndTagToken token){
			switch(token.Name){
			case "html":
				tree.ChangeInsertionMode<AfterAfterBodyInsertionMode>();
				return;
			}
		}

		public override void AppendEndOfFileToken(TreeConstruction tree, EndOfFileToken token){
			tree.Parser.Stop();
			return;
		}

		public override void AppendAnythingElse(TreeConstruction tree, Token token){
			OnParseErrorRaised(string.Format("html終了タグの後ろに不明なトークンがあります。: {0}", token.Name));
			tree.ChangeInsertionMode<InBodyInsertionMode>();
			tree.ReprocessFlag = true;
			return;
		}

	}
}
