using System;
using System.Reflection;
using System.Xml;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class BeforeHeadInsertionMode : InsertionMode{

			public override void AppendToken(TreeConstruction tree, Token token){

				if(token.IsWhiteSpace){
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

				if(token.IsStartTag("html")){
					// ToDo:
					// Process the token using the rules for the "in body" insertion mode.
					Console.WriteLine("not implemented:" + this.GetType().Name);
					tree.Parser.Stop();
					return;
				}

				if(token.IsStartTag("head")){
					XmlElement headElement = tree.InsertElementForToken((StartTagToken)token);
					tree.HeadElementPointer = headElement;
					tree.ChangeInsertionMode<InHeadInsertionMode>();
					return;
				}

				if(token is EndTagToken && !token.IsEndTag("head", "body", "html", "br")){
					tree.Parser.OnParseErrorRaised(string.Format("不明な終了タグがあります。"));
					return;
				}

				XmlElement defaultHeadElement = tree.Document.CreateElement("head");
				tree.InsertElement(defaultHeadElement);
				tree.HeadElementPointer = defaultHeadElement;
				tree.ReprocessFlag = true;
				tree.ChangeInsertionMode<InHeadInsertionMode>();
				return;
			}

		}
	}
}
