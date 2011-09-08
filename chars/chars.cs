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
		public const char BOM                   = '\ufeff';
		public const char REPLACEMENT_CHARACTER = '\ufffd';

		public static readonly Regex NameToken = new Regex("[0-9A-Za-z]");
		public static readonly Regex HexDigitRange = new Regex("[0-9A-Fa-f]");
		public static readonly Regex DigitRange = new Regex("[0-9]");

		public const System.Globalization.NumberStyles HexParseStyle = System.Globalization.NumberStyles.AllowHexSpecifier;
		public const System.Globalization.NumberStyles DecimalParseStyle = System.Globalization.NumberStyles.None;

	}
}



