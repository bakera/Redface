using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Bakera.RedFace{


	public partial class TreeConstruction : ParserEventSender{

		private StateManager<InsertionMode> myInsertionModeManager = new StateManager<InsertionMode>();
		private Document myDocument = new Document();
		private StackOfElements myStackOfOpenElements = new StackOfElements();
		private ListOfElements myListOfActiveFormatElements = new ListOfElements();
		private Dictionary<XmlElement, TagToken> myCreatedElementToken = new Dictionary<XmlElement, TagToken>();
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
			do{
				ReprocessFlag = false;
				AppendToken(CurrentInsertionMode, t);
			}while(ReprocessFlag);
			if(t is StartTagToken && t.SelfClosing && !t.AcknowledgedSelfClosing){
				OnMessageRaised(new NotAcknowledgedSelfClosingTagError(t.Name));
			}
			if(t is EndTagToken && t.Attributes.Length > 0){
				OnMessageRaised(new EndTagWithAttributeError(t.Name));
			}
			if(t is EndTagToken && t.SelfClosing){
				OnMessageRaised(new SelfClosingEndTagError(t.Name));
			}
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
				OnMessageRaised(EventLevel.Verbose, string.Format("HTMLコンテキストでトークンを挿入します。: {0}", t));
				t.AppendTo(this, mode);
			} else {
				OnMessageRaised(EventLevel.Verbose, string.Format("InForeignContentコンテキストでトークンを挿入します。: {0}", t));
				t.AppendTo(this, myInsertionModeManager.GetState<InForeignContent>());
			}
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
			OnMessageRaised(EventLevel.Verbose, string.Format("InsertionMode を変更しました: {0}", CurrentInsertionMode));
		}

		// InsertionModeを元に戻します。
		public void SwitchToOriginalInsertionMode(){
			myInsertionModeManager.SetState(OriginalInsertionMode);
			OriginalInsertionMode = null;
			OnMessageRaised(EventLevel.Verbose, string.Format("InsertionMode を元に戻しました: {0}", CurrentInsertionMode));
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

		// TagTokenに対応する要素を作って挿入します。
		public XmlElement InsertElementForToken(TagToken t){
			XmlElement result = CreateElementForToken(t);
			return InsertElement(result);
		}

		// TagTokenに対応する要素を、指定された名前空間で作って返します。
		// XmlElement と TagToken の対応関係をDictionaryに記録します。
		public XmlElement CreateElementForToken(TagToken t, string ns){
			string name = Document.ReplaceInvalidXmlName(t.Name);
			if(name != t.Name){
				OnMessageRaised(EventLevel.Alert, string.Format("要素名 {0} はXMLで使用できない文字を含んでいます。", t.Name));
			}
			XmlElement result = Document.CreateElement(name, ns);
			foreach(AttributeToken at in t.Attributes){
				string attrName = Document.ReplaceInvalidXmlName(at.Name);
				if(attrName != at.Name){
					OnMessageRaised(EventLevel.Alert, string.Format("属性名 {0} はXMLで使用できない文字を含んでいます。", at.Name));
				}
				try{
					result.SetAttribute(attrName, at.Value);
				} catch(XmlException e){
					OnMessageRaised(EventLevel.Alert, string.Format("XMLのエラーが発生しました。: {0}", e.Message));
				}
			}
			myCreatedElementToken.Add(result, t);
			return result;
		}


		// TagTokenと名前空間を指定して、対応する要素を作って挿入します。
		// TagTokenが指定された名前空間と異なるxmlnsを持つ場合はエラーとなります。
		public XmlElement InsertForeignElementForToken(TagToken t, string ns){
			XmlElement result = CreateElementForToken(t, ns);
			string xmlns = result.GetAttribute("xmlns", Document.XmlnsNamespace);
			if(xmlns != null && !xmlns.Equals(ns, StringComparison.InvariantCulture)){
				OnMessageRaised(new UnexpectedNamespaceError(t.Name, xmlns, ns));
			}

			string xlink = result.GetAttribute("xmlns:xlink");
			if(xlink != null && !xlink.Equals(Document.XLinkNamespace, StringComparison.InvariantCulture)){
				OnMessageRaised(new UnexpectedXlinkNamespaceError(Document.XLinkNamespace, t.Name));
			}
			return InsertElement(result);
		}

		// XmlElementをCurrentNodeに挿入します。
		public XmlElement InsertElement(XmlElement e){
			OnMessageRaised(EventLevel.Verbose, string.Format("要素を挿入しました。: {0}", e.Name));
			AppendChild(e);
			PutToStack(e);
			return e;
		}

		// CharacterTokenに対応する文字を挿入します。
		public XmlNode InsertCharacter(CharacterToken t){
			XmlText result = Document.CreateTextNode(t.Data);
			AppendChild(result);
			return result;
		}

		// 渡された文字を挿入します。
		public XmlNode InsertCharacter(Char c){
			XmlText result = Document.CreateTextNode(c.ToString());
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
			t.AcknowledgedSelfClosing = true;
			OnMessageRaised(EventLevel.Verbose, string.Format("終了タグの省略が可能なトークンです。: {0}", t.Name));
		}

// Tokenの参照

		// 渡されたXmlElementに対応するTagTokenを返します。
		public TagToken GetToken(XmlElement e){
			return myCreatedElementToken[e];
		}



	}
}



