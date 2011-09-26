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

	}

}
