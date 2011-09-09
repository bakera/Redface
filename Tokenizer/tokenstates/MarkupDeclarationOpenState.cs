using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public const string DoctypeString = "DOCTYPE";
		public class MarkupDeclarationOpenState : TokenState{

			public MarkupDeclarationOpenState(Tokenizer t) : base(t){}

			public override Token Read(){
				ConsumeChar();

				if(CurrentInputChar == Chars.HYPHEN_MINUS){
					// ToDo: Comment
				} else if(IsDoctypeMatch()){
					ChangeTokenState(typeof(DoctypeState));
				} else {
					Parser.Stop();
				}
				return null;
			}

			// 文字が doctype であるかどうかを調べます。
			// マッチすればそのまま true を返し、マッチしなければUnConsumeしてfalseを返します。
			// UnConsumeした場合の CurrentInputChar は 'D' に相当する文字になります。
			private bool IsDoctypeMatch(){
				if(CurrentInputChar != 'D' && CurrentInputChar != 'd') return false;

				string probablyDoctypeString = CurrentInputChar.ToString();
				string nextString = myTokenizer.ConsumeChar(DoctypeString.Length - 1);
				probablyDoctypeString += nextString;
				if(probablyDoctypeString.Equals(DoctypeString, StringComparison.InvariantCultureIgnoreCase)){
					return true;
				}
				UnConsume(DoctypeString.Length - 1);
				return false;
			}

		}

	}
}
