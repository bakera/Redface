using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bakera.RedFace{

	public class StateManager<T> : KeyedByTypeCollection<T> where T : RedFaceParserState{

		public T CurrentState{get; private set;}
		private RedFaceParser myParser = null;

		public StateManager(RedFaceParser parser){
			myParser = parser;
		}

		public void SetState<U>() where U : T, new(){
			Type t = typeof(U);
			if(!this.Contains(t)){
				this.Add(new U());
			}
			CurrentState = this[t];
		}

	}

}



