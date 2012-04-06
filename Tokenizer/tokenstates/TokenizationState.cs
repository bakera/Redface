using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Bakera.RedFace{

	public abstract class TokenizationState : RedFaceParserState{

		public const string DoctypeId = "DOCTYPE";
		public const string CDATASectionStartId = "[CDATA[";
		public const string CDATASectionEndId = "]]>";
		public const string CommentId = "--";
		public const string DoctypePublicId = "PUBLIC";
		public const string DoctypeSystemId = "SYSTEM";


// プロパティ
		public virtual string Name{
			get{return this.GetType().Name;}
		}


// 抽象メソッド
		public abstract void Read(Tokenizer t);


// メソッド

		// CurrentInputCharを含むこの先の文字が、渡された文字列にマッチするかどうか Case Insensitive で調べます。
		// マッチすればそのまま true を返し、マッチしなければUnConsumeしてfalseを返します。
		// UnConsumeした場合は CurrentInputChar は変更されません。
		protected bool IsStringMatch(Tokenizer t, string testString){
			return IsStringMatch(t, testString, StringComparison.InvariantCultureIgnoreCase);
		}

		protected bool IsStringMatchCaseSensitive(Tokenizer t, string testString){
			return IsStringMatch(t, testString, StringComparison.InvariantCulture);
		}

		protected bool IsStringMatch(Tokenizer t, string testString, StringComparison sc){
			char? c = t.CurrentInputChar;
			string inputString = c.ToString() + t.ConsumeChar(testString.Length - 1);
			if(inputString.Equals(testString, sc)){
				return true;
			}
			t.UnConsume(testString.Length - 1);
			return false;
		}


// 文字参照

		// 参照されている文字を取得します。失敗したときはnullを返します。
		protected ReferencedCharacterToken ConsumeCharacterReference(Tokenizer t){
			char? c = t.ConsumeChar();
			if(t.AdditionalAllowedCharacter != null && c == t.AdditionalAllowedCharacter){
				// Not a character reference. No characters are consumed, and nothing is returned. (This is not an error, either.)
				OnMessageRaised(new RawAmpersandWarning());
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
					OnMessageRaised(new RawAmpersandWarning());
					t.UnConsume(1);
					return null;
				case Chars.NUMBER_SIGN:
					return ConsumeNumericCharacterReference(t);
				default:
					return ConsumeNamedCharacterReference(t);
			}
		}

/*
If the character reference is being consumed as part of an attribute, and the last character matched is not a U+003B SEMICOLON character (;), and the next character is either a U+003D EQUALS SIGN character (=) or in the range U+0030 DIGIT ZERO (0) to U+0039 DIGIT NINE (9), U+0041 LATIN CAPITAL LETTER A to U+005A LATIN CAPITAL LETTER Z, or U+0061 LATIN SMALL LETTER A to U+007A LATIN SMALL LETTER Z, then, for historical reasons, all the characters that were matched after the U+0026 AMPERSAND character (&) must be unconsumed, and nothing is returned.
*/

		// 名前による文字参照を展開します。
		protected ReferencedCharacterToken ConsumeNamedCharacterReference(Tokenizer t){
			StringBuilder referenceName = new StringBuilder();
			bool semicolonFound = false;

			char? lastChar = t.CurrentInputChar;
			while(lastChar.IsNameToken()){
				referenceName.Append(lastChar);
				lastChar = t.ConsumeChar();
			}
			if(lastChar == Chars.SEMICOLON){
				referenceName.Append(lastChar);
				semicolonFound = true;
			} else {
				// CurrentInputCharをreferenceNameの末尾にそろえる
				t.UnConsume(1);
			}
			string originalString = referenceName.ToString();

			string matchResult = null;
			while(referenceName.Length > 0){
				if(Chars.ExistsNamedChar(referenceName.ToString())){
					matchResult = Chars.GetNamedChar(referenceName.ToString());
					break;
				}
				referenceName.Remove(referenceName.Length-1, 1);
				// CurrentInputCharをreferenceNameの末尾にそろえる
				t.UnConsume(1);
			}

			if(matchResult == null){
				if(semicolonFound){
					// 名前がなく、セミコロンがある場合はパースエラー
					// if the characters after the U+0026 AMPERSAND character (&) consist of a sequence of one or more characters in the range ASCII digits, lowercase ASCII letters, and uppercase ASCII letters, followed by a ";" (U+003B) character, then this is a parse error.
					OnMessageRaised(new UnknownNamedCharacterReferenceWithSemicolonError(originalString));
				} else {
					// 名前がなくセミコロンがない場合はパースエラーにならない
					OnMessageRaised(new UnknownNamedCharacterReferenceWithoutSemicolonWarning(originalString));
				}
				t.UnConsume(referenceName.Length);
				return null;
			}
			if(!semicolonFound){
				// 属性値の中のセミコロンなし文字参照は無視する
				if(t.CurrentTokenState is CharacterReferenceInAttributeState && t.NextInputChar.IsSuffixOfIgnoreCharacterReferenceInAttribute()){
					OnMessageRaised(new IgnoredCharacterReferenceInAttributeWarning(referenceName));
					t.UnConsume(referenceName.Length);
					return null;
				}
				// それ以外のセミコロンなし文字参照はエラー
				OnMessageRaised(new NamedCharacterReferenceWithoutSemicolonError(referenceName));
			}

			ReferencedCharacterToken resultToken = new ReferencedCharacterToken(matchResult);
			resultToken.OriginalString = referenceName.ToString();
			return resultToken;
		}


		// 数値文字参照を展開します。
		protected ReferencedCharacterToken ConsumeNumericCharacterReference(Tokenizer t){
			char? c = t.ConsumeChar();
			Predicate<char?> isNumeric = null;
			System.Globalization.NumberStyles parseStyle = Chars.DecimalParseStyle;
			string prefix = "";
			string suffix = "";
			if(c == 'x' || c == 'X'){
				prefix = c.ToString();
				isNumeric = Chars.IsHexDigit;
				parseStyle = Chars.HexParseStyle;
				c = t.ConsumeChar();
			} else {
				isNumeric = Chars.IsDigit;
			}
			StringBuilder matchResult = new StringBuilder();
			while(isNumeric(c)){
				matchResult.Append(c);
				c = t.ConsumeChar();
			}
			if(matchResult.Length == 0){
				OnMessageRaised(new EmptyNumericCharacterReferenceError());
				return null;
			}
			string numberString = matchResult.ToString();
			int resultNumber = int.Parse(numberString, parseStyle);
			string result = GetNumberedChar(t, resultNumber);
			if(c == Chars.SEMICOLON){
				suffix += Chars.SEMICOLON;
			} else {
				OnMessageRaised(new NumericCharacterReferenceWithoutSemicolonError());
				t.UnConsume(1);
			}
			string originalString = prefix + numberString + suffix;

			ReferencedCharacterToken resultToken = new ReferencedCharacterToken(result);
			resultToken.OriginalString = originalString;
			return resultToken;
		}


		private string GetNumberedChar(Tokenizer t, int num){
			if(num.IsReplacedChar()){
				string errorResult = Chars.GetReplacedCharByNumber(num);
				OnMessageRaised(new ReplacedNumericCharacterReferenceError(num, errorResult));
				return errorResult;
			}
			if(num.IsSurrogate()){
				string errorResult = Chars.REPLACEMENT_CHARACTER.ToString();
				OnMessageRaised(new SurrogateNumericCharacterReferenceError(num));
				return errorResult;
			}
			if(num > 0x10FFFF){
				string errorResult = Chars.REPLACEMENT_CHARACTER.ToString();
				OnMessageRaised(new TooLargeNumericCharacterReferenceError(num));
				return errorResult;
			}
			string result = Chars.GetCharByNumber(num);
			if(num.IsErrorChar()){
				OnMessageRaised(new NoncharactersNumericCharacterReferenceError(num));
			}
			return result;
		}



	}
}
