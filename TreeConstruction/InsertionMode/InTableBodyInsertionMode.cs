using System;
using System.Xml;

namespace Bakera.RedFace{

	public class InTableBodyInsertionMode : TableRelatedInsertionMode{


		public override void AppendStartTagToken(TreeConstruction tree, StartTagToken token){
			switch(token.Name){
			case "tr":
				tree.StackOfOpenElements.ClearBackToTableBody();
				tree.InsertElementForToken(token);
				tree.ChangeInsertionMode<InRowInsertionMode>();
				return;
			case "th":
			case "td":
				OnMessageRaised(new CellWithoutTableRowError(token.Name));
				AppendStartTagToken(tree, new FakeStartTagToken(){Name = "tr"});
				tree.ReprocessFlag = true;
				return;
			case "caption":
			case "col":
			case "colgroup":
			case "tbody":
			case "tfoot":
			case "thead":
				tree.StackOfOpenElements.ClearBackToTableBody();
				AppendEndTagToken(tree, new FakeEndTagToken(){Name = tree.CurrentNode.Name});
				tree.ReprocessFlag = true;
				return;
			}
			AppendAnythingElse(tree, token);
		}

		public override void AppendEndTagToken(TreeConstruction tree, EndTagToken token){
			switch(token.Name){
			case "tbody":
			case "tfoot":
			case "thead":
				if(!tree.StackOfOpenElements.HaveElementInTableScope(token.Name)){
					OnMessageRaised(new LonlyEndTagError(token.Name));
					return;
				}
				tree.StackOfOpenElements.ClearBackToTableBody();
				tree.PopFromStack();
				tree.ChangeInsertionMode<InTableInsertionMode>();
				return;
			case "table":
				tree.StackOfOpenElements.ClearBackToTableBody();
				AppendEndTagToken(tree, new FakeEndTagToken(){Name = tree.CurrentNode.Name});
				tree.ReprocessFlag = true;
				return;
			case "body":
			case "caption":
			case "col":
			case "colgroup":
			case "html":
			case "td":
			case "th":
			case "tr":
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
