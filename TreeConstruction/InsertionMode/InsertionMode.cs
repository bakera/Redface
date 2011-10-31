using System;
using System.Xml;

namespace Bakera.RedFace{

	public abstract class InsertionMode : RedFaceParserState{
		
// メソッド
		public void AppendToken(TreeConstruction tree, Token token){
			if(token is CharacterToken){
				AppendCharacterToken(tree, (CharacterToken)token);
				return;
			} else if(token is CommentToken){
				AppendCommentToken(tree, (CommentToken)token);
				return;
			} else if(token is DoctypeToken){
				AppendDoctypeToken(tree, (DoctypeToken)token);
				return;
			} else if(token is StartTagToken) {
				AppendStartTagToken(tree, (StartTagToken)token);
				return;
			} else if(token is EndTagToken) {
				AppendEndTagToken(tree, (EndTagToken)token);
				return;
			} else if(token is EndOfFileToken) {
				AppendEndOfFileToken(tree, (EndOfFileToken)token);
				return;
			}
			AppendAnythingElse(tree, token);
		}

		public virtual void AppendCharacterToken(TreeConstruction tree, CharacterToken token){
			AppendAnythingElse(tree, token);
		}
		public virtual void AppendCommentToken(TreeConstruction tree, CommentToken token){
			AppendAnythingElse(tree, token);
		}
		public virtual void AppendDoctypeToken(TreeConstruction tree, DoctypeToken token){
			AppendAnythingElse(tree, token);
		}
		public virtual void AppendStartTagToken(TreeConstruction tree, StartTagToken token){
			AppendAnythingElse(tree, token);
		}
		public virtual void AppendEndTagToken(TreeConstruction tree, EndTagToken token){
			AppendAnythingElse(tree, token);
		}
		public virtual void AppendEndOfFileToken(TreeConstruction tree, EndOfFileToken token){
			AppendAnythingElse(tree, token);
		}
		public virtual void AppendAnythingElse(TreeConstruction tree, Token token){
		}




		// 補える終了タグを補う処理
		// 例外を指定
		protected void GenerateImpliedEndTags(TreeConstruction tree, Token token, params string[] except){
			while(tree.StackOfOpenElements.IsImpliedEndTagElement()){
				if(tree.StackOfOpenElements.IsCurrentNameMatch(except)) break;
				XmlElement e = tree.StackOfOpenElements.Pop();
				OnImpliedEndTagInserted(e, token);
			}
			return;
		}



// Text Parsing 
		protected void GenericRCDATAElementParsingAlgorithm(TreeConstruction tree, Token token){
			tree.InsertElementForToken((TagToken)token);
			tree.Parser.ChangeTokenState<RCDATAState>();
			tree.OriginalInsertionMode = tree.CurrentInsertionMode;
			tree.ChangeInsertionMode<TextInsertionMode>();
		}

		protected void GenericRawtextElementParsingAlgorithm(TreeConstruction tree, Token token){
			tree.InsertElementForToken((TagToken)token);
			tree.Parser.ChangeTokenState<RAWTEXTState>();
			tree.OriginalInsertionMode = tree.CurrentInsertionMode;
			tree.ChangeInsertionMode<TextInsertionMode>();
		}


// プロパティ
		public virtual string Name{
			get{return this.GetType().Name;}
		}

	}
}
