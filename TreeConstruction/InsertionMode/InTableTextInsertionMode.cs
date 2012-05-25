using System;
using System.Xml;

namespace Bakera.RedFace{


	public class InTableTextInsertionMode : TableRelatedInsertionMode{

		public override void AppendCharacterToken(TreeConstruction tree, CharacterToken token){
			if(token.IsNULL){
				OnMessageRaised(new NullInDataError());
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
				// reprocess the character tokens in the pending table character tokens list using the rules given in the "anything else" entry in the "in table" insertion mode.
				OnMessageRaised(new FosterParentedTextError());
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
