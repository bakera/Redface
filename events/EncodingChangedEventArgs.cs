using System;
using System.Text;

namespace Bakera.RedFace{

	public class EncodingChangedEventArgs : EventArgs{

		public EncodingChangedEventArgs(Encoding enc){
			this.Encoding = enc;
		}
		public Encoding Encoding{get; set;}
	}

}



