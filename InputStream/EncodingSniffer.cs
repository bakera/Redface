using System;
using System.Collections.Generic;
using System.Text;

namespace Bakera.RedFace{

	public class EncodingSniffer{
		private int myPosition = 0;
		private byte[] myBuffer;

		// Sniffを成功させるために必要な最低の文字数。「<meta charset=X」という15文字が現れる余地がなければ処理を打ち切って良い。
		private const int MinimumSniffableLength = 15; 

		private static readonly Dictionary<string, string> CharacterEncodingOverrides = new Dictionary<string, string>(){
			{"EUC-KR", "windows-949"},
			{"EUC-JP", "CP51932"},
			{"GB2312", "GBK"},
			{"GB_2312-80", "GBK"},
			{"ISO-8859-1", "windows-1252"},
			{"ISO-8859-9", "windows-1254"},
			{"ISO-8859-11", "windows-874"},
			{"KS_C_5601-1987", "windows-949"},
//			{"Shift_JIS", "Windows-31J"},
			{"TIS-620", "windows-874"},
			{"US-ASCII", "windows-1252"},
		};


		// 現在位置からのオフセットを指定してバイト列を取得します。
		private byte Get(int offset){
			return myBuffer[myPosition + offset];
		}


		// 文字列を受け取って Encoding を返します。
		public static Encoding GetEncodingByName(string s){
			try{
				if(CharacterEncodingOverrides.ContainsKey(s)){
					string newName = CharacterEncodingOverrides[s];
					Console.WriteLine("文字符号化方式の名称 {0} が指定されましたが、{1} を使用します。", s, newName);
					s = newName;
				}
				Encoding enc = Encoding.GetEncoding(s);
				return enc;
			} catch(ArgumentException){
				Console.WriteLine("指定された名称の文字符号化方式は扱えません。: {0}", s);
				return null;
			}
		}



		// 渡されたバイナリデータを読み取ってEncodingを判別します。
		// 判別に成功した場合は Encoding を返します。
		// 判別に失敗した場合は null を返します。
		public Encoding SniffEncoding(byte[] buffer){
			myBuffer = buffer;
			while(myPosition < buffer.Length - MinimumSniffableLength){
				if(Get(0) != 0x3c){
					myPosition++;
					continue;
				}
				byte nextByte = Get(1);
				byte nextNextByte = Get(2);
				if(nextByte == 0x21 && nextNextByte == 0x2D && Get(3) == 0x2D){ // <!--
					myPosition += 5; // !-- をスキップ、さらに --> の -- ぶんスキップ
					SkipComment();
					continue;
				} else if(IsMeta()){ // <meta
					Encoding result = SniffMetaElement();
					if(result == null) continue;
					return result;
				} else if(IsLetter(nextByte) || (nextByte == 0x2f && IsLetter(nextNextByte))){
					SkipTag();
					continue;
				}
			}
			return null;
		}


		// 空白類文字をスキップします。
		private void SkipSpace(){
			while(myPosition < myBuffer.Length - MinimumSniffableLength){
				if(!IsSpace(Get(0))) break;
				myPosition++;
			}
		}

		// コメントらしき部分をスキップします。
		// --> が出現するまでスキップします。
		private void SkipComment(){
			while(myPosition < myBuffer.Length - MinimumSniffableLength){
				if(Get(0) == 0x3E && Get(-1) == 0x21 && Get(-2) == 0x21){
					myPosition++;
					break;
				}
				myPosition++;
			}
		}

		// タグらしき部分をスキップします。
		private void SkipTag(){
			// 次のスペースか > まで飛ばす
			while(myPosition < myBuffer.Length - MinimumSniffableLength){
				if(IsSpace(Get(0)) || Get(0) == 0x3E) break;
				myPosition++;
			}
			while(GetAttribute() != null){}
		}

		// 次の > までをスキップします。
		private void SkipToGt(){
			while(myPosition < myBuffer.Length - MinimumSniffableLength){
				if(Get(0) == 0x3c){
					myPosition++;
					break;
				}
				myPosition++;
			}
		}


		// アルファベットの文字だったら true を返します。
		private static bool IsLetter(byte b){
			return (0x41 <= b && b <= 0x5A) || (0x61 <= b && b <= 0x7A);
		}

		// Space だったら true を返します。
		private static bool IsSpace(byte b){
			switch(b){
			case 0x09:
			case 0x0A:
			case 0x0C:
			case 0x0D:
			case 0x20:
				return true;
			default:
				return false;
			}

		}


		// 現在位置以降が 「meta 」という文字列だったら trueを返します。
		private bool IsMeta(){
			if (Get(1) != 0x4D && Get(1) != 0x6D) return false;
			if (Get(2) != 0x45 && Get(2) != 0x65) return false;
			if (Get(3) != 0x54 && Get(3) != 0x74) return false;
			if (Get(4) != 0x41 && Get(4) != 0x61) return false;
			if (!IsSpace(Get(5)) && Get(5) != 0x2f) return false;
			return true;
		}


	// meta 要素の属性を読み取って Encoding を決定します。

		private Encoding SniffMetaElement(){
			while(myPosition < myBuffer.Length - MinimumSniffableLength){
				if(IsSpace(Get(0)) || Get(0) == 0x2F) break;
				myPosition++;
			}
			HashSet<string> attributeList = new HashSet<string>();
			bool gotPragma = false;
			bool? needPragma = null;
			string charset = null;

			for(;;){
				AttributeToken at = GetAttribute();
				if(at == null) break;
				if(attributeList.Add(at.Name) == false) continue;
				switch(at.Name){
				case "http-equiv":
					if(at.Value == "content-type") gotPragma =  true;
					break;
				case "content":
					string resultString = ExtractEncodingNameFromMetaElement(at.Value);
					if(resultString != null && charset == null){
						charset = resultString;
						needPragma = true;
					}
					break;
				case "charset":
					charset = at.Value;
					needPragma = false;
					break;
				}
			}

			if(needPragma == null) return null;
			if(needPragma == true && gotPragma == false) return null;

			if(charset.Equals("UTF-16", StringComparison.InvariantCultureIgnoreCase)){
				charset = "UTF-8";
			}
			Encoding result = GetEncodingByName(charset);
			return result;
		}




		// 現在位置以降から属性を取得します。
		private AttributeToken GetAttribute(){
			SkipSpace();

			if(Get(0) == 0x3E) return null;

			string attributeName = "";
			string attributeValue = "";

			while(myPosition < myBuffer.Length - MinimumSniffableLength){
				byte current = Get(0);
				if(current == 0x3D && attributeName == ""){
					myPosition++;
					goto Value;
				} else if(IsSpace(current)){
					goto Spaces;
				} else if(current == 0x2F || current == 0x3E){
					return new AttributeToken(){Name = attributeName, Value = ""};
				} else if(0x41 <= current && current <= 0x5A){
					attributeName += (char)(current + 0x20);
				} else {
					attributeName += (char)(current);
				}
				myPosition++;
			}

			Spaces:{
				SkipSpace();
				if(Get(0) != 0x3D){
					return new AttributeToken(){Name = attributeName, Value = ""};
				}
				myPosition++;
			}

			Value:{
				SkipSpace();
				byte current = Get(0);
				if(current == 0x22 || current == 0x27){
					byte b = current;
					while(myPosition < myBuffer.Length - MinimumSniffableLength){
						myPosition++;
						byte inAttr = Get(0);
						if(inAttr == b){
							myPosition++;
							return new AttributeToken(){Name = attributeName, Value = attributeValue};
						} else if(0x41 <= current && current <= 0x5A){
							attributeValue += (char)(current + 0x20);
						} else {
							attributeValue += (char)(current);
						}
					}
				} else if(current == 0x3E){
					return new AttributeToken(){Name = attributeName, Value = ""};
				} else if(0x41 <= current && current <= 0x5A){
					attributeValue += (char)(current + 0x20);
					myPosition++;
				} else {
					attributeValue += (char)(current);
					myPosition++;
				}

				while(myPosition < myBuffer.Length - MinimumSniffableLength){
					current = Get(0);
					if(IsSpace(current) || current == 0x3E){
						return new AttributeToken(){Name = attributeName, Value = attributeValue};
					} else if(0x41 <= current && current <= 0x5A){
						attributeValue += (char)(current + 0x20);
					} else {
						attributeValue += (char)(current);
					}
					myPosition++;
				}
			}
			return null;
		}

		
		private string ExtractEncodingNameFromMetaElement(string s){
			int position = 0;
			for(;;){
				int idx = s.IndexOf("charset", position, StringComparison.InvariantCultureIgnoreCase);
				if(idx < 0) return null;
				position = idx + 7;
				for(;position < s.Length; position++){
					if(!s[position].IsSpaceCharacter()) break;
				}
				if(position >= s.Length) return null;
				if(s[position] == Chars.EQUALS_SIGN) break;
			}
			position++;
			for(;position < s.Length; position++){
				if(!s[position].IsSpaceCharacter()) break;
			}
			if(position >= s.Length) return null;
			if(s[position] == Chars.QUOTATION_MARK){
				int idx = s.IndexOf(Chars.QUOTATION_MARK, position);
				if(idx < 0) return null;
				return s.Substring(position, idx - position);
			} else if(s[position] == Chars.APOSTROPHE){
				int idx = s.IndexOf(Chars.APOSTROPHE, position);
				if(idx < 0) return null;
				return s.Substring(position, idx - position);
			}

			for(int idx = 0; position + idx < s.Length; idx++){
				if(!s[position+idx].IsSpaceCharacter()) continue;
				return s.Substring(position, idx - position);
			}
			return null;
		}


	}
}



