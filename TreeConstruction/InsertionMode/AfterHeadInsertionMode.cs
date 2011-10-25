using System;
using System.Xml;

namespace Bakera.RedFace{


	public class AfterHeadInsertionMode : InsertionMode{


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
			tree.AppendCommentForToken(token);
		}

		protected override void AppendStartTagToken(TreeConstruction tree, StartTagToken token){
			if(token.IsStartTag("html")){
				tree.AppendToken<InBodyInsertionMode>(token);
				return;
			}

			if(token.IsStartTag("body")){
				tree.InsertElementForToken(token);
				tree.Parser.FramesetOK = false;
				tree.ChangeInsertionMode<InBodyInsertionMode>();
				return;
			}

			if(token.IsStartTag("frameset")){
				tree.InsertElementForToken(token);
				tree.ChangeInsertionMode<InFramesetInsertionMode>();
				return;
			}

			if(token.IsStartTag("base", "basefont", "bgsound", "link", "meta", "noframes", "script", "style", "title")){
				OnParseErrorRaised(string.Format("head要素内にしか出現できない要素です。: {0}", token.Name));
				tree.PutToStack(tree.HeadElementPointer);
				tree.AppendToken<InHeadInsertionMode>(token);
				tree.PopFromStack();
				return;
			}
			if(token.IsStartTag("head")){
				OnParseErrorRaised(string.Format("head要素が終了してからhaad要素の開始タグが出現しました。"));
				return;
			}

			AppendAnythingElse(tree, token);
		}


		protected override void AppendEndTagToken(TreeConstruction tree, EndTagToken token){
			if(token.IsEndTag("body", "html", "br")){
				AppendAnythingElse(tree, token);
				return;
			}
			OnParseErrorRaised(string.Format("不明な終了タグがあります。"));
			return;
		}

		protected override void AppendAnythingElse(TreeConstruction tree, Token token){
			AppendStartTagToken(tree, new PseudoStartTagToken(){Name = "body"});
			tree.Parser.FramesetOK = true;
			tree.ReprocessFlag = true;
			return;
		}
	}
}
