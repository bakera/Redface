using System;
using System.Xml;

namespace Bakera.RedFace{

	public class InSelectInTableInsertionMode : InsertionMode{

		public override void AppendStartTagToken(TreeConstruction tree, StartTagToken token){
			switch(token.Name){
			case "caption":
			case "table":
			case "tbody":
			case "tfoot":
			case "thead":
			case "tr":
			case "td":
			case "th":
				OnMessageRaised(new UnexpectedStartTagInSelectInTableError(token.Name));
				AppendEndTagToken(tree, new FakeEndTagToken(){Name = "select"});
				tree.ReprocessFlag = true;
				return;
			}
			AppendAnythingElse(tree, token);
		}

		public override void AppendEndTagToken(TreeConstruction tree, EndTagToken token){
			switch(token.Name){
			case "caption":
			case "table":
			case "tbody":
			case "tfoot":
			case "thead":
			case "tr":
			case "td":
			case "th":
				OnMessageRaised(new UnexpectedEndTagInSelectInTableError(token.Name));
				if(tree.StackOfOpenElements.HaveElementInTableScope(token.Name)){
					AppendEndTagToken(tree, new FakeEndTagToken(){Name = "select"});
					tree.ReprocessFlag = true;
					return;
				}
				return;
			}
			AppendAnythingElse(tree, token);
		}


		public override void AppendAnythingElse(TreeConstruction tree, Token token){
			tree.AppendToken<InSelectInsertionMode>(token);
		}

	}
}
