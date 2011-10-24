using System;
using System.Reflection;
using System.Xml;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class InTableInsertionMode : TableRelatedInsertionMode{

			public override void AppendToken(TreeConstruction tree, Token token){

				if(token is CharacterToken){
					tree.ClearPendingTableCharacterTokens();
					tree.OriginalInsertionMode = tree.CurrentInsertionMode;
					tree.ChangeInsertionMode<InTableTextInsertionMode>();
					return;
				}

				if(token is CommentToken){
					tree.AppendCommentForToken((CommentToken)token);
					return;
				}

				if(token is DoctypeToken){
					tree.Parser.OnParseErrorRaised(string.Format("先頭以外の箇所に文書型宣言があります。"));
					return;
				}

				if(token.IsStartTag("caption")){
					tree.StackOfOpenElements.ClearBackToTable();
					tree.ListOfActiveFormatElements.InsertMarker();
					tree.InsertElementForToken((TagToken)token);
					tree.ChangeInsertionMode<InCaptionInsertionMode>();
					return;

				}


				Console.WriteLine("========\nnot implemented: {0} - {1}", this.Name, token);
				tree.Parser.Stop();
				return;





			}

		}
	}
}
