using System;
using System.Collections.Generic;

namespace Bakera.RedFace{


/*
 Start and end tag tokens have a tag name, a self-closing flag, and a list of attributes, each of which has a name and a value.
 When a start or end tag token is created, its self-closing flag must be unset (its other state is that it be set), and its attributes list must be empty.
*/

	public abstract class TagToken : Token{

		private Dictionary<string, AttributeToken> myAttributes = new Dictionary<string, AttributeToken>();
		private List<AttributeToken> myDroppedAttributes = new List<AttributeToken>();
		private AttributeToken myCurrentAttribute;

		public AttributeToken CurrentAttribute{
			get{return myCurrentAttribute;}
			set{myCurrentAttribute = value;}
		}

		public override AttributeToken[] Attributes{
			get{
				AttributeToken[] result = new AttributeToken[myAttributes.Values.Count];
				myAttributes.Values.CopyTo(result, 0);
				return result;
			}
		}

		public override string GetAttributeValue(string name){
			if(!myAttributes.ContainsKey(name)) return null;
			return myAttributes[name].Value;
		}


		public TagToken(){
			Name = null;
			SelfClosing = false;
		}

		// 指定された名前の属性があればtrueを返します。
		public override bool HasAttribute(string key){
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

// Adjust Name
		public void AdjustSVGElementName(){
			if(mySVGElementNameReferences.ContainsKey(this.Name)) this.Name = mySVGElementNameReferences[this.Name];
		}

		public void AdjustSVGAttributes(){
			AdjustAttributes(mySVGAttributeNameReferences);
		}

		public void AdjustMathMLAttributes(){
			AdjustAttributes(myMathMLAttributeNameReferences);
		}

		public void AdjustForeignAttributes(){
			AdjustAttributes(myForeignAttributeNameReferences);
		}

		private void AdjustAttributes(Dictionary<string, AttributeInfo> dic){
			foreach(AttributeToken attr in myAttributes.Values){
				if(!dic.ContainsKey(attr.Name)) continue;
				attr.AdjustAttribute(dic[attr.Name]);
			}
		}

		private static readonly Dictionary<string, AttributeInfo> myMathMLAttributeNameReferences = new Dictionary<string, AttributeInfo>(){
			{"definitionurl", new AttributeInfo("definitionURL")},
		};

		private static readonly Dictionary<string, AttributeInfo> mySVGAttributeNameReferences = new Dictionary<string, AttributeInfo>(){
			{"attributename", new AttributeInfo("attributeName")},
			{"attributetype", new AttributeInfo("attributeType")},
			{"basefrequency", new AttributeInfo("baseFrequency")},
			{"baseprofile", new AttributeInfo("baseProfile")},
			{"calcmode", new AttributeInfo("calcMode")},
			{"clippathunits", new AttributeInfo("clipPathUnits")},
			{"contentscripttype", new AttributeInfo("contentScriptType")},
			{"contentstyletype", new AttributeInfo("contentStyleType")},
			{"diffuseconstant", new AttributeInfo("diffuseConstant")},
			{"edgemode", new AttributeInfo("edgeMode")},
			{"externalresourcesrequired", new AttributeInfo("externalResourcesRequired")},
			{"filterres", new AttributeInfo("filterRes")},
			{"filterunits", new AttributeInfo("filterUnits")},
			{"glyphref", new AttributeInfo("glyphRef")},
			{"gradienttransform", new AttributeInfo("gradientTransform")},
			{"gradientunits", new AttributeInfo("gradientUnits")},
			{"kernelmatrix", new AttributeInfo("kernelMatrix")},
			{"kernelunitlength", new AttributeInfo("kernelUnitLength")},
			{"keypoints", new AttributeInfo("keyPoints")},
			{"keysplines", new AttributeInfo("keySplines")},
			{"keytimes", new AttributeInfo("keyTimes")},
			{"lengthadjust", new AttributeInfo("lengthAdjust")},
			{"limitingconeangle", new AttributeInfo("limitingConeAngle")},
			{"markerheight", new AttributeInfo("markerHeight")},
			{"markerunits", new AttributeInfo("markerUnits")},
			{"markerwidth", new AttributeInfo("markerWidth")},
			{"maskcontentunits", new AttributeInfo("maskContentUnits")},
			{"maskunits", new AttributeInfo("maskUnits")},
			{"numoctaves", new AttributeInfo("numOctaves")},
			{"pathlength", new AttributeInfo("pathLength")},
			{"patterncontentunits", new AttributeInfo("patternContentUnits")},
			{"patterntransform", new AttributeInfo("patternTransform")},
			{"patternunits", new AttributeInfo("patternUnits")},
			{"pointsatx", new AttributeInfo("pointsAtX")},
			{"pointsaty", new AttributeInfo("pointsAtY")},
			{"pointsatz", new AttributeInfo("pointsAtZ")},
			{"preservealpha", new AttributeInfo("preserveAlpha")},
			{"preserveaspectratio", new AttributeInfo("preserveAspectRatio")},
			{"primitiveunits", new AttributeInfo("primitiveUnits")},
			{"refx", new AttributeInfo("refX")},
			{"refy", new AttributeInfo("refY")},
			{"repeatcount", new AttributeInfo("repeatCount")},
			{"repeatdur", new AttributeInfo("repeatDur")},
			{"requiredextensions", new AttributeInfo("requiredExtensions")},
			{"requiredfeatures", new AttributeInfo("requiredFeatures")},
			{"specularconstant", new AttributeInfo("specularConstant")},
			{"specularexponent", new AttributeInfo("specularExponent")},
			{"spreadmethod", new AttributeInfo("spreadMethod")},
			{"startoffset", new AttributeInfo("startOffset")},
			{"stddeviation", new AttributeInfo("stdDeviation")},
			{"stitchtiles", new AttributeInfo("stitchTiles")},
			{"surfacescale", new AttributeInfo("surfaceScale")},
			{"systemlanguage", new AttributeInfo("systemLanguage")},
			{"tablevalues", new AttributeInfo("tableValues")},
			{"targetx", new AttributeInfo("targetX")},
			{"targety", new AttributeInfo("targetY")},
			{"textlength", new AttributeInfo("textLength")},
			{"viewbox", new AttributeInfo("viewBox")},
			{"viewtarget", new AttributeInfo("viewTarget")},
			{"xchannelselector", new AttributeInfo("xChannelSelector")},
			{"ychannelselector", new AttributeInfo("yChannelSelector")},
			{"zoomandpan", new AttributeInfo("zoomAndPan")},
		};

		private static readonly Dictionary<string, AttributeInfo> myForeignAttributeNameReferences = new Dictionary<string, AttributeInfo>(){
			{"xlink:actuate", new AttributeInfo("xlink", "actuate", Document.XLinkNamespace)},
			{"xlink:arcrole", new AttributeInfo("xlink", "arcrole", Document.XLinkNamespace)},
			{"xlink:href", new AttributeInfo("xlink", "href", Document.XLinkNamespace)},
			{"xlink:role", new AttributeInfo("xlink", "role", Document.XLinkNamespace)},
			{"xlink:show", new AttributeInfo("xlink", "show", Document.XLinkNamespace)},
			{"xlink:title", new AttributeInfo("xlink", "title", Document.XLinkNamespace)},
			{"xlink:type", new AttributeInfo("xlink", "type", Document.XLinkNamespace)},
			{"xml:base", new AttributeInfo("xml", "base", Document.XmlNamespace)},
			{"xml:lang", new AttributeInfo("xml", "lang", Document.XmlNamespace)},
			{"xml:space", new AttributeInfo("xml", "space", Document.XmlNamespace)},
			{"xmlns", new AttributeInfo(null, "xmlns", Document.XmlnsNamespace)},
			{"xmlns:xlink", new AttributeInfo("xmlns", "xlink", Document.XmlnsNamespace)},
		};

		private static readonly Dictionary<string, string> mySVGElementNameReferences = new Dictionary<string, string>(){
			{"altglyph", "altGlyph"},
			{"altglyphdef", "altGlyphDef"},
			{"altglyphitem", "altGlyphItem"},
			{"animatecolor", "animateColor"},
			{"animatemotion", "animateMotion"},
			{"animatetransform", "animateTransform"},
			{"clippath", "clipPath"},
			{"feblend", "feBlend"},
			{"fecolormatrix", "feColorMatrix"},
			{"fecomponenttransfer", "feComponentTransfer"},
			{"fecomposite", "feComposite"},
			{"feconvolvematrix", "feConvolveMatrix"},
			{"fediffuselighting", "feDiffuseLighting"},
			{"fedisplacementmap", "feDisplacementMap"},
			{"fedistantlight", "feDistantLight"},
			{"feflood", "feFlood"},
			{"fefunca", "feFuncA"},
			{"fefuncb", "feFuncB"},
			{"fefuncg", "feFuncG"},
			{"fefuncr", "feFuncR"},
			{"fegaussianblur", "feGaussianBlur"},
			{"feimage", "feImage"},
			{"femerge", "feMerge"},
			{"femergenode", "feMergeNode"},
			{"femorphology", "feMorphology"},
			{"feoffset", "feOffset"},
			{"fepointlight", "fePointLight"},
			{"fespecularlighting", "feSpecularLighting"},
			{"fespotlight", "feSpotLight"},
			{"fetile", "feTile"},
			{"feturbulence", "feTurbulence"},
			{"foreignobject", "foreignObject"},
			{"glyphref", "glyphRef"},
			{"lineargradient", "linearGradient"},
			{"radialgradient", "radialGradient"},
			{"textpath", "textPath"},
		};


	}
}
