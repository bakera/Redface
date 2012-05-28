using System;
using System.Xml;

namespace Bakera.RedFace{

	public abstract class ParserMessage{

		public string Message{
			get{
				string format = this.MessageTemplate;
				object[] paramObjects = this.Params;
				if(paramObjects == null || paramObjects.Length == 0){
					return format;
				}
				return string.Format(format, paramObjects);
			}
		}

		public abstract string MessageTemplate{
			get;
		}

		public abstract EventLevel Level{
			get;
		}

		public object[] Params{
			get;
			protected set;
		}

	}

}



