using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public const string DoctypeString = "DOCTYPE";
		public class MarkupDeclarationOpenState : TokenizationState{

			public override Token Read(Tokenizer t){
				char? c = t.ConsumeChar();
				if(c == Chars.HYPHEN_MINUS){
					// ToDo: Comment
				} else if(IsDoctypeMatch(t)){
					t.ChangeTokenState<DoctypeState>();
				} else {
					t.Parser.Stop();
				}
				return null;
			}

			// 文字が doctype であるかどうかを調べます。
			// マッチすればそのまま true を返し、マッチしなければUnConsumeしてfalseを返します。
			// UnConsumeした場合の CurrentInputChar は 'D' に相当する文字になります。
			private bool IsDoctypeMatch(Tokenizer t){
				char? c = t.CurrentInputChar;
				if(c != 'D' && c != 'd') return false;

				string probablyDoctypeString = c.ToString();
				string nextString = t.ConsumeChar(DoctypeString.Length - 1);
				probablyDoctypeString += nextString;
				if(probablyDoctypeString.Equals(DoctypeString, StringComparison.InvariantCultureIgnoreCase)){
					return true;
				}
				t.UnConsume(DoctypeString.Length - 1);
				return false;
			}

		}

	}
}
