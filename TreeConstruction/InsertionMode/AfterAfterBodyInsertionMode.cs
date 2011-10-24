using System;
using System.Xml;

namespace Bakera.RedFace{

	public class AfterAfterBodyInsertionMode : InsertionMode{

		public override void AppendToken(TreeConstruction tree, Token token){

			if(token is CommentToken){
				XmlComment comment = tree.CreateCommentForToken((CommentToken)token);
				tree.StackOfOpenElements[0].AppendChild(comment);
				return;
			}

			if(token is DoctypeToken || token.IsWhiteSpace || token.IsStartTag("html")){
				tree.AppendToken<InBodyInsertionMode>(token);
				return;
			}

			if(token is EndOfFileToken){
				tree.Parser.Stop();
				return;
			}

			OnParseErrorRaised(string.Format("html終了タグの後ろに不明なトークンがあります。: {0}", token.Name));
			tree.AppendToken<InBodyInsertionMode>(token);
			return;
		}

	}
}
