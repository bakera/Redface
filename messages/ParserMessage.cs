using System;
using System.Xml;

namespace Bakera.RedFace{

	public abstract class ParserMessage{

		public string Message{
			get{
				return string.Format(MessageTemplate, Params);
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



