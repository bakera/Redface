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
				OnParseErrorRaised(string.Format("tbody要素直下に{0}要素の開始タグが出現しました。", token.Name));
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
					OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", token.Name));
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
				OnParseErrorRaised(string.Format("終了タグが出現しましたが、対応する開始タグがありません。: {0}", token.Name));
				return;
			}
			AppendAnythingElse(tree, token);
		}

		public override void AppendAnythingElse(TreeConstruction tree, Token token){
			tree.AppendToken<InTableInsertionMode>(token);
		}
	}
}
