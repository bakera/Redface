using System;
using System.Xml;

namespace Bakera.RedFace{

	public class AttributeInfo{

		public virtual string Prefix{get; protected set;}
		public virtual string LocalName{get; protected set;}
		public virtual string Namespace{get; protected set;}

		public AttributeInfo(string name){
			this.LocalName = name;
		}

		public AttributeInfo(string prefix, string name, string nameSpace){
			this.LocalName = name;
			this.Prefix = prefix;
			this.Namespace = nameSpace;
		}


	}

}



