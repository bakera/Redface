using System;
using System.Xml;

namespace Bakera.RedFace{


	public class BeforeHeadInsertionMode : InsertionMode{

		protected override void AppendDoctypeToken(TreeConstruction tree, DoctypeToken token){
			OnParseErrorRaised(string.Format("先頭以外の箇所に文書型宣言があります。"));
		}

		protected override void AppendCommentToken(TreeConstruction tree, CommentToken token){
			tree.AppendCommentForToken(token);
		}

		protected override void AppendCharacterToken(TreeConstruction tree, CharacterToken token){
			if(token.IsWhiteSpace) return;
			AppendAnythingElse(tree, token);
		}

		protected override void AppendStartTagToken(TreeConstruction tree, StartTagToken token){

			if(token.IsStartTag("html")){
				tree.AppendToken<InBodyInsertionMode>(token);
				return;
			}

			if(token.IsStartTag("head")){
				XmlElement headElement = tree.InsertElementForToken((StartTagToken)token);
				tree.HeadElementPointer = headElement;
				tree.ChangeInsertionMode<InHeadInsertionMode>();
				return;
			}
			AppendAnythingElse(tree, token);
		}

		protected override void AppendEndTagToken(TreeConstruction tree, EndTagToken token){
			if(token.IsEndTag("head", "body", "html", "br")){
				AppendAnythingElse(tree, token);
				return;
			}
			OnParseErrorRaised(string.Format("不明な終了タグがあります。"));
			return;
		}

		protected override void AppendAnythingElse(TreeConstruction tree, Token token){
			AppendStartTagToken(tree, new PseudoStartTagToken(){Name = "head"});
			tree.ReprocessFlag = true;
			return;
		}

	}
}
