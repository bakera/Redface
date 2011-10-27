using System;

namespace Bakera.RedFace{
	public class StartTagToken : TagToken{

		// StartTagTokenでかつNameが指定文字列の場合true
		public override bool IsStartTag(string name){
			return this.Name.Equals(name, StringComparison.InvariantCulture);
		}

		// EndTagTokenでかつNameが指定文字列の場合true
		public override bool IsStartTag(params string[] names){
			foreach(string s in names){
				if(this.Name.Equals(s, StringComparison.InvariantCulture)) return true;
			}
			return false;
		}

		// type 属性が hidden の場合に true を返します。
		public bool IsHiddenType(){
			string typeValue = this.GetAttributeValue("type");
			if(typeValue == null) return false;
			if(typeValue.Equals("hidden", StringComparison.InvariantCultureIgnoreCase)) return true;
			return false;
		}

	}

}
