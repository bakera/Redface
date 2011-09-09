using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bakera.RedFace{

	public class StateManager<T> : KeyedByTypeCollection<T> where T : RedFaceParserState, new(){

		public T CurrentState{get; private set;}
		private RedFaceParser myParser = null;

		public void SetState(Type t){
			if(!this.Contains(t)){
				this.Add(Create(t));
			}
			CurrentState = this[t];
		}

		public T Create(Type t){
			if(!(typeof(T)).IsAssignableFrom(t)){
				throw new Exception("Createメソッドはこの型のインスタンスを作成できません。渡された型 :" + t.ToString());
			}
			ConstructorInfo ci = t.GetConstructor(new Type[]{typeof(RedFaceParser)});
			T result = ci.Invoke(new Object[]{myParser}) as T;
			return result;
		}

		public U Create<U>() where U : T, new(){
			U result = new U();
			result.Parser = myParser;
			return result;
		}


	}

}



