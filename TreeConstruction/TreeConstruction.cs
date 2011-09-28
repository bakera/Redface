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
			private StackOfOpenElements myStackOfOpenElements = new StackOfOpenElements();
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

			public StackOfOpenElements StackOfOpenElements{
				get{
					return myStackOfOpenElements;
				}
			}

			public XmlElement HeadElementPointer{get; set;}
			public XmlElement FormElementPointer{get; set;}

			public bool ReprocessFlag{get; set;}


	// コンストラクタ

			public TreeConstruction(RedFaceParser p){
				myParser = p;
				myInsertionModeManager = new StateManager<InsertionMode>(p);
				myInsertionModeManager.SetState<InitialInsertionMode>();
			}


	// メソッド
			public void AppendToken(Token t){
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

			// TagTokenに対応する要素を作ります。
			public XmlElement CreateElementForToken(TagToken t){
				XmlElement result = Document.CreateElement(t.Name);
				foreach(AttributeToken at in t.Attributes){
					result.SetAttribute(at.Name, at.Value);
				}
				return result;
			}

			// TagTokenに対応する要素を作って挿入します。
			public XmlElement InsertElementForToken(TagToken t){
				XmlElement result = CreateElementForToken(t);
				return InsertElement(result);
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

			public void MergeAttribute(XmlElement e, TagToken t){
				foreach(AttributeToken at in t.Attributes){
					if(e.Attributes[at.Name] == null) e.SetAttribute(at.Name, at.Value);
				}
			}


			public void AcknowledgeSelfClosingFlag(TagToken t){
				//ToDo:
				Console.WriteLine("AcknowledgeSelfClosingFlag: {0}", t.Name);

			}

		}
	}

}



