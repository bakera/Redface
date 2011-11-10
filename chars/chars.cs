using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Bakera.RedFace{

	public static partial class Chars{
		public const char NULL                  = '\u0000';
		public const char CHARACTER_TABULATION  = '\u0009';
		public const char LINE_FEED             = '\u000a';
		public const char FORM_FEED             = '\u000c';
		public const char CARRIAGE_RETURN       = '\u000d';
		public const char EXCLAMATION_MARK      = '\u0021';
		public const char SPACE                 = '\u0020';
		public const char QUOTATION_MARK        = '\u0022';
		public const char NUMBER_SIGN           = '\u0023';
		public const char AMPERSAND             = '\u0026';
		public const char APOSTROPHE            = '\u0027';
		public const char HYPHEN_MINUS          = '\u002d';
		public const char SOLIDUS               = '\u002f';
		public const char SEMICOLON             = '\u003b';
		public const char LESS_THAN_SIGN        = '\u003c';
		public const char EQUALS_SIGN           = '\u003d';
		public const char GREATER_THAN_SIGN     = '\u003e';
		public const char QUESTION_MARK         = '\u003f';
		public const char RIGHT_SQUARE_BRACKET  = '\u005d';
		public const char GRAVE_ACCENT          = '\u0060';
		public const char BOM                   = '\ufeff';
		public const char REPLACEMENT_CHARACTER = '\ufffd';

		public const System.Globalization.NumberStyles HexParseStyle = System.Globalization.NumberStyles.AllowHexSpecifier;
		public const System.Globalization.NumberStyles DecimalParseStyle = System.Globalization.NumberStyles.None;

		public static bool IsLatinCapitalLetter(this char? c){
			if(c == null) return false;
			return '\u0041' <= c && c <= '\u0058';
		}
		public static bool IsLatinSmallLetter(this char? c){
			if(c == null) return false;
			return '\u0061' <= c && c <= '\u007a';
		}
		public static bool IsDigit(this char? c){
			if(c == null) return false;
			return '\u0030' <= c && c <= '\u0039';
		}
		public static bool IsHexDigit(this char? c){
			if(c == null) return false;
			return IsDigit(c) || ('\u0041' <= c && c <= '\u0046') || ('\u0061' <= c && c <= '\u0066');
		}
		public static bool IsNameToken(this char? c){
			return IsLatinSmallLetter(c) || IsLatinCapitalLetter(c) || IsDigit(c);
		}

		public static char? ToLower(this char? c){
			if(c == null) return null;
			return (char)(c + 0x20);
		}

		public static bool IsSpaceCharacter(this char c){
			switch(c){
				case Chars.CHARACTER_TABULATION:
				case Chars.LINE_FEED:
				case Chars.FORM_FEED:
				case Chars.CARRIAGE_RETURN:
				case Chars.SPACE:
					return true;
				default:
					return false;
			}
		}

/*
If the character reference is being consumed as part of an attribute, and the last character matched is not a U+003B SEMICOLON character (;), and the next character is either a U+003D EQUALS SIGN character (=) or in the range U+0030 DIGIT ZERO (0) to U+0039 DIGIT NINE (9), U+0041 LATIN CAPITAL LETTER A to U+005A LATIN CAPITAL LETTER Z, or U+0061 LATIN SMALL LETTER A to U+007A LATIN SMALL LETTER Z, then, for historical reasons, all the characters that were matched after the U+0026 AMPERSAND character (&) must be unconsumed, and nothing is returned.
*/
		public static bool IsSuffixOfIgnoreCharacterReferenceInAttribute(this char? c){
			if(c == null) return false;
			if(c == EQUALS_SIGN) return true;
			if('\u0030' <= c && c <= '\u0039') return true;
			if('\u0041' <= c && c <= '\u005a') return true;
			if('\u0061' <= c && c <= '\u007a') return true;
			return false;
		}
	}
}



