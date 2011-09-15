using System;
using System.Collections.Generic;
using System.Text;

namespace Bakera.RedFace{

	public static partial class Chars{

		public static string GetReplacedCharByNumber(this int num){
			return myReplacedChars[num];
		}

		public static bool IsReplacedChar(this int num){
			return myReplacedChars.ContainsKey(num);
		}

		public static bool IsSurrogate(this int num){
			return 0xD800 <= num && num >= 0xDFFF;
		}

		public static bool IsErrorChar(this int num){
			if(0x0001 <= num && num <= 0x0008) return true;
			if(0x000E <= num && num <= 0x001F) return true;
			if(0x007F <= num && num <= 0x009F) return true;
			if(0xFDD0 <= num && num <= 0xFDEF) return true;
			if(Array.IndexOf(myErrorChars, num) >= 0) return true;
			return false;
		}


		public static string GetCharByNumber(this int num){
			byte[] bytes = BitConverter.GetBytes(num);
			return Encoding.UTF32.GetString(bytes);
		}

		private static readonly int[] myErrorChars = new int[]{
			0x000B,
			0xFFFE,
			0xFFFF,
			0x1FFFE,
			0x1FFFF,
			0x2FFFE,
			0x2FFFF,
			0x3FFFE,
			0x3FFFF,
			0x4FFFE,
			0x4FFFF,
			0x5FFFE,
			0x5FFFF,
			0x6FFFE,
			0x6FFFF,
			0x7FFFE,
			0x7FFFF,
			0x8FFFE,
			0x8FFFF,
			0x9FFFE,
			0x9FFFF,
			0xAFFFE,
			0xAFFFF,
			0xBFFFE,
			0xBFFFF,
			0xCFFFE,
			0xCFFFF,
			0xDFFFE,
			0xDFFFF,
			0xEFFFE,
			0xEFFFF,
			0xFFFFE,
			0xFFFFF,
			0x10FFFE,
			0x10FFFF,
		};

		private static readonly Dictionary<int, string> myReplacedChars = new Dictionary<int, string>(){
			{0x00, "\uFFFD"},
			{0x0D, "\u000D"},
			{0x80, "\u20AC"},
			{0x81, "\u0081"},
			{0x82, "\u201A"},
			{0x83, "\u0192"},
			{0x84, "\u201E"},
			{0x85, "\u2026"},
			{0x86, "\u2020"},
			{0x87, "\u2021"},
			{0x88, "\u02C6"},
			{0x89, "\u2030"},
			{0x8A, "\u0160"},
			{0x8B, "\u2039"},
			{0x8C, "\u0152"},
			{0x8D, "\u008D"},
			{0x8E, "\u017D"},
			{0x8F, "\u008F"},
			{0x90, "\u0090"},
			{0x91, "\u2018"},
			{0x92, "\u2019"},
			{0x93, "\u201C"},
			{0x94, "\u201D"},
			{0x95, "\u2022"},
			{0x96, "\u2013"},
			{0x97, "\u2014"},
			{0x98, "\u02DC"},
			{0x99, "\u2122"},
			{0x9A, "\u0161"},
			{0x9B, "\u203A"},
			{0x9C, "\u0153"},
			{0x9D, "\u009D"},
			{0x9E, "\u017E"},
			{0x9F, "\u0178"},
		};
	}

}



