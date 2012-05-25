using System;
using System.Xml;

namespace Bakera.RedFace{

	public class InTableInsertionMode : TableRelatedInsertionMode{

		public override void AppendDoctypeToken(TreeConstruction tree, DoctypeToken token){
			OnMessageRaised(new UnexpectedDoctypeError());
		}

		public override void AppendCommentToken(TreeConstruction tree, CommentToken token){
			tree.AppendCommentForToken(token);
		}

		public override void AppendCharacterToken(TreeConstruction tree, CharacterToken token){
			tree.ClearPendingTableCharacterTokens();
			tree.OriginalInsertionMode = tree.CurrentInsertionMode;
			tree.ChangeInsertionMode<InTableTextInsertionMode>();
			tree.ReprocessFlag = true;
		}

		public override void AppendStartTagToken(TreeConstruction tree, StartTagToken token){
			switch(token.Name){
			case "caption":
				tree.StackOfOpenElements.ClearBackToTable();
				tree.ListOfActiveFormatElements.InsertMarker();
				tree.InsertElementForToken(token);
				tree.ChangeInsertionMode<InCaptionInsertionMode>();
				return;
			case "colgroup":
				tree.StackOfOpenElements.ClearBackToTable();
				tree.InsertElementForToken(token);
				tree.ChangeInsertionMode<InColumnGroupInsertionMode>();
				return;
			case "col":
				AppendStartTagToken(tree, new FakeStartTagToken(){Name = "colgroup"});
				tree.ReprocessFlag = true;
				return;
			case "tbody":
			case "tfoot":
			case "thead":
				tree.StackOfOpenElements.ClearBackToTable();
				tree.InsertElementForToken(token);
				tree.ChangeInsertionMode<InTableBodyInsertionMode>();
				return;
			case "td":
			case "th":
			case "tr":
				AppendStartTagToken(tree, new FakeStartTagToken(){Name = "tbody"});
				tree.ReprocessFlag = true;
				return;
			case "table":
				OnMessageRaised(new DoubleTableError());
				AppendEndTagToken(tree, new FakeEndTagToken(){Name = "table"});
				// このパーサーは fragment case を実装しない
				// then, if that token wasn't ignored, reprocess the current token.
				// Note: The fake end tag token here can only be ignored in the fragment case.
				tree.ReprocessFlag = true;
				return;
			case "input":
				if(!token.IsHiddenType()){
					AppendAnythingElse(tree, token);
					return;
				}
				OnMessageRaised(new HiddenInputInTableError());
				tree.InsertElementForToken(token);
				tree.PopFromStack();
				return;
			case "form":
				OnMessageRaised(new FormInTableError());
				if(tree.FormElementPointer != null) return;
				XmlElement form = tree.InsertElementForToken(token);
				tree.FormElementPointer = form;
				tree.PopFromStack();
				return;
			}
			AppendAnythingElse(tree, token);
		}

		public override void AppendEndTagToken(TreeConstruction tree, EndTagToken token){
			switch(token.Name){
			case "table":
				if(!tree.StackOfOpenElements.HaveElementInTableScope(token.Name)){
					OnMessageRaised(new LonlyEndTagError(token.Name));
					return;
				}
				tree.StackOfOpenElements.PopUntilSameTagName(token.Name);
				tree.ResetInsertionModeAppropriately();
				return;
			case "body":
			case "caption":
			case "col":
			case "colgroup":
			case "html":
			case "tbody":
			case "td":
			case "tfoot":
			case "th":
			case "thead":
			case "tr":
				OnMessageRaised(new UnexpectedEndTagError(token.Name));
				return;
			}
			AppendAnythingElse(tree, token);
		}

		public override void AppendEndOfFileToken(TreeConstruction tree, EndOfFileToken token){
			// If the current node is not the root html element, then this is a parse error.
			// Note: It can only be the current node in the fragment case.
			OnMessageRaised(new SuddenlyEndAtElementError(token.Name));
			tree.Parser.Stop();
			return;
		}


		public override void AppendAnythingElse(TreeConstruction tree, Token token){
			OnMessageRaised(new FosterParentedTokenError(token.Name));
			tree.FosterParentMode = true;
			tree.AppendToken<InBodyInsertionMode>(token);
			tree.FosterParentMode = false;
			return;
		}



	}
}
