using System;
using System.Xml;

namespace Bakera.RedFace{

	public class InCellInsertionMode : TableRelatedInsertionMode{

		public override void AppendStartTagToken(TreeConstruction tree, StartTagToken token){
			switch(token.Name){
			case "caption":
			case "col":
			case "colgroup":
			case "tbody":
			case "td":
			case "tfoot":
			case "th":
			case "thead":
			case "tr":
				CloseTheCell(tree);
				tree.ReprocessFlag = true;
				return;
			}
			AppendAnythingElse(tree, token);
		}

		public override void AppendEndTagToken(TreeConstruction tree, EndTagToken token){
			switch(token.Name){
			case "th":
			case "td":
				if(!tree.StackOfOpenElements.HaveElementInTableScope(token.Name)){
					OnMessageRaised(new LonlyEndTagError(token.Name));
					return;
				}
				GenerateImpliedEndTags(tree, token);
				if(!tree.StackOfOpenElements.IsCurrentNameMatch(token.Name)){
					OnMessageRaised(new LonlyEndTagError(token.Name));
				}
				tree.StackOfOpenElements.PopUntilSameTagName(token.Name);
				tree.ListOfActiveFormatElements.ClearUpToTheLastMarker();
				tree.ChangeInsertionMode<InRowInsertionMode>();
				return;

			case "body":
			case "caption":
			case "col":
			case "colgroup":
			case "html":
				OnMessageRaised(new LonlyEndTagError(token.Name));
				return;

			case "table":
			case "tbody":
			case "tfoot":
			case "thead":
			case "tr":
				if(!tree.StackOfOpenElements.HaveElementInTableScope(token.Name)){
					OnMessageRaised(new LonlyEndTagError(token.Name));
					return;
				}
				CloseTheCell(tree);
				tree.ReprocessFlag = true;
				return;
			}
			AppendAnythingElse(tree, token);
		}

		public override void AppendAnythingElse(TreeConstruction tree, Token token){
			tree.AppendToken<InBodyInsertionMode>(token);
		}

		private void CloseTheCell(TreeConstruction tree){
			FakeEndTagToken cellEndTag = new FakeEndTagToken();
			if(tree.StackOfOpenElements.HaveElementInTableScope("td")){
				cellEndTag.Name = "td";
			} else {
				cellEndTag.Name = "th";
			}
			AppendEndTagToken(tree, cellEndTag);
		}

	}
}
