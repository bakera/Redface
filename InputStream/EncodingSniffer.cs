using System;
using System.IO;
using System.Text;

namespace Bakera.RedFace{

	public class EncodingSniffer{
		private int myPosition = 0;
		private byte[] myBuffer;

		// Sniffを成功させるために必要な最低の文字数。「<meta charset=X」という15文字が現れる余地がなければ処理を打ち切って良い。
		private const int MinimumSniffableLength = 15; 


		// 現在位置からのオフセットを指定してバイト列を取得します。
		private byte Get(int offset){
			return myBuffer[myPosition + offset];
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
					myPosition++;
					while(myPosition < myBuffer.Length - MinimumSniffableLength){
						byte inAttr = Get(0);
						if(inAttr == b){
							myPosition++;
							return new AttributeToken(){Name = attributeName, Value = attributeValue};
						}
//ToDo
					}
					

				}

				
			}



			return null;
		}

		

	}
}



