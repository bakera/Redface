using System;
using System.IO;

namespace Bakera.RedFaceLint{

	public class DataState : TokenizationState{

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

		public DataState(Html5Parser p) : base(p){}

		public override void Read(){

			
			
			if(Parser.IsEOF) return;
			char c = ReadChar();
			Console.Write(c);
		}
	}

}
