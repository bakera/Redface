using System;
using System.Xml;

namespace Bakera.RedFace{

	public class InRowInsertionMode : TableRelatedInsertionMode{

		public override void AppendStartTagToken(TreeConstruction tree, StartTagToken token){
			switch(token.Name){
			case "th":
			case "td":
				tree.StackOfOpenElements.ClearBackToTableRow();
				tree.InsertElementForToken(token);
				tree.ChangeInsertionMode<InCellInsertionMode>();
				tree.ListOfActiveFormatElements.InsertMarker();
				return;

			case "caption":
			case "col":
			case "colgroup":
			case "tbody":
			case "tfoot":
			case "thead":
			case "tr":
				AppendEndTagToken(tree, new FakeEndTagToken(){Name = "tr"});
				tree.ReprocessFlag = true;
				return;
			}
			AppendAnythingElse(tree, token);
		}

		public override void AppendEndTagToken(TreeConstruction tree, EndTagToken token){
			switch(token.Name){
			case "tr":
				tree.StackOfOpenElements.ClearBackToTableRow();
				tree.PopFromStack();
				tree.ChangeInsertionMode<InTableBodyInsertionMode>();
				return;

			case "table":
				AppendEndTagToken(tree, new FakeEndTagToken(){Name = "tr"});
				tree.ReprocessFlag = true;
				return;

			case "tbody":
			case "tfoot":
			case "thead":
				if(!tree.StackOfOpenElements.HaveElementInTableScope(token.Name)){
					OnMessageRaised(new LonlyEndTagError(token.Name));
					return;
				}
				AppendEndTagToken(tree, new FakeEndTagToken(){Name = "tr"});
				tree.ReprocessFlag = true;
				return;

			case "body":
			case "caption":
			case "col":
			case "colgroup":
			case "html":
			case "td":
			case "th":
				OnMessageRaised(new LonlyEndTagError(token.Name));
				return;
			}
			AppendAnythingElse(tree, token);
		}

		public override void AppendAnythingElse(TreeConstruction tree, Token token){
			tree.AppendToken<InTableInsertionMode>(token);
		}

	}
}
