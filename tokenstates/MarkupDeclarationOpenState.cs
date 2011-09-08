using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public const string DoctypeString = "DOCTYPE";
		public class MarkupDeclarationOpenState : TokenState{

			public MarkupDeclarationOpenState(RedFaceParser p) : base(p){}

			public override void Read(){
				Parser.ConsumeChar();

				if(Parser.CurrentInputChar == Chars.HYPHEN_MINUS){
					// ToDo: Comment
				} else if(IsDoctypeMatch()){
					Parser.ChangeTokenState(typeof(DoctypeState));
				} else {
					Parser.Stop();
				}
			}

			// 文字が doctype であるかどうかを調べます。
			// マッチすればそのまま true を返し、マッチしなければUnConsumeしてfalseを返します。
			// UnConsumeした場合の CurrentInputChar は 'D' に相当する文字になります。
			private bool IsDoctypeMatch(){
				if(Parser.CurrentInputChar != 'D' && Parser.CurrentInputChar != 'd') return false;

				string probablyDoctypeString = Parser.CurrentInputChar.ToString();
				string nextString = Parser.ConsumeChar(DoctypeString.Length - 1);
				probablyDoctypeString += nextString;
				if(probablyDoctypeString.Equals(DoctypeString, StringComparison.InvariantCultureIgnoreCase)){
					return true;
				}
				Parser.UnConsume(DoctypeString.Length - 1);
				return false;
			}

		}

	}
}
