using System;
using System.Collections.Generic;

namespace Bakera.RedFace{


/*
 Start and end tag tokens have a tag name, a self-closing flag, and a list of attributes, each of which has a name and a value.
 When a start or end tag token is created, its self-closing flag must be unset (its other state is that it be set), and its attributes list must be empty.
*/

	public abstract class TagToken : Token{

		public string Name{get; set;}
		public bool SelfClosing{get; set;}
		private Dictionary<string, AttributeToken> myAttributes = new Dictionary<string, AttributeToken>();
		private AttributeToken myCurrentAttribute;

		public AttributeToken CurrentAttribute{
			get{return myCurrentAttribute;}
			set{myCurrentAttribute = value;}
		}

		public TagToken(){
			Name = null;
			SelfClosing = false;
		}


		public bool HasAttribute(string key){
			return myAttributes.ContainsKey(key);
		}

		public AttributeToken CreateAttribute(){
			return CreateAttribute(null, null);
		}
		public AttributeToken CreateAttribute(char? c){
			return CreateAttribute(c, null);
		}

		// 属性を追加します
		// CurrentAttributeをnullにしてから呼ぶ必要があります。
		// このメソッドを呼ぶ前に FixAttribute を呼んで属性重複エラーがないかチェックしてください。
		public AttributeToken CreateAttribute(char? c, string s){
			if(myCurrentAttribute != null){
				throw new Exception("属性がfixされていません。");
			}
			
			myCurrentAttribute = new AttributeToken();
			if(c != null){
				myCurrentAttribute.Name = c.ToString();
				myCurrentAttribute.Value = s;
			}
			return CurrentAttribute;
		}


		// CurrentAttributeを確定してこのトークンに追加します。成功するとtrueを返して CurrentAttribute を null にします。
		// 既存の属性と名前がかぶっている場合は失敗し、falseを返します。このとき CurrentAttribute はそのまま残ります。
		// CurrentAttributeがnullのときに呼ぶと true を返します (いつでも呼んで良い)。
		public bool FixAttribute(){
			if(myCurrentAttribute == null) return true;

			string attrName = myCurrentAttribute.Name;
			if(myAttributes.ContainsKey(attrName)) return false;

			myAttributes.Add(attrName, myCurrentAttribute);
			myCurrentAttribute = null;
			return true;
		}

		public override string ToString(){
			string result = string.Format("{0} / Name: \"{1}\"", this.GetType().Name, this.Name);
			foreach(string key in myAttributes.Keys){
				AttributeToken attr = myAttributes[key];
				result += string.Format("\n Attribute: {0}", attr.Name);
				if(attr.Value != null) result += string.Format(" = \"{0}\"", attr.Value);
			}

			if(this.SelfClosing) result += "\n SelfClosing: true";
			return result;
		}


	}
}
