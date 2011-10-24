using System;
using System.Xml;

namespace Bakera.RedFace{

	public class InHeadNoscriptInsertionMode : InsertionMode{

		public override void AppendToken(TreeConstruction tree, Token token){
			if(token is DoctypeToken){
				OnParseErrorRaised(string.Format("先頭以外の箇所に文書型宣言があります。"));
				return;
			}

			if(token.IsStartTag("html")){
				tree.AppendToken<InBodyInsertionMode>(token);
				return;
			}

			if(token.IsEndTag("noscript")){
				EndTagNoScriptHadBeSeen(tree, token);
				return;
			}

			if(token.IsWhiteSpace || token is CommentToken || token.IsStartTag("basefont", "bgsound", "link", "meta", "noframes", "style")){
				tree.AppendToken<InHeadInsertionMode>(token);
				return;
			}

			if(token.IsEndTag("br")){
				AnythingElse(tree, token);
				return;
			}

			if(token.IsStartTag("head", "noscript")){
				OnParseErrorRaised(string.Format("開始タグが重複しています。: {0}", token.Name));
				return;
			}

			AnythingElse(tree, token);
			return;
		}


		private void EndTagNoScriptHadBeSeen(TreeConstruction tree, Token token){
			tree.PopFromStack();
			tree.ChangeInsertionMode<InHeadInsertionMode>();
			return;
		}

		private void AnythingElse(TreeConstruction tree, Token token){
				OnParseErrorRaised(string.Format("この場所には出現できないトークンです。: {0}", token.Name));
			EndTagNoScriptHadBeSeen(tree, token);
			tree.ReprocessFlag = true;
			return;
		}

	}
}
