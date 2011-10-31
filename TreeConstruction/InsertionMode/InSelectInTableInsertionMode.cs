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
				OnParseErrorRaised(string.Format("select要素の中に出現できない要素です。: {0}", token.Name));
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
				OnParseErrorRaised(string.Format("select要素の中に出現できない終了タグです。: {0}", token.Name));
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
