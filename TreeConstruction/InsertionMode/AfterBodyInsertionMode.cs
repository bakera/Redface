using System;
using System.Reflection;
using System.Xml;

namespace Bakera.RedFace{

	public class AfterBodyInsertionMode : InsertionMode{

		public override void AppendToken(TreeConstruction tree, Token token){

			if(token.IsWhiteSpace){
				tree.AppendToken<InBodyInsertionMode>(token);
				return;
			}

			if(token is CommentToken){
				XmlComment comment = tree.CreateCommentForToken((CommentToken)token);
				tree.StackOfOpenElements[0].AppendChild(comment);
				return;
			}

			if(token is DoctypeToken){
				OnParseErrorRaised(string.Format("先頭以外の箇所に文書型宣言があります。"));
				return;
			}

			if(token.IsStartTag("html")){
				tree.AppendToken<InBodyInsertionMode>(token);
				return;
			}

			if(token.IsEndTag("html")){
				//ToDo: If the parser was originally created as part of the HTML fragment parsing algorithm, this is a parse error; ignore the token. (fragment case)
				tree.ChangeInsertionMode<AfterAfterBodyInsertionMode>();
				return;
			}

			if(token is EndOfFileToken){
				tree.Parser.Stop();
				return;
			}

			OnParseErrorRaised(string.Format("body終了タグの後ろに不明なトークンがあります。: {0}", token.Name));
			tree.AppendToken<InBodyInsertionMode>(token);
			return;
		}

	}
}
