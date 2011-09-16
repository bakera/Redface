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
		private Dictionary<string, Attribute> myAttributes = new Dictionary<string, Attribute>();
		private Attribute myCurrentAttribute;

		public Attribute CurrentAttribute{
			get{return myCurrentAttribute;}
		}

		public TagToken(){
			Name = null;
			SelfClosing = false;
		}


		public bool HasAttribute(string key){
			return myAttributes.ContainsKey(key);
		}

		public Attribute CreateAttribute(){
			myCurrentAttribute = new Attribute();
			return CurrentAttribute;
		}

		public bool FixAttribute(){
			string attrName = myCurrentAttribute.Name;
			if(myAttributes.ContainsKey(attrName)) return false;
			myAttributes.Add(attrName, myCurrentAttribute);
			myCurrentAttribute = null;
			return true;
		}

		public override string ToString(){
			string result = string.Format("{0} / Name: \"{1}\"", this.GetType().Name, this.Name);
			return result;
		}


	}
}
