using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public abstract class TokenizationState : RedFaceParserState{


	// プロパティ
			public virtual string Name{
				get{return this.GetType().Name;}
			}


	// 抽象メソッド
			public abstract Token Read(Tokenizer t);


	// メソッド

			// 参照されている文字を取得します。失敗したときはnullを返します。
			protected string ConsumeCharacterReference(Tokenizer t){
				return ConsumeCharacterReference(t, null);
			}
			protected string ConsumeCharacterReference(Tokenizer t, char? additional_allowed_character){
				char? c = t.ConsumeChar();
				
				if(additional_allowed_character != null && c == additional_allowed_character){
					t.UnConsume(1);
					return null;
				}
				switch(c){
					case Chars.AMPERSAND:
					case Chars.LINE_FEED:
					case Chars.FORM_FEED:
					case Chars.SPACE:
					case Chars.LESS_THAN_SIGN:
					case null:
						// Not a character reference. No characters are consumed, and nothing is returned. (This is not an error, either.)
						t.UnConsume(1);
						return null;
					case Chars.NUMBER_SIGN:
						return ConsumeNumericCharacterReference(t);
					default:
						return ConsumeNamedCharacterReference(t);
				}
			}


			// 名前による文字参照を展開します。
			protected string ConsumeNamedCharacterReference(Tokenizer t){
				Regex namePattern = Chars.NameToken;
				StringBuilder matchResult = new StringBuilder();
				bool semicolonFound = false;

				char? c = t.CurrentInputChar;
				while(namePattern.IsMatch(c.ToString())){
					matchResult.Append(c);
					c = t.ConsumeChar();
				}
				if(c == Chars.SEMICOLON){
					matchResult.Append(c);
					semicolonFound = true;
				}
				string originalString = matchResult.ToString();
				string result = null;

				while(matchResult.Length > 0){
					if(Chars.ExistsNamedChar(matchResult.ToString())){
						result = Chars.GetNamedChar(matchResult.ToString());
						break;
					}
					matchResult.Remove(matchResult.Length-1, 1);
				}
				if(result == null){
					t.Parser.OnParseErrorRaised(string.Format("文字参照 {0} を参照しようとしましたが、みつかりませんでした。", originalString));
					t.UnConsume(originalString.Length);
				}
				if(!semicolonFound){
					t.Parser.OnParseErrorRaised(string.Format("文字参照 &{0}; の末尾のセミコロンがありません。", matchResult));
					int diff = originalString.Length - matchResult.Length;
					t.UnConsume(diff+1);
				}
				t.Parser.OnCharacterReferenced(originalString, result);
				return result;
			}


			// 数値文字参照を展開します。
			protected string ConsumeNumericCharacterReference(Tokenizer t){
				char? c = t.ConsumeChar();
				Regex numericPattern = null;
				System.Globalization.NumberStyles parseStyle = Chars.DecimalParseStyle;
				string prefix = "";
				string suffix = "";
				if(c == 'x' || c == 'X'){
					prefix = c.ToString();
					numericPattern = Chars.HexDigitRange;
					parseStyle = Chars.HexParseStyle;
					c = t.ConsumeChar();
				} else {
					numericPattern = Chars.DigitRange;
				}

				StringBuilder matchResult = new StringBuilder();
				while(numericPattern.IsMatch(c.ToString())){
					matchResult.Append(c);
					c = t.ConsumeChar();
				}
				if(matchResult.Length == 0){
					t.Parser.OnParseErrorRaised("数値文字参照の数値が空です。");
					return null;
				}
				string numberString = matchResult.ToString();
				int resultNumber = int.Parse(numberString, parseStyle);
				string result = GetNumberedChar(t, resultNumber);
				if(c == Chars.SEMICOLON){
					suffix += Chars.SEMICOLON;
				} else {
					t.Parser.OnParseErrorRaised(string.Format("文字参照の末尾にセミコロンがありません。"));
					t.UnConsume(1);
				}
				string originalString = prefix + numberString + suffix;
				t.Parser.OnCharacterReferenced(originalString, result);
				return result;
			}


			private string GetNumberedChar(Tokenizer t, int num){
				if(Chars.IsReplacedChar(num)){
					string errorResult = Chars.GetReplacedCharByNumber(num);
					t.Parser.OnParseErrorRaised(string.Format("参照不可能な文字のコード {0} を参照しようとしました。文字は「{1}」に置換されます。", num, errorResult));
					return errorResult;
				}
				if(Chars.IsSurrogate(num)){
					string errorResult = Chars.REPLACEMENT_CHARACTER.ToString();
					t.Parser.OnParseErrorRaised(string.Format("サロゲート文字のコード {0} を参照しようとしました。文字は「{1}」に置換されます。", num, errorResult));
					return errorResult;
				}
				if(num > 0x10FFFF){
					string errorResult = Chars.REPLACEMENT_CHARACTER.ToString();
					t.Parser.OnParseErrorRaised(string.Format("指定された文字のコード {0} はUnicodeの範囲を超えています。文字は「{1}」に置換されます。", num, errorResult));
					return errorResult;
				}
				string result = Chars.GetCharByNumber(num);
				if(Chars.IsErrorChar(num)){
					t.Parser.OnParseErrorRaised(string.Format("指定された文字のコード {0} は非Unicode文字 (noncharacters) です。", num));
				}
				return result;
			}



		}
	}
}
