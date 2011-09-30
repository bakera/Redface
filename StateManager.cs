using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bakera.RedFace{

	public class StateManager<T> : KeyedByTypeCollection<T> where T : RedFaceParserState{

		public T CurrentState{get; private set;}
		public T PreviousState{get; private set;}
		private RedFaceParser myParser = null;

		public StateManager(RedFaceParser parser){
			myParser = parser;
		}

		public void SetState<U>() where U : T, new(){
			PreviousState = CurrentState;
			CurrentState = GetState<U>();
		}

		public void SetState(T state){
			if(!this.Contains(state)){
				this.Add(state);
			}
			PreviousState = CurrentState;
			CurrentState = state;
		}

		public T GetState<U>() where U : T, new(){
			Type t = typeof(U);
			if(!this.Contains(t)){
				this.Add(new U());
			}
			return this[t];
		}

		// ひとつ前のステータスに戻します。
		public void BackState(){
			T temp = CurrentState;
			CurrentState = PreviousState;
			PreviousState = temp;
		}


	}

}



