using System;
using System.Collections.Generic;

namespace Bakera.RedFace{


/*
 Start and end tag tokens have a tag name, a self-closing flag, and a list of attributes, each of which has a name and a value.
 When a start or end tag token is created, its self-closing flag must be unset (its other state is that it be set), and its attributes list must be empty.
*/

	public abstract class TagToken : Token{

		public string Name{get; set;}
		public bool SelfClosing{get; set;}
		private Dictionary<string, string> myAttributes = new Dictionary<string, string>();

		public TagToken(){
			Name = null;
			SelfClosing = false;
		}


		public string this[string key]{
			get{return myAttributes[key];}
		}

		public bool HasAttribute(string key){
			return myAttributes.ContainsKey(key);
		}

		public void AddAttribute(string key, string value){
			myAttributes.Add(key, value);
		}


	}
}
