using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public abstract class TokenState{
			
			protected RedFaceParser Parser{get; set;}


	// プロパティ
			public virtual string Name{get{return this.GetType().Name;}}
			public char? CurrentInputChar{
				get{
					return Parser.CurrentInputChar;
				}
			}

	// コンストラクタ
			public TokenState(RedFaceParser p){
				Parser = p;
			}

	// 抽象メソッド
			public virtual void Read(){}

	// ファクトリ

			public static TokenState CreateTokenState(Type t, RedFaceParser parser){
				if(!(typeof(TokenState)).IsAssignableFrom(t)){
					throw new Exception("CreateTokenStateメソッドはTokenStateしか作成できません。渡された型 :" + t.ToString());
				}
				ConstructorInfo ci = t.GetConstructor(new Type[]{typeof(RedFaceParser)});
				TokenState result = ci.Invoke(new Object[]{parser}) as TokenState;
				return result;
			}


	// メソッド

			// 1文字読み進めます。
			protected char? ConsumeChar(){
				char? c = Parser.ConsumeChar();
				return c;
			}

			// 参照されている文字を取得します。失敗したときはnullを返します。
			protected string ConsumeCharacterReference(){
				return ConsumeCharacterReference(null);
			}
			protected string ConsumeCharacterReference(char? additional_allowed_character){
				char? nextCharacter = Parser.ConsumeChar();
				
				if(additional_allowed_character != null && nextCharacter == additional_allowed_character){
					Parser.UnConsume(nextCharacter);
					return null;
				}
				switch(nextCharacter){
					case Chars.AMPERSAND:
					case Chars.LINE_FEED:
					case Chars.FORM_FEED:
					case Chars.SPACE:
					case Chars.LESS_THAN_SIGN:
					case null:
						// Not a character reference. No characters are consumed, and nothing is returned. (This is not an error, either.)
						Parser.UnConsume(nextCharacter);
						return null;
					case Chars.NUMBER_SIGN:
						return ConsumeNumericCharacterReference();
					default:
						return ConsumeNamedCharacterReference();
				}
			}


			// 名前による文字参照を展開します。
			protected string ConsumeNamedCharacterReference(){
				Regex numericPattern = Chars.NameToken;
				StringBuilder matchResult = new StringBuilder();
				bool semicolonFound = false;
				while(numericPattern.IsMatch(Parser.CurrentInputChar.ToString())){
					matchResult.Append(Parser.CurrentInputChar);
					Parser.ConsumeChar();
				}
				if(Parser.CurrentInputChar == Chars.SEMICOLON){
					matchResult.Append(Parser.CurrentInputChar);
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
					Parser.OnParseErrorRaised(string.Format("文字参照 {0} を参照しようとしましたが、みつかりませんでした。", originalString));
					Parser.UnConsume(originalString);
				}
				if(!semicolonFound){
					Parser.OnParseErrorRaised(string.Format("文字参照 &{0}; の末尾にセミコロンがついていません。", matchResult));
					int diff = originalString.Length - matchResult.Length;
					if(diff > 0){
						Parser.UnConsume(originalString.Substring(matchResult.Length, diff) + Parser.CurrentInputChar);
					}
				}
				Parser.OnCharacterReferenced(originalString, result);
				return result;
			}


			// 数値文字参照を展開します。
			protected string ConsumeNumericCharacterReference(){
				Parser.ConsumeChar();
				Regex numericPattern = null;
				System.Globalization.NumberStyles parseStyle = Chars.DecimalParseStyle;
				string prefix = "";
				string suffix = "";
				if(Parser.CurrentInputChar == 'x' || Parser.CurrentInputChar == 'X'){
					prefix = Parser.CurrentInputChar.ToString();
					numericPattern = Chars.HexDigitRange;
					parseStyle = Chars.HexParseStyle;
					Parser.ConsumeChar();
				} else {
					numericPattern = Chars.DigitRange;
				}

				StringBuilder matchResult = new StringBuilder();
				while(numericPattern.IsMatch(Parser.CurrentInputChar.ToString())){
					matchResult.Append(Parser.CurrentInputChar);
					Parser.ConsumeChar();
				}
				if(matchResult.Length == 0){
					Parser.OnParseErrorRaised("数値文字参照の数値が空です。");
					return null;
				}
				string numberString = matchResult.ToString();
				int resultNumber = int.Parse(numberString, parseStyle);
				string result = GetNumberedChar(resultNumber);
				if(Parser.CurrentInputChar == Chars.SEMICOLON){
					suffix += Chars.SEMICOLON;
				} else {
					Parser.OnParseErrorRaised(string.Format("文字参照の末尾にセミコロンがありません。"));
					Parser.UnConsume(Parser.CurrentInputChar);
				}
				string originalString = prefix + numberString + suffix;
				Parser.OnCharacterReferenced(originalString, result);
				return result;
			}


			private string GetNumberedChar(int num){
				if(Chars.IsReplacedChar(num)){
					string errorResult = Chars.GetReplacedCharByNumber(num);
					Parser.OnParseErrorRaised(string.Format("参照不可能な文字のコード {0} を参照しようとしました。文字は「{1}」に置換されます。", num, errorResult));
					return errorResult;
				}
				if(Chars.IsSurrogate(num)){
					string errorResult = Chars.REPLACEMENT_CHARACTER.ToString();
					Parser.OnParseErrorRaised(string.Format("サロゲート文字のコード {0} を参照しようとしました。文字は「{1}」に置換されます。", num, errorResult));
					return errorResult;
				}
				if(num > 0x10FFFF){
					string errorResult = Chars.REPLACEMENT_CHARACTER.ToString();
					Parser.OnParseErrorRaised(string.Format("指定された文字のコード {0} はUnicodeの範囲を超えています。文字は「{1}」に置換されます。", num, errorResult));
					return errorResult;
				}
				string result = Chars.GetCharByNumber(num);
				if(Chars.IsErrorChar(num)){
					Parser.OnParseErrorRaised(string.Format("指定された文字のコード {0} はパースエラーとなりますが、文字はそのまま返します。", num));
				}
				return result;
			}



		}
	}
}
