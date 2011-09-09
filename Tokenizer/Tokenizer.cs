using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public partial class Tokenizer{

			private KeyedByTypeCollection<TokenizationState> myTokenStateManager = new KeyedByTypeCollection<TokenizationState>();
			private RedFaceParser myParser = null;
			private Token myCurrentToken;
			private TokenizationState myCurrentTokenState;
			private InputStream myInputStream;

			public char? CurrentInputChar {
				get{
					return myInputStream.CurrentInputChar;
				}
			}

			public RedFaceParser Parser {
				get{
					return myParser;
				}
			}

			public TokenizationState CurrentTokenState {
				get{
					return myCurrentTokenState;
				}
			}

			public Token CurrentToken {
				get{return myCurrentToken;}
				set{myCurrentToken = value;}
			}


	// コンストラクタ

			public Tokenizer(RedFaceParser p, InputStream stream){
				myParser = p;
				myInputStream = stream;
				SetTokenState(typeof(DataState));
			}


	// トークンの取得
			public Token GetToken(){
				while(!Parser.IsStopped){
					Token t = CurrentTokenState.Read();
					if(t == null) continue;
					return t;
				}
				return null;
			}


			// トークン走査状態を変更します。
			public void ChangeTokenState(Type t){
				if(myCurrentTokenState != null && t == myCurrentTokenState.GetType()) return;
				SetTokenState(t);
				Parser.OnTokenStateChanged();
			}

			// トークン走査状態を設定します。
			public void SetTokenState(Type t){
				if(!myTokenStateManager.Contains(t)){
					myTokenStateManager.Add(TokenizationState.CreateTokenState(t, this));
				}
				myCurrentTokenState = myTokenStateManager[t];
			}


	// 文字の読み取り
			// 一つ読み進みます。
			public void ConsumeChar(){
				myInputStream.ConsumeNextInputChar();
			}

			// 指定された文字数の文字を読んで文字列を返します。
			public string ConsumeChar(int n){
				StringBuilder result = new StringBuilder();
				for(int i=0; i < n; i++){
					ConsumeChar();
					result.Append(CurrentInputChar);
				}
				return result.ToString();
			}

			// 読み取った文字を返してUnConsumeします。
			// 返された文字はキューに格納されて再利用されます。
			public void UnConsume(int offset){
				myInputStream.UnConsume(offset);
			}

			// CurrentInputCharをUnConsumeします。
			public void UnConsume(){
				myInputStream.UnConsume(1);
			}

		}

	}

}



