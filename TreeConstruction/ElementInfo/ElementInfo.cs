using System;
using System.Xml;

namespace Bakera.RedFace{

	public abstract class ElementInfo{

		public virtual string Name{get; protected set;}
		public abstract string Namespace{get;}


		public virtual bool IsMatch(XmlElement e){
			return e.Name.Equals(this.Name, StringComparison.InvariantCulture) && e.NamespaceURI.Equals(this.Namespace, StringComparison.InvariantCulture);
		}
	}

}



