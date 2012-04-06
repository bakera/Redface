using System;
namespace Bakera.RedFace{
public class NonCharactersError : ParserMessage{
public NonCharactersError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "制御コード、あるいは未定義の文字 (noncharacters) が含まれています。: {0}";}}
} // NonCharactersError

public class ZWNBSPInformation : ParserMessage{
public ZWNBSPInformation(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.Warning;}}
public override string MessageTemplate{get{return "文中に U+FEFF (BYTE ORDER MARK / ZERO WIDTH NO BREAK SPACE) を検出しましたが、無視します。: {0}";}}
} // ZWNBSPInformation

public class NullInElementNameError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "要素名にNULL文字が含まれています。";}}
} // NullInElementNameError

public class NullInAttributeNameError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "属性名にNULL文字が含まれています。";}}
} // NullInAttributeNameError

public class NullInAttributeValueError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "属性値にNULL文字が含まれています。";}}
} // NullInAttributeValueError

public class NullInDoctypeError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "文書型宣言にNULL文字が含まれています。";}}
} // NullInDoctypeError

public class NullInCommentError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "コメントの中にNULL文字が含まれています。";}}
} // NullInCommentError

public class NullInDataError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "データにNULL文字が含まれています。";}}
} // NullInDataError

public class NullInScriptError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "スクリプトのデータにNULL文字が含まれています。";}}
} // NullInScriptError

public class InvaridCharAtBeforeAttributeNameError : ParserMessage{
public InvaridCharAtBeforeAttributeNameError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "属性名がなく、{0} が出現しました。";}}
} // InvaridCharAtBeforeAttributeNameError

public class InvaridCharAtAfterAttributeNameError : ParserMessage{
public InvaridCharAtAfterAttributeNameError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "属性名 {0} の後に = がなく、{1} が出現しました。";}}
} // InvaridCharAtAfterAttributeNameError

public class MissingAttributeValueError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "属性の = の後ろに属性値が指定されていません。";}}
} // MissingAttributeValueError

public class MissingSpaceAfterAttributeValueError : ParserMessage{
public MissingSpaceAfterAttributeValueError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "属性値の後にスペースがなく、{1} が出現しました。";}}
} // MissingSpaceAfterAttributeValueError

public class InvalidCharInUnquotedAttributeValueError : ParserMessage{
public InvalidCharInUnquotedAttributeValueError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "引用符でくくられていない属性値の中に、使用できない文字 {0} が出現しました。";}}
} // InvalidCharInUnquotedAttributeValueError

public class SuddenlyEndAtAttributeError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "属性が終了していません。";}}
} // SuddenlyEndAtAttributeError

public class SuddenlyEndAtDoctypeError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "文書型宣言が終了していません。";}}
} // SuddenlyEndAtDoctypeError

public class SuddenlyEndAtCommentError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "コメントが終了していません。";}}
} // SuddenlyEndAtCommentError

public class SuddenlyEndAtTagError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "タグが終了していません。";}}
} // SuddenlyEndAtTagError

public class SuddenlyEndAtScriptError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "スクリプトが終了していません。";}}
} // SuddenlyEndAtScriptError

public class EmptyDoctypeError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "文書型宣言が空です。";}}
} // EmptyDoctypeError

public class UnknownIdentifierInDoctypeError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "文書型宣言の中に不明な文字があります。";}}
} // UnknownIdentifierInDoctypeError

public class UnknownIdentifierAfterDoctypeError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "文書型宣言の末尾に不明な文字があります。";}}
} // UnknownIdentifierAfterDoctypeError

public class MissingSpaceBeforeDoctypeIdentifierError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "文書型宣言の識別子の前にスペースがありません。";}}
} // MissingSpaceBeforeDoctypeIdentifierError

public class MissingPublicIdentifierError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "文書型宣言の公開識別子がありません。";}}
} // MissingPublicIdentifierError

public class GreaterThanSignInIdentifierError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "文書型宣言の識別子の引用符が閉じていません。";}}
} // GreaterThanSignInIdentifierError

public class MissingSystemIdentifierError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "文書型宣言のシステム識別子がありません。";}}
} // MissingSystemIdentifierError

public class MissingQuoteIdentifierError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "文書型宣言の識別子が引用符で括られていません。";}}
} // MissingQuoteIdentifierError

public class UnknownMarkupDeclarationError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "不明なマーク宣言です。";}}
} // UnknownMarkupDeclarationError

public class ProcessingInstructionError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "処理命令を検出しました。";}}
} // ProcessingInstructionError

public class HyphenTooMuchCommentError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "コメントの中に余計なハイフンがあります。";}}
} // HyphenTooMuchCommentError

public class DoubleHyphenInCommentError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "コメントの途中に -- を含めることはできません。";}}
} // DoubleHyphenInCommentError

public class LessHyphenCommentError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "コメントにはハイフンが4つ必要です。";}}
} // LessHyphenCommentError

public class DuplicateAttributeError : ParserMessage{
public DuplicateAttributeError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "同じ名前の属性が複数指定されています。: {0}";}}
} // DuplicateAttributeError

public class InvaridAttributeInSelfClosingTagError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "SelfClosingTagの / の後に属性が書かれています。";}}
} // InvaridAttributeInSelfClosingTagError

public class EmptyEndTagError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "空の終了タグが使われています。";}}
} // EmptyEndTagError

public class UnknownEndTagError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "不明な終了タグがあります。";}}
} // UnknownEndTagError

public class UnknownMarkupError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "不明な < があります。";}}
} // UnknownMarkupError

public class UnknownNamedCharacterWithSemicolonError : ParserMessage{
public UnknownNamedCharacterWithSemicolonError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "名前つき文字参照 {0} は不明な文字参照です。";}}
} // UnknownNamedCharacterWithSemicolonError

public class UnknownNamedCharacterWithoutSemicolonWarning : ParserMessage{
public UnknownNamedCharacterWithoutSemicolonWarning(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "名前つき文字参照らしき文字列がありますが、名前 {0} は存在しないため無視します。";}}
} // UnknownNamedCharacterWithoutSemicolonWarning

public class NamedCharacterWithoutSemicolonError : ParserMessage{
public NamedCharacterWithoutSemicolonError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "名前つき文字参照 {0} の末尾にはセミコロンが必要です。";}}
} // NamedCharacterWithoutSemicolonError

public class RawAmpersandWarning : ParserMessage{
public override EventLevel Level{get{return EventLevel.Warning;}}
public override string MessageTemplate{get{return "生の & が使われています。";}}
} // RawAmpersandWarning

public class IgnoredCharacterReferenceInAttributeWarning : ParserMessage{
public IgnoredCharacterReferenceInAttributeWarning(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.Warning;}}
public override string MessageTemplate{get{return "属性値中にセミコロンなしの名前つき文字参照 {0} が出現しましたが、この文字参照は展開されません。";}}
} // IgnoredCharacterReferenceInAttributeWarning

} // namespace

