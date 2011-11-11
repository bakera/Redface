using System;
using System.Xml;

namespace Bakera.RedFace{

	public enum EventLevel{
		Verbose,          // デバッグ用などの詳細情報
		Information,      // 一般的な情報。エラーではなく、問題ない
		Warning,          // 注意。エラーではないが、望ましくない可能性があるもの
		Alert,            // 警告。仕様に反しないが、望ましくないと仕様に明記してあるもの
		ConformanceError, // 文法エラー。ParseError ではないが、仕様に反するもの
		ParseError,       // パースエラー。ParseError と仕様に明記してあるもの
		SystemError,      // システムエラー。システム実装の都合で処理できないもの。
		Exception,        // 例外。意図しないエラー (データが読めなかった、プログラムの不具合、など)。
	}
}



