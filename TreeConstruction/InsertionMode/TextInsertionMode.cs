using System;
using System.Xml;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class TextInsertionMode : InsertionMode{

			public override void AppendToken(TreeConstruction tree, Token token){

				if(token is CharacterToken){
					tree.InsertCharacter((CharacterToken)token);
					return;
				}

				if(token is EndOfFileToken){
					tree.Parser.OnParseErrorRaised(string.Format("テキストの途中で終端に達しました。終了タグがありません。"));
					// Ignore?: 
					// If the current node is a script element, mark the script element as "already started".
					tree.PopFromStack();
					tree.SwitchToOriginalInsertionMode();
					tree.ReprocessFlag = true;
					return;
				}

				if(token.IsEndTag("script")){
					// Ignore?: Provide a stable state.
					// XmlElement script = tree.CurrentNode as XmlElement;
					tree.PopFromStack();
					tree.SwitchToOriginalInsertionMode();
					// Ignore? script etc.
					return;
				}

				if(token is EndTagToken){
					tree.PopFromStack();
					tree.SwitchToOriginalInsertionMode();
					return;
				}

				throw new Exception(string.Format("このモードでの処理が定義されていないトークンです。モード: {0} トークン: {1}", this.Name, token));
			}

		}
	}
}
