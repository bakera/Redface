using System;
using System.Xml;

namespace Bakera.RedFace{

	public class InSelectInsertionMode : InsertionMode{


		public override void AppendDoctypeToken(TreeConstruction tree, DoctypeToken token){
			OnMessageRaised(new UnexpectedDoctypeError());
			return;
		}

		public override void AppendCommentToken(TreeConstruction tree, CommentToken token){
			tree.AppendCommentForToken(token);
		}

		public override void AppendCharacterToken(TreeConstruction tree, CharacterToken token){
			if(token.IsNULL){
				OnMessageRaised(new NullInDataError());
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
					OnMessageRaised(new NestedSelectElementError());
					AppendEndTagToken(tree, new FakeEndTagToken(){Name = "select"});
					return;
				}
				return;

			case "input":
			case "keygen":
			case "textarea":
				OnMessageRaised(new UnexpectedStartTagInSelectError(token.Name));
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
					OnMessageRaised(new LonlyEndTagError(token.Name));
					return;
				}
				return;

			case "option":
				if(tree.StackOfOpenElements.IsCurrentNameMatch("option")){
					tree.PopFromStack();
				} else {
					OnMessageRaised(new LonlyEndTagError(token.Name));
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
			OnMessageRaised(new UnexpectedTokenInSelectError(token.Name));
			return;
		}


	}
}
