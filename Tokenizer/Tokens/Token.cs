using System;

namespace Bakera.RedFace{

/*
The output of the tokenization step is a series of zero or more of the following tokens: DOCTYPE, start tag, end tag, comment, character, end-of-file.
*/

	public abstract class Token{

		public virtual string Name{get; set;}

		// 属性値を取得します。TagTokenの場合を除いて常にnullを返します。
		public virtual string GetAttributeValue(string name){
			return null;
		}

		public virtual AttributeToken[] Attributes{
			get{return new AttributeToken[0];}
		}


		// CharacterTokenでかつ文字が空白類文字の場合true
		public virtual bool IsWhiteSpace{
			get{ return false; }
		}

		// CharacterTokenでかつLFの場合true
		public virtual bool IsLineFeed{
			get{ return false; }
		}

		// CharacterTokenでかつNUL文字の場合true
		public virtual bool IsNULL{
			get{ return false; }
		}

		// StartTagTokenでかつNameが指定文字列の場合true
		public virtual bool IsStartTag(string name){
			return false;
		}
		// StartTagTokenでかつNameが指定文字列の場合true
		public virtual bool IsStartTag(params string[] names){
			return false;
		}

		// EndTagTokenでかつNameが指定文字列の場合true
		public virtual bool IsEndTag(string name){
			return false;
		}
		// EndTagTokenでかつNameが指定文字列の場合true
		public virtual bool IsEndTag(params string[] names){
			return false;
		}

	}

}
