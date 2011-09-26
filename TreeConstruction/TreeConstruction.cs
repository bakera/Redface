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
			private Stack<XmlElement> myStackOfOpenlement = null;
			private RedFaceParser myParser = null;
			public RedFaceParser Parser {
				get{
					return myParser;
				}
			}

			public InsertionMode CurrentInsertionMode{
				get{return myInsertionModeManager.CurrentState;}
			}

			public Document Document{
				get{return myDocument;}
			}

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

			// InsertionModeを変更します。
			public void ChangeInsertionMode<T>() where T : InsertionMode, new(){
				if(CurrentInsertionMode != null && CurrentInsertionMode.GetType() == typeof(T)) return;
				myInsertionModeManager.SetState<T>();
				Parser.OnInsertionModeChanged();
			}

		}
	}

}



