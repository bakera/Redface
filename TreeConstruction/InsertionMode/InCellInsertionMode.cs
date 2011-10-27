using System;
using System.Xml;

namespace Bakera.RedFace{

	public class InCellInsertionMode : TableRelatedInsertionMode{

		protected override void AppendDoctypeToken(TreeConstruction tree, DoctypeToken token){
			OnParseErrorRaised(string.Format("先頭以外の箇所に文書型宣言があります。"));
		}

		protected override void AppendCommentToken(TreeConstruction tree, CommentToken token){
			tree.AppendCommentForToken(token);
		}

		protected override void AppendCharacterToken(TreeConstruction tree, CharacterToken token){
			tree.ClearPendingTableCharacterTokens();
			tree.OriginalInsertionMode = tree.CurrentInsertionMode;
			tree.ChangeInsertionMode<InTableTextInsertionMode>();
			tree.ReprocessFlag = true;
		}

		protected override void AppendStartTagToken(TreeConstruction tree, StartTagToken token){
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
			}
		}

		protected override void AppendAnythingElse(TreeConstruction tree, Token token){
			Console.WriteLine("========\nnot implemented: {0} - {1}", this.Name, token);
			tree.Parser.Stop();
			return;
		}
	}
}
