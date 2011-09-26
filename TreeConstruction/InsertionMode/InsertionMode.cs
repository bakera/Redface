using System;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public abstract class InsertionMode : RedFaceParserState{
			

	// メソッド
			public abstract void AppendToken(TreeConstruction tc, Token t);


			protected void GenericRCDATAElementParsingAlgorithm(TreeConstruction tree, Token token){
				tree.InsertElementForToken((TagToken)token);
				tree.Parser.ChangeTokenState<RCDATAState>();
				tree.OriginalInsertionMode = tree.CurrentInsertionMode;
				tree.ChangeInsertionMode<TextInsertionMode>();
			}

			protected void GenericRawtextElementParsingAlgorithm(TreeConstruction tree, Token token){
				tree.InsertElementForToken((TagToken)token);
// ToDo:				tree.Parser.ChangeTokenState<RCDATAState>();
				tree.OriginalInsertionMode = tree.CurrentInsertionMode;
				tree.ChangeInsertionMode<TextInsertionMode>();
			}

	// プロパティ
			public virtual string Name{
				get{return this.GetType().Name;}
			}

		}
	}
}
