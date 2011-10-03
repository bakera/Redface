using System;
using System.IO;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public class EndTagOpenState : TokenizationState{


			public override void Read(Tokenizer t){
				char? c = t.ConsumeChar();
				switch(c){
					case Chars.GREATER_THAN_SIGN:
						t.Parser.OnParseErrorRaised(string.Format("空の終了タグを検出しました。"));
						t.ChangeTokenState<DataState>();
						return;
					case null:
						t.Parser.OnParseErrorRaised(string.Format("終了タグの解析中に終端に達しました。"));
						t.EmitToken(Chars.LESS_THAN_SIGN);
						t.EmitToken(Chars.SOLIDUS);
						t.UnConsume(1);
						t.ChangeTokenState<DataState>();
						return;
				}
				if(c.IsLatinCapitalLetter()){
					t.CurrentToken = new EndTagToken(){Name = c.ToLower().ToString()};
					t.ChangeTokenState<TagNameState>();
					return;
				} else if(c.IsLatinSmallLetter()){
					t.CurrentToken = new EndTagToken(){Name = c.ToString()};
					t.ChangeTokenState<TagNameState>();
					return;
 				}
				t.Parser.OnParseErrorRaised(string.Format("終了タグの中に不明な文字があります。"));
				t.ChangeTokenState<BogusCommentState>();
				return;
			}
		}
	}
}
