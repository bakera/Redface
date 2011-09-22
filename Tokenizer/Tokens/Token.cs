using System;

namespace Bakera.RedFace{

/*
The output of the tokenization step is a series of zero or more of the following tokens: DOCTYPE, start tag, end tag, comment, character, end-of-file.
*/

	public abstract class Token{

		// CharacterTokenでかつ文字が空白類文字の場合true
		public virtual bool IsWhiteSpace{
			get{ return false; }
		}
	}

}
