using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public partial class TreeConstruction{

			private KeyedByTypeCollection<InsertionMode> myInsertionModeManager = new KeyedByTypeCollection<InsertionMode>();

			private XmlDocument myDocumentTree = new XmlDocument(){XmlResolver=null};
			private Stack<XmlElement> myStackOfOpenlement = null;
			private InsertionMode myCurrentInsertionMode = null;
			private RedFaceParser myParser = null;
			public RedFaceParser Parser {
				get{
					return myParser;
				}
			}



	// コンストラクタ

			public TreeConstruction(RedFaceParser p){
				myParser = p;
				SetInsertionMode(typeof(InitialMode));
			}


	// メソッド
			public void AppendToken(Token t){
				myCurrentInsertionMode.AppendToken(t);
			}

			// InsertionModeを変更します。
			public void ChangeInsertionMode(Type t){
				if(myCurrentInsertionMode != null && t == myCurrentInsertionMode.GetType()) return;
				SetInsertionMode(t);
				Parser.OnInsertionModeChanged();
			}

			// InsertionModeを設定します。
			public void SetInsertionMode(Type t){
				if(!myInsertionModeManager.Contains(t)){
					myInsertionModeManager.Add(InsertionMode.CreateInsertionMode(t, Parser));
				}
				myCurrentInsertionMode = myInsertionModeManager[t];
			}


		}
	}

}



