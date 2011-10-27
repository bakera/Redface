using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Bakera.RedFace{


	public partial class TreeConstruction{

		private StateManager<InsertionMode> myInsertionModeManager = new StateManager<InsertionMode>();
		private Document myDocument = new Document();
		private StackOfElements myStackOfOpenElements = new StackOfElements();
		private ListOfElements myListOfActiveFormatElements = new ListOfElements();
		private Dictionary<XmlElement, TagToken> myCreatedElementToken = new Dictionary<XmlElement, TagToken>();
		private TagToken myAcknowledgedSelfClosingTag = null;
		private List<CharacterToken> myPendingTableCharacterTokens = new List<CharacterToken>();


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
		public bool FosterParentMode = false;


// コンストラクタ

		public TreeConstruction(RedFaceParser p){
			myParser = p;
			myInsertionModeManager.ParserEventRaised += OnParserEventRaised;
			myInsertionModeManager.SetState<InitialInsertionMode>();
		}


// メソッド
		public void AppendToken(Token t){
			AppendToken(CurrentInsertionMode, t);
		}

		public void AppendToken<T>(Token t) where T : InsertionMode, new() {
			InsertionMode mode = myInsertionModeManager.GetState<T>();
			AppendToken(mode, t);
		}

		private void AppendToken(InsertionMode mode, Token t){
			// 開始タグ直後の改行を無視するケース
			if(IgnoreNextLineFeed){
				if(t.IsLineFeed) return;
				IgnoreNextLineFeed = false;
			}
			if(IsInHtmlContext(t)){
				mode.AppendToken(this, t);
			} else {
				myInsertionModeManager.GetState<InForeignContent>().AppendToken(this, t);
			}
			if(t is StartTagToken && t.SelfClosing && t != myAcknowledgedSelfClosingTag){
				OnParseErrorRaised(string.Format("空要素でない要素に空要素タグを使用することはできません。: {0}", t.Name));
			}
			if(t is EndTagToken && t.Attributes.Length > 0){
				OnParseErrorRaised(string.Format("終了タグに属性を指定することはできません。: {0}", t.Name));

			}
			if(t is EndTagToken && t.SelfClosing){
				OnParseErrorRaised(string.Format("終了タグに空要素タグを使用することはできません。: {0}", t.Name));
			}
			myAcknowledgedSelfClosingTag = null;
		}

		// 渡されたトークンとカレントノードの状態を見て、current insertion mode in HTML content で扱うべきならtrueを返します。
		public bool IsInHtmlContext(Token t){
			XmlElement e = this.CurrentNode as XmlElement;
			if(e == null) return true;
			if(e.NamespaceURI.Equals(Document.HtmlNamespace, StringComparison.InvariantCulture)) return true;
			if(ElementInfo.IsMathMLTextIntegrationPoint(e)){
				if(t is StartTagToken){
					if(!t.IsStartTag("mglyph", "malignmark")) return true;
				}
			}
			if(ElementInfo.IsMathMLNameSpace(e)){
				if(e.Name.Equals("annotation-xml", StringComparison.InvariantCulture)){
					if(t.IsStartTag("svg")) return true;
				}
			}
			if(ElementInfo.IsHtmlIntegrationPoint(e)){
				if(t is StartTagToken) return true;
				if(t is CharacterToken) return true;
			}
			if(t is EndTagToken) return true;
			return false;
		}

		// InsertionModeを変更します。
		public void ChangeInsertionMode<T>() where T : InsertionMode, new(){
			if(CurrentInsertionMode != null && CurrentInsertionMode.GetType() == typeof(T)) return;
			myInsertionModeManager.SetState<T>();
			OnInsertionModeChanged();
		}

		// InsertionModeを元に戻します。
		public void SwitchToOriginalInsertionMode(){
			myInsertionModeManager.SetState(OriginalInsertionMode);
			OriginalInsertionMode = null;
			OnInsertionModeChanged();
		}

		public void ResetInsertionModeAppropriately(){
			// bool last = false;
			// "last" は fragment case の場合のみtrueになりえる
			// 通常は必ずいずれかの要素が存在するのでループが最後まで回ることはない
			// このパーサは fragment case を実装しないので last は使用しない
			XmlElement node = myStackOfOpenElements.Peek();
			while(node != null){
				if(StackOfElements.IsNameMatch(node, "td", "th")){ // && last == false
					ChangeInsertionMode<InCellInsertionMode>();
					return;
				}
				if(StackOfElements.IsNameMatch(node, "tr")){
					ChangeInsertionMode<InRowInsertionMode>();
					return;
				}
				if(StackOfElements.IsNameMatch(node, "tbody", "thead", "tfoot")){
					ChangeInsertionMode<InTableBodyInsertionMode>();
					return;
				}
				if(StackOfElements.IsNameMatch(node, "caption")){
					ChangeInsertionMode<InCaptionInsertionMode>();
					return;
				}
				if(StackOfElements.IsNameMatch(node, "table")){
					ChangeInsertionMode<InTableInsertionMode>();
					return;
				}
				if(StackOfElements.IsNameMatch(node, "body")){
					ChangeInsertionMode<InBodyInsertionMode>();
					return;
				}
				node = myStackOfOpenElements.GetAncestor(node);
			}
		}



// Stack操作
		public void PutToStack(XmlElement e){
			myStackOfOpenElements.Push(e);
		}

		public XmlElement PopFromStack(){
			return myStackOfOpenElements.Pop();
		}


// Pending table character tokens

		public void ClearPendingTableCharacterTokens(){
			myPendingTableCharacterTokens.Clear();
		}

		public void AppendPendingTableCharacterToken(CharacterToken t){
			myPendingTableCharacterTokens.Add(t);
		}

		public CharacterToken[] GetPendingTableCharacterTokens(){
			return myPendingTableCharacterTokens.ToArray();
		}


// ノード作成

		// カレントノードにノードを挿入します。
		// FosterParentモードがONかつカレントノードがtable関係の要素の場合はFosterParentを実行します。
		public void AppendChild(XmlNode x){
			if(FosterParentMode && myStackOfOpenElements.IsTableRealtedElement(CurrentNode as XmlElement)){
				myStackOfOpenElements.FosterParent(x);
			} else {
				CurrentNode.AppendChild(x);
			}
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
			return CreateElementForToken(t, Document.HtmlNamespace);
		}

		// TagTokenに対応する要素を、指定された名前空間で作って返します。
		// XmlElement と TagToken の対応関係をDictionaryに記録します。
		public XmlElement CreateElementForToken(TagToken t, string ns){
			XmlElement result = Document.CreateElement(t.Name, ns);
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
			OnElementInserted();
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
			myAcknowledgedSelfClosingTag = t;
		}

// Tokenの参照

		// 渡されたXmlElementに対応するTagTokenを返します。
		public TagToken GetToken(XmlElement e){
			return myCreatedElementToken[e];
		}


// イベント

		public event EventHandler<ParserEventArgs> ParserEventRaised;

		protected virtual void OnParserEventRaised(Object sender, ParserEventArgs e){
			if(ParserEventRaised != null){
				ParserEventRaised(this, e);
			}
		}

		// ParseErrorRaisedイベントを発生します。
		protected virtual void OnParseErrorRaised(string message){
			OnParserEventRaised(this, new ParserEventArgs(){Message = message});
		}
		protected virtual void OnParseErrorRaised(Object sender, ParserEventArgs args){
			OnParserEventRaised(sender, args);
		}

		// InsertionModeChangedイベントを発生します。
		protected virtual void OnInsertionModeChanged(){
			OnParserEventRaised(this, new ParserEventArgs());
		}

		// ElementInsertedイベントを発生します。
		protected virtual void OnElementInserted(){
			OnParserEventRaised(this, new ParserEventArgs(null));
		}



	}
}



