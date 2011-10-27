using System;
using System.Xml;

namespace Bakera.RedFace{

	public class InHeadNoscriptInsertionMode : InsertionMode{

		protected override void AppendDoctypeToken(TreeConstruction tree, DoctypeToken token){
			OnParseErrorRaised(string.Format("先頭以外の箇所に文書型宣言があります。"));
		}

		protected override void AppendCharacterToken(TreeConstruction tree, CharacterToken token){
			if(token.IsWhiteSpace){
				tree.AppendToken<InHeadInsertionMode>(token);
				return;
			}
			AppendAnythingElse(tree, token);
		}

		protected override void AppendCommentToken(TreeConstruction tree, CommentToken token){
			tree.AppendToken<InHeadInsertionMode>(token);
		}

		protected override void AppendStartTagToken(TreeConstruction tree, StartTagToken token){
			if(token.IsStartTag("html")){
				tree.AppendToken<InBodyInsertionMode>(token);
				return;
			}

			if(token.IsStartTag("basefont", "bgsound", "link", "meta", "noframes", "style")){
				tree.AppendToken<InHeadInsertionMode>(token);
				return;
			}
			if(token.IsStartTag("head", "noscript")){
				OnParseErrorRaised(string.Format("開始タグが重複しています。: {0}", token.Name));
				return;
			}
			AppendAnythingElse(tree, token);
		}

		protected override void AppendEndTagToken(TreeConstruction tree, EndTagToken token){
			if(token.IsEndTag("noscript")){
				tree.PopFromStack();
				tree.ChangeInsertionMode<InHeadInsertionMode>();
				return;
			}
			if(token.IsEndTag("br")){
				AppendAnythingElse(tree, token);
				return;
			}
			OnParseErrorRaised(string.Format("不明な終了タグがあります。"));
			return;
		}

		protected override void AppendAnythingElse(TreeConstruction tree, Token token){
			AppendEndTagToken(tree, new FakeEndTagToken(){Name = "noscript"});
			tree.ReprocessFlag = true;
			return;
		}

	}
}
