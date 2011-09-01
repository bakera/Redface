using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public abstract class TokenState{
			
			protected RedFaceParser Parser{get; set;}
			protected static readonly Regex DigitRange = new Regex("[0-9A-Za-z]");
			protected static readonly Regex HexDigitRange = new Regex("[0-9]");


	// プロパティ


	// コンストラクタ
			public TokenState(RedFaceParser p){
				Parser = p;
			}

	// 抽象メソッド
			public virtual void Read(){
			}

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
			protected char ReadChar(){
				char c = (char)Parser.Reader.Read();
				return c;
			}

			// その場で次の1文字を読みます。
			protected char PeekChar(){
				char c = (char)Parser.Reader.Peek();
				return c;
			}

			// 参照されている文字を取得します。失敗したときはnullを返します。
			protected string ConsumeCharacterReference(){
				return ConsumeCharacterReference(null);
			}
			protected string ConsumeCharacterReference(char? additional_allowed_character){
				if(additional_allowed_character != null && Parser.NextInputChar == additional_allowed_character){
					return null;
				}
				switch(Parser.NextInputChar){
					case Chars.AMPERSAND:
					case Chars.LINE_FEED:
					case Chars.FORM_FEED:
					case Chars.SPACE:
					case Chars.LESS_THAN_SIGN:
					case null:
						// Not a character reference. No characters are consumed, and nothing is returned. (This is not an error, either.)
						return null;
					case Chars.NUMBER_SIGN:
						return ConsumeNumericCharacterReference();
					default:
						break;
				}
				return null;
			}


			protected string ConsumeNumericCharacterReference(){
				Parser.ConsumeChar();
				Regex numericPattern = null;
				if(Parser.NextInputChar == 'x' || Parser.NextInputChar == 'X'){
					numericPattern = HexDigitRange;
					Parser.ConsumeChar();
				} else {
					numericPattern = DigitRange;
				}
				StringBuilder matchResult = new StringBuilder();
				while(numericPattern.IsMatch(Parser.NextInputChar.ToString())){
					matchResult.Append(Parser.NextInputChar);
					Parser.ConsumeChar();
				}
				if(matchResult.Length == 0){
					Parser.OnParseErrorRaised("数値文字参照の数値が空です。");
					return null;
				}
				return null;
			}

		}
	}
}
