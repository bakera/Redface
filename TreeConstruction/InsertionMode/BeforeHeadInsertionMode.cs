using System;
using System.Xml;

namespace Bakera.RedFace{


	public class BeforeHeadInsertionMode : InsertionMode{

		public override void AppendDoctypeToken(TreeConstruction tree, DoctypeToken token){
			OnParseErrorRaised(string.Format("先頭以外の箇所に文書型宣言があります。"));
		}

		public override void AppendCommentToken(TreeConstruction tree, CommentToken token){
			tree.AppendCommentForToken(token);
		}

		public override void AppendCharacterToken(TreeConstruction tree, CharacterToken token){
			if(token.IsWhiteSpace) return;
			AppendAnythingElse(tree, token);
		}

		public override void AppendStartTagToken(TreeConstruction tree, StartTagToken token){

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

		public override void AppendEndTagToken(TreeConstruction tree, EndTagToken token){
			if(token.IsEndTag("head", "body", "html", "br")){
				AppendAnythingElse(tree, token);
				return;
			}
			OnParseErrorRaised(string.Format("不明な終了タグがあります。"));
			return;
		}

		public override void AppendAnythingElse(TreeConstruction tree, Token token){
			AppendStartTagToken(tree, new FakeStartTagToken(){Name = "head"});
			tree.ReprocessFlag = true;
			return;
		}

	}
}
