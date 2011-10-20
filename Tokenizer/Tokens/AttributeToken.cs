using System;
using System.Collections.Generic;

namespace Bakera.RedFace{

	public class AttributeToken{

		public string Name{get; set;}
		public string Value{get; set;}
		public bool Dropped{get; set;}

		public string Prefix{get; set;}
		public string Namespace{get; set;}

		public void AdjustAttribute(AttributeInfo attr){
			this.Name = attr.LocalName;
			this.Prefix = attr.Prefix;
			this.Namespace = attr.Namespace;
		}

	}
}

