using System;
using System.Xml;

namespace Bakera.RedFace{


	public class InTableTextInsertionMode : TableRelatedInsertionMode{

		public override void AppendCharacterToken(TreeConstruction tree, CharacterToken token){
			if(token.IsNULL){
				OnParseErrorRaised(string.Format("NUL文字が出現しました。"));
				return;
			}
			tree.AppendPendingTableCharacterToken(token);
		}

		public override void AppendAnythingElse(TreeConstruction tree, Token token){
			CharacterToken[] tokenlist = tree.GetPendingTableCharacterTokens();
			// 空白類以外があるかチェック
			if(Array.Find(tokenlist, (c)=>!c.IsSpaceCharacter) == null){
				// 空白類しかない
				Array.ForEach(tokenlist, (c)=>tree.InsertCharacter(c));
			} else {
				OnParseErrorRaised(string.Format("table要素の中にテキストが出現しました。" ));
				tree.FosterParentMode = true;
				Array.ForEach(tokenlist, (c)=>tree.AppendToken<InBodyInsertionMode>(c));
				tree.FosterParentMode = false;
			}
			tree.SwitchToOriginalInsertionMode();
			tree.ReprocessFlag = true;
			return;
		}

	}
}
