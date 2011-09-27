using System;
using System.Collections.Generic;

namespace Bakera.RedFace{


/*
 Start and end tag tokens have a tag name, a self-closing flag, and a list of attributes, each of which has a name and a value.
 When a start or end tag token is created, its self-closing flag must be unset (its other state is that it be set), and its attributes list must be empty.
*/

	public abstract class TagToken : Token{

		public bool SelfClosing{get; set;}
		private Dictionary<string, AttributeToken> myAttributes = new Dictionary<string, AttributeToken>();
		private List<AttributeToken> myDroppedAttributes = new List<AttributeToken>();
		private AttributeToken myCurrentAttribute;

		public AttributeToken CurrentAttribute{
			get{return myCurrentAttribute;}
			set{myCurrentAttribute = value;}
		}

		public AttributeToken[] Attributes{
			get{
				AttributeToken[] result = new AttributeToken[myAttributes.Values.Count];
				myAttributes.Values.CopyTo(result, 0);
				return result;
			}
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

		// CurrentAttributeがDroppedとしてチェックされているか確認します。
		// DroppedとしてチェックされたAttributeはFix時にDroppedAttributeに追加されます。
		public bool IsDroppedAttribute{
			get{
				if(myCurrentAttribute == null) return false;
				return myCurrentAttribute.Dropped;
			}
		}

		// CurrentAttributeの名前が既存の属性と重複しているかどうかチェックします。
		// 重複していればtrueを返します。
		public bool IsDuplicateAttribute{
			get{
				if(myCurrentAttribute == null) return false;
				string attrName = myCurrentAttribute.Name;
				if(myAttributes.ContainsKey(attrName)) return true;
				return false;
			}
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
			if(IsDuplicateAttribute){
				DropAttribute();
				myDroppedAttributes.Add(myCurrentAttribute);
				myCurrentAttribute = null;
				return false;
			}
			myAttributes.Add(myCurrentAttribute.Name, myCurrentAttribute);
			myCurrentAttribute = null;
			return true;
		}


		// CurrentAttributeをDroppedとしてチェックします。
		// DroppedとしてチェックされたAttributeはFix時にDroppedAttributeに追加されます。
		public void DropAttribute(){
			if(myCurrentAttribute == null) return;
			myCurrentAttribute.Dropped = true;
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
