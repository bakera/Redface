using System;
using System.IO;

namespace Bakera.RedFaceLint{

	public class DataState : TokenState{

/*
8.2.4.1 Data state
Consume the next input character:
↪U+0026 AMPERSAND (&)
	Switch to the character reference in data state.
↪U+003C LESS-THAN SIGN (<)
	Switch to the tag open state.
↪U+0000 NULL
	Parse error. Emit the current input character as a character token.
↪EOF
	Emit an end-of-file token.
↪Anything else
	Emit the current input character as a character token.
*/

		public DataState(RedFaceParser p) : base(p){}

		public override void Read(){
			Parser.ConsumeChar();
			switch(Parser.NextInputChar){
				case Chars.AMPERSAND:
					Parser.AddError("&");
					break;
				case Chars.LESS_THAN_SIGN:
					Parser.AddError("LT");
					break;
				case Chars.NULL:
					Parser.AddError("NULL文字が含まれています。");
					break;
				case null:
					Console.WriteLine("end");
					break;
				default:
					break;
			}
		}
	}

}
