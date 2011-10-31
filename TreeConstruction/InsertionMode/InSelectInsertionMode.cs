using System;
using System.Xml;

namespace Bakera.RedFace{

	public class InSelectInsertionMode : InsertionMode{


		public override void AppendDoctypeToken(TreeConstruction tree, DoctypeToken token){
			OnParseErrorRaised(string.Format("先頭以外の箇所に文書型宣言があります。"));
			return;
		}

		public override void AppendCommentToken(TreeConstruction tree, CommentToken token){
			tree.AppendCommentForToken(token);
		}

		public override void AppendCharacterToken(TreeConstruction tree, CharacterToken token){
			if(token.IsNULL){
				OnParseErrorRaised(string.Format("NUL文字が出現しました。"));
				return;
			}
			tree.InsertCharacter(token);
		}

		public override void AppendEndOfFileToken(TreeConstruction tree, EndOfFileToken token){
			tree.Parser.Stop();
			return;
		}

		public override void AppendStartTagToken(TreeConstruction tree, StartTagToken token){
			switch(token.Name){
			case "html":
				tree.AppendToken<InBodyInsertionMode>(token);
				return;

			case "option":
				if(tree.StackOfOpenElements.IsCurrentNameMatch("option")){
					AppendEndTagToken(tree, new FakeEndTagToken(){Name = "option"});
				}
				tree.InsertElementForToken(token);
				return;

			case "optgroup":
				if(tree.StackOfOpenElements.IsCurrentNameMatch("option")){
					AppendEndTagToken(tree, new FakeEndTagToken(){Name = "option"});
				}
				if(tree.StackOfOpenElements.IsCurrentNameMatch("optgroup")){
					AppendEndTagToken(tree, new FakeEndTagToken(){Name = "optgroup"});
				}
				tree.InsertElementForToken(token);
				return;

			case "select":
				if(!tree.StackOfOpenElements.HaveElementInSelectScope(token.Name)){
					OnParseErrorRaised(string.Format("select要素内にselect要素の開始タグが出現しました。"));
					AppendEndTagToken(tree, new FakeEndTagToken(){Name = "select"});
					return;
				}
				return;

			case "input":
			case "keygen":
			case "textarea":
				OnParseErrorRaised(string.Format("select要素内に出現できない要素です。: {0}", token.Name));
				AppendEndTagToken(tree, new FakeEndTagToken(){Name = "select"});
				tree.ReprocessFlag = true;
				return;

			case "script":
				tree.AppendToken<InHeadInsertionMode>(token);
				return;

			}
			AppendAnythingElse(tree, token);
		}


		public override void AppendEndTagToken(TreeConstruction tree, EndTagToken token){
			switch(token.Name){
			case "optgroup":
				if(tree.StackOfOpenElements.IsCurrentNameMatch("option")){
					XmlElement immediatelyBeforeNode = tree.StackOfOpenElements.GetImmediatelyBeforeCurrentNode();
					if(immediatelyBeforeNode.Name == "optgroup"){
						AppendEndTagToken(tree, new FakeEndTagToken(){Name = "option"});
					}
				}
				if(tree.StackOfOpenElements.IsCurrentNameMatch("optgroup")){
					tree.PopFromStack();
				} else {
					OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", token.Name));
					return;
				}
				return;

			case "option":
				if(tree.StackOfOpenElements.IsCurrentNameMatch("option")){
					tree.PopFromStack();
				} else {
					OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", token.Name));
					return;
				}
				return;

			case "select":
				tree.StackOfOpenElements.PopUntilSameTagName("select");
				tree.ResetInsertionModeAppropriately();
				return;

			}
			AppendAnythingElse(tree, token);
		}


		public override void AppendAnythingElse(TreeConstruction tree, Token token){
			OnParseErrorRaised(string.Format("select要素内に出現できないトークンです。: {0}", token.Name));
			return;
		}


	}
}
