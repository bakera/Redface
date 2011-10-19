using System;
using System.Collections.Generic;
using System.Xml;

namespace Bakera.RedFace{

	public static class AttributeNames{

		public static void AdjustSVGAttributes(XmlElement e){
			AdjustAttributes(e, mySVGAttributeNameReferences);
		}

		public static void AdjustMathMLAttributes(XmlElement e){
			AdjustAttributes(e, myMathMLAttributeNameReferences);
		}

		public static void AdjustForeignAttributes(XmlElement e){
			AdjustAttributes(e, mySVGAttributeNameReferences);
		}

		public static void AdjustAttributes(XmlElement e, Dictionary<string, AttributeInfo> dic){
			foreach(XmlAttribute attr in e.Attributes){
				if(!dic.ContainsKey(attr.Name)) continue;
				AttributeInfo newAttrInfo = dic[attr.Name];
				XmlAttribute newAttr = e.OwnerDocument.CreateAttribute(newAttrInfo.Prefix, newAttrInfo.LocalName, newAttrInfo.Namespace);
				newAttr.Value = attr.Value;
				e.RemoveAttributeNode(attr);
				e.AppendChild(newAttr);
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
	}
}
