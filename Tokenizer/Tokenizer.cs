using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Bakera.RedFace{

	public partial class Tokenizer{

		private StateManager<TokenizationState> myTokenStateManager = new StateManager<TokenizationState>();
		private RedFaceParser myParser = null;
		private Token myCurrentToken;
		private Queue<Token> myEmittedTokenQueue = new Queue<Token>();
		private InputStream myInputStream;

		public char? CurrentInputChar {
			get{
				return myInputStream.CurrentInputChar;
			}
		}
		public char? NextInputChar {
			get{
				return myInputStream.NextInputChar;
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
		public TokenizationState PreviousTokenState {
			get{
				return myTokenStateManager.PreviousState;
			}
		}

		public Token CurrentToken {
			get{return myCurrentToken;}
			set{myCurrentToken = value;}
		}

		public TagToken CurrentTagToken{get{return (TagToken)CurrentToken;}}
		public EndTagToken CurrentEndTagToken{get{return (EndTagToken)CurrentToken;}}
		public CommentToken CurrentCommentToken{get{return (CommentToken)CurrentToken;}}
		public DoctypeToken CurrentDoctypeToken{get{return (DoctypeToken)CurrentToken;}}

		public char? AdditionalAllowedCharacter{get; set;}
		public string TemporaryBuffer{get; set;}
		public StartTagToken LastStartTag{get; set;}

		// CurrentTokenがAppropriateEndTagTokenならtrueを返します。
		public bool IsAppropriateEndTagToken{
			get{
				if(!(CurrentToken is EndTagToken)) return false;
				return CurrentEndTagToken.Name.Equals(LastStartTag.Name, StringComparison.InvariantCulture);
			}
		}

		public InputStream InputStream {
			get{return myInputStream;}
		}


// コンストラクタ

		public Tokenizer(RedFaceParser p, InputStream stream){
			myParser = p;
			myInputStream = stream;
			myTokenStateManager.ParserEventRaised += OnParserEventRaised;
			myTokenStateManager.SetState<DataState>();
		}


// トークンの取得
		public Token GetToken(){
			while(!Parser.IsStopped){
				if(myEmittedTokenQueue.Count > 0){
					return myEmittedTokenQueue.Dequeue();
				}
				CurrentTokenState.Read(this);
			}
			return null;
		}

		// TokenをEmitします。
		public void EmitToken(Token t){
			myCurrentToken = null;
			// TagTokenがEmitされた場合、CurrentAtrtributeを処理する必要がある
			if(t is TagToken){
				TagToken tt = (TagToken)t;
				bool alreadyAttrErrorRaised = tt.IsDroppedAttribute;
				bool attrFixResult = tt.FixAttribute();
				if(!attrFixResult && !alreadyAttrErrorRaised){
					OnParseErrorRaised(string.Format("属性が重複しています。{0}", tt.Name));
				}
				if(t is StartTagToken) LastStartTag = (StartTagToken)t;
			}
			myEmittedTokenQueue.Enqueue(t);
		}

		// charをEmitします。
		public void EmitToken(char? c){
			if(c == null) return;
			EmitToken(new CharacterToken(c));
		}

		// stringをEmitします。
		// ToDo: 
		public void EmitToken(string s){
			if(s == null) return;
			EmitToken(new CharacterToken(s));
		}

		// CurrentTokenをEmitします。
		public void EmitToken(){
			EmitToken(myCurrentToken);
		}

		// トークン走査状態を変更します。
		public void ChangeTokenState<T>() where T : TokenizationState, new(){
			if(CurrentTokenState != null && typeof(T) == CurrentTokenState.GetType()) return;
			myTokenStateManager.SetState<T>();
			OnTokenStateChanged();
		}
		// トークン走査状態をひとつ前のものに戻します。
		public void BackTokenState(){
			myTokenStateManager.BackState();
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

		// もっとも長い名前字句を取得します。
		public string ConsumeLongestNametokens(){
			StringBuilder result = new StringBuilder();
			result.Append(CurrentInputChar);
			for(;;){
				char? c = ConsumeChar();
				if(!c.IsNameToken()) break;
				result.Append(c);
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


// イベント
		public event EventHandler<ParserEventArgs> ParserEventRaised;

		protected virtual void OnParserEventRaised(Object sender, ParserEventArgs e){
			if(ParserEventRaised != null){
				ParserEventRaised(this, e);
			}
		}

		// ParseErrorRaisedイベントを発生します。
		protected virtual void OnParseErrorRaised(string s){
			OnParserEventRaised(this, new ParserEventArgs(EventLevel.ParseError){Message = s});
		}
		// TokenStateChangedイベントを発生します。
		protected virtual void OnTokenStateChanged(){
			OnParserEventRaised(this, new ParserEventArgs(EventLevel.Verbose));
		}




	}

}

