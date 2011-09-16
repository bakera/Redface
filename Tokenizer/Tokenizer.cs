using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Bakera.RedFace{

	public partial class RedFaceParser{

		public partial class Tokenizer{

			private StateManager<TokenizationState> myTokenStateManager = null;
			private RedFaceParser myParser = null;
			private Token myCurrentToken;
			private Token myEmittedToken;
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
					return myTokenStateManager.CurrentState;
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
				myTokenStateManager = new StateManager<TokenizationState>(p);
				myTokenStateManager.SetState<DataState>();
			}


	// トークンの取得
			public Token GetToken(){
				while(!Parser.IsStopped){
					CurrentTokenState.Read(this);
					if(myEmittedToken != null){
						Token result = myEmittedToken;
						myEmittedToken = null;
						return result;
					}
				}
				return null;
			}


			// TokenをEmitします。
			public void EmitToken(Token t){
				myEmittedToken = t;
				myCurrentToken = null;
			}
			// CurrentTokenをEmitします。
			public void EmitToken(){
				EmitToken(myCurrentToken);
			}

			// トークン走査状態を変更します。
			public void ChangeTokenState<T>() where T : TokenizationState, new(){
				if(CurrentTokenState != null && typeof(T) == CurrentTokenState.GetType()) return;
				myTokenStateManager.SetState<T>();
				Parser.OnTokenStateChanged();
			}

	// 文字の読み取り
			// 一つ読み進みます。
			public char? ConsumeChar(){
				myInputStream.ConsumeNextInputChar();
				return CurrentInputChar;
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



