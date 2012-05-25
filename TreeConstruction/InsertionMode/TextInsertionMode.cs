using System;
using System.Xml;

namespace Bakera.RedFace{

	public class TextInsertionMode : InsertionMode{

		public override void AppendCharacterToken(TreeConstruction tree, CharacterToken token){
			tree.InsertCharacter(token);
		}

		public override void AppendEndOfFileToken(TreeConstruction tree, EndOfFileToken token){
			OnMessageRaised(new SuddenlyEndAtElementError(tree.CurrentNode.Name));
			// Ignore?: 
			// If the current node is a script element, mark the script element as "already started".
			tree.PopFromStack();
			tree.SwitchToOriginalInsertionMode();
			tree.ReprocessFlag = true;
			return;
		}


		public override void AppendCommentToken(TreeConstruction tree, CommentToken token){
			XmlComment comment = tree.CreateCommentForToken(token);
			tree.Document.AppendChild(comment);
			return;
		}

		public override void AppendDoctypeToken(TreeConstruction tree, DoctypeToken token){
			tree.AppendToken<InBodyInsertionMode>(token);
		}


		public override void AppendEndTagToken(TreeConstruction tree, EndTagToken token){
			switch(token.Name){
			case "script":
				// Ignore?: Provide a stable state.
				// XmlElement script = tree.CurrentNode as XmlElement;
				tree.PopFromStack();
				tree.SwitchToOriginalInsertionMode();
				// Ignore? script etc.
				return;
			}

			// Any Other End Tag
			tree.PopFromStack();
			tree.SwitchToOriginalInsertionMode();
			return;
		}


		public override void AppendAnythingElse(TreeConstruction tree, Token token){
			// ここには来ないはず (来たらバグ)
			throw new Exception(string.Format("このモードでの処理が定義されていないトークンです。モード: {0} トークン: {1}", this.Name, token));
		}

	}
}
