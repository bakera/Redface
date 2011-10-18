using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public partial class TreeConstruction{

			private StateManager<InsertionMode> myInsertionModeManager = null;
			private Document myDocument = new Document();
			private StackOfElements myStackOfOpenElements = new StackOfElements();
			private ListOfElements myListOfActiveFormatElements = new ListOfElements();
			private Dictionary<XmlElement, TagToken> myCreatedElementToken = new Dictionary<XmlElement, TagToken>();

			private RedFaceParser myParser = null;
			public RedFaceParser Parser {
				get{
					return myParser;
				}
			}

			public InsertionMode CurrentInsertionMode{
				get{return myInsertionModeManager.CurrentState;}
			}
			public InsertionMode OriginalInsertionMode{
				get; set;
			}

			public Document Document{
				get{return myDocument;}
			}

			public XmlNode CurrentNode{
				get{
					if(myStackOfOpenElements.Count == 0) return myDocument;
					return myStackOfOpenElements.Peek();
				}
			}

			public StackOfElements StackOfOpenElements{
				get{
					return myStackOfOpenElements;
				}
			}

			public ListOfElements ListOfActiveFormatElements{
				get{
					return myListOfActiveFormatElements;
				}
			}

			public XmlElement HeadElementPointer{get; set;}
			public XmlElement FormElementPointer{get; set;}

			public bool ReprocessFlag{get; set;}
			public bool IgnoreNextLineFeed{get; set;}


	// コンストラクタ

			public TreeConstruction(RedFaceParser p){
				myParser = p;
				myInsertionModeManager = new StateManager<InsertionMode>(p);
				myInsertionModeManager.SetState<InitialInsertionMode>();
			}


	// メソッド
			public void AppendToken(Token t){
				// 開始タグ直後の改行を無視するケース
				if(IgnoreNextLineFeed){
					if(t.IsLineFeed) return;
					IgnoreNextLineFeed = false;
				}
				CurrentInsertionMode.AppendToken(this, t);
			}
			public void AppendToken<T>(Token t) where T : InsertionMode, new() {
				InsertionMode mode = myInsertionModeManager.GetState<T>();
				mode.AppendToken(this, t);
			}

			// InsertionModeを変更します。
			public void ChangeInsertionMode<T>() where T : InsertionMode, new(){
				if(CurrentInsertionMode != null && CurrentInsertionMode.GetType() == typeof(T)) return;
				myInsertionModeManager.SetState<T>();
				Parser.OnInsertionModeChanged();
			}

			// InsertionModeを元に戻します。
			public void SwitchToOriginalInsertionMode(){
				myInsertionModeManager.SetState(OriginalInsertionMode);
				OriginalInsertionMode = null;
				Parser.OnInsertionModeChanged();
			}


	// Stack操作
			public void PutToStack(XmlElement e){
				myStackOfOpenElements.Push(e);
			}
			public XmlElement PopFromStack(){
				return myStackOfOpenElements.Pop();
			}



	// ノード作成

			public void AppendChild(XmlNode x){
				CurrentNode.AppendChild(x);
			}

			// CommentTokenに対応するコメントを作ります。
			public XmlComment CreateCommentForToken(CommentToken token){
				XmlComment result = Document.CreateComment(token.Data);
				return result;
			}

			// CommentTokenに対応するコメントを作って挿入します。
			public void AppendCommentForToken(CommentToken token){
				XmlNode result = this.CreateCommentForToken(token);
				AppendChild(result);
			}

			// TagTokenに対応する要素を作って返します。
			// XmlElement と TagToken の対応関係をDictionaryに記録します。
			public XmlElement CreateElementForToken(TagToken t){
				XmlElement result = Document.CreateHtmlElement(t.Name);
				foreach(AttributeToken at in t.Attributes){
					result.SetAttribute(at.Name, at.Value);
				}
				myCreatedElementToken.Add(result, t);
				return result;
			}

			// TagTokenに対応する要素を作って挿入します。
			public XmlElement InsertElementForToken(TagToken t){
				XmlElement result = CreateElementForToken(t);
				return InsertElement(result);
			}

			// 渡された名前の要素を作って挿入します。
			// tagTokenをねつ造し、XmlElement と TagToken の対応関係を記録します。
			// isindex要素の処理に使われます。
			public XmlElement InsertElementForToken(string s){
				StartTagToken token = new StartTagToken();
				token.Name = s;
				return InsertElementForToken(token);
			}

			// XmlElementをCurrentNodeに挿入します。
			public XmlElement InsertElement(XmlElement e){
				Parser.OnElementInserted();
				AppendChild(e);
				PutToStack(e);
				return e;
			}

			public XmlNode InsertCharacter(CharacterToken t){
				XmlText result = Document.CreateTextNode(t.Data);
				AppendChild(result);
				return result;
			}

			// 文字列をカレントノードに挿入します。
			// isindexの処理に使われます。
			public XmlNode InsertText(string s){
				XmlText result = Document.CreateTextNode(s);
				AppendChild(result);
				return result;
			}

			public void MergeAttribute(XmlElement e, TagToken t){
				foreach(AttributeToken at in t.Attributes){
					if(e.Attributes[at.Name] == null) e.SetAttribute(at.Name, at.Value);
				}
			}

			public void AcknowledgeSelfClosingFlag(TagToken t){
				//ToDo:
				Console.WriteLine("AcknowledgeSelfClosingFlag: {0}", t.Name);

			}

// Tokenの参照

			// 渡されたXmlElementに対応するTagTokenを返します。
			public TagToken GetToken(XmlElement e){
				return myCreatedElementToken[e];
			}

		}
	}

}



