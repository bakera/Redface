using System;
namespace Bakera.RedFace{
public class GenericVerbose : ParserMessage{
public GenericVerbose(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.Verbose;}}
public override string MessageTemplate{get{return "{0}";}}
} // GenericVerbose

public class EncodingSniffingInformation : ParserMessage{
public override EventLevel Level{get{return EventLevel.Information;}}
public override string MessageTemplate{get{return "文字符号化方式が不明なため、コンテンツ内容から文字符号化方式の推測を行います。";}}
} // EncodingSniffingInformation

public class BOMFoundInformation : ParserMessage{
public BOMFoundInformation(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.Information;}}
public override string MessageTemplate{get{return "BYTE ORDER MARKを検出しました。文字符号化方式を確定しました。: {0}";}}
} // BOMFoundInformation

public class MetaCharsetFoundInformation : ParserMessage{
public MetaCharsetFoundInformation(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.Information;}}
public override string MessageTemplate{get{return "文字符号化方式のSniffing中にmeta charsetを検出しました。文字符号化方式を仮定しました。: {0}";}}
} // MetaCharsetFoundInformation

public class SniffingFailureWarning : ParserMessage{
public SniffingFailureWarning(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.Warning;}}
public override string MessageTemplate{get{return "文字符号化方式のSniffingに失敗しました。文字符号化方式 {0} を仮定して処理します。";}}
} // SniffingFailureWarning

public class CannotChangeFromUTF16Warning : ParserMessage{
public CannotChangeFromUTF16Warning(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.Warning;}}
public override string MessageTemplate{get{return "UTF-16のストリームに文字符号化方式 {0} が指定されています。指定を無視し、文字符号化方式をUTF-16に確定します。";}}
} // CannotChangeFromUTF16Warning

public class CannotChangeToUTF16Warning : ParserMessage{
public CannotChangeToUTF16Warning(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.Warning;}}
public override string MessageTemplate{get{return "文字符号化方式 {0} として読み込まれているデータに、UTF-16が指定されています。UTF-8が指定されているものとみなします。";}}
} // CannotChangeToUTF16Warning

public class SameCharsetInformation : ParserMessage{
public SameCharsetInformation(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.Information;}}
public override string MessageTemplate{get{return "仮定した文字符号化方式 {0} と指定が一致しています。";}}
} // SameCharsetInformation

public class DifferentCharsetWarning : ParserMessage{
public DifferentCharsetWarning(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.Warning;}}
public override string MessageTemplate{get{return "仮定した文字符号化方式 {0} と異なる文字符号化方式 {1} が指定されています。文字符号化方式を変更して構文解析をやり直します。";}}
} // DifferentCharsetWarning

public class UnknownCharsetWarning : ParserMessage{
public UnknownCharsetWarning(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.Warning;}}
public override string MessageTemplate{get{return "未知の文字符号化方式が指定されています。: {0}";}}
} // UnknownCharsetWarning

public class NonCharactersError : ParserMessage{
public NonCharactersError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "制御コード、あるいは未定義の文字 (noncharacters) が含まれています。: {0}";}}
} // NonCharactersError

public class ZWNBSPWarning : ParserMessage{
public override EventLevel Level{get{return EventLevel.Warning;}}
public override string MessageTemplate{get{return "文中に U+FEFF (BYTE ORDER MARK / ZERO WIDTH NO BREAK SPACE) を検出しましたが、無視します。";}}
} // ZWNBSPWarning

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

public class InvaridXMLCharInElementNameError : ParserMessage{
public InvaridXMLCharInElementNameError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.Alert;}}
public override string MessageTemplate{get{return "要素名 {0} にはXMLの要素名に使用できない文字が含まれています。";}}
} // InvaridXMLCharInElementNameError

public class InvaridXMLCharInAttributeNameError : ParserMessage{
public InvaridXMLCharInAttributeNameError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.Alert;}}
public override string MessageTemplate{get{return "属性名 {0} にはXMLでは使用できない文字が含まれています。";}}
} // InvaridXMLCharInAttributeNameError

public class UnknownXMLError : ParserMessage{
public UnknownXMLError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.Alert;}}
public override string MessageTemplate{get{return "不明なXMLのエラーです。: {0}";}}
} // UnknownXMLError

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

public class SuddenlyEndAtElementError : ParserMessage{
public SuddenlyEndAtElementError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "{0}要素が終了していません。";}}
} // SuddenlyEndAtElementError

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

public class EmptyNumericCharacterReferenceError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "数値文字参照の数値が指定されていません。";}}
} // EmptyNumericCharacterReferenceError

public class NamedCharacterReferenceWithoutSemicolonError : ParserMessage{
public NamedCharacterReferenceWithoutSemicolonError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "数値文字参照の末尾にはセミコロンが必要です。";}}
} // NamedCharacterReferenceWithoutSemicolonError

public class UnknownNamedCharacterReferenceWithSemicolonError : ParserMessage{
public UnknownNamedCharacterReferenceWithSemicolonError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "名前つき文字参照 {0} は不明な文字参照です。";}}
} // UnknownNamedCharacterReferenceWithSemicolonError

public class UnknownNamedCharacterReferenceWithoutSemicolonWarning : ParserMessage{
public UnknownNamedCharacterReferenceWithoutSemicolonWarning(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "名前つき文字参照らしき文字列がありますが、名前 {0} は存在しないため無視します。";}}
} // UnknownNamedCharacterReferenceWithoutSemicolonWarning

public class ReplacedNumericCharacterReferenceError : ParserMessage{
public ReplacedNumericCharacterReferenceError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "使用できない文字 {0} を参照しようとしました。文字は {1} に置き換えられます。";}}
} // ReplacedNumericCharacterReferenceError

public class SurrogateNumericCharacterReferenceError : ParserMessage{
public SurrogateNumericCharacterReferenceError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "サロゲート領域の文字 {0} を参照しようとしました。";}}
} // SurrogateNumericCharacterReferenceError

public class TooLargeNumericCharacterReferenceError : ParserMessage{
public TooLargeNumericCharacterReferenceError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "数値文字参照で0x10FFFF以降のコード {0} が指定されています。";}}
} // TooLargeNumericCharacterReferenceError

public class NoncharactersNumericCharacterReferenceError : ParserMessage{
public NoncharactersNumericCharacterReferenceError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "数値文字参照で非Unicode文字 (noncharacters) のコード {0} が指定されています。";}}
} // NoncharactersNumericCharacterReferenceError

public class NumericCharacterReferenceWithoutSemicolonError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "数値文字参照の末尾にはセミコロンが必要です。";}}
} // NumericCharacterReferenceWithoutSemicolonError

public class RawAmpersandWarning : ParserMessage{
public override EventLevel Level{get{return EventLevel.Warning;}}
public override string MessageTemplate{get{return "生の & が使われています。";}}
} // RawAmpersandWarning

public class IgnoredCharacterReferenceInAttributeWarning : ParserMessage{
public IgnoredCharacterReferenceInAttributeWarning(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.Warning;}}
public override string MessageTemplate{get{return "属性値中にセミコロンなしの名前つき文字参照 {0} が出現しましたが、この文字参照は展開されません。";}}
} // IgnoredCharacterReferenceInAttributeWarning

public class NotAcknowledgedSelfClosingTagError : ParserMessage{
public NotAcknowledgedSelfClosingTagError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "{0} 要素の開始タグをself-closingタグにすることはできません。";}}
} // NotAcknowledgedSelfClosingTagError

public class SelfClosingEndTagError : ParserMessage{
public SelfClosingEndTagError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "終了タグをself-closingタグにすることはできません。";}}
} // SelfClosingEndTagError

public class EndTagWithAttributeError : ParserMessage{
public EndTagWithAttributeError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "{0}要素の終了タグに属性が指定されています。";}}
} // EndTagWithAttributeError

public class UnexpectedNamespaceError : ParserMessage{
public UnexpectedNamespaceError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "{0}要素は名前空間{1}に属する要素ですが、xmlns属性で名前空間{2}が指定されています。";}}
} // UnexpectedNamespaceError

public class UnexpectedXlinkNamespaceError : ParserMessage{
public UnexpectedXlinkNamespaceError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "xmlns:xlink属性の値は{0}でなければなりませんが、{1}が指定されています。";}}
} // UnexpectedXlinkNamespaceError

public class UnexpectedTokenAfterHtmlError : ParserMessage{
public UnexpectedTokenAfterHtmlError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "html要素の後ろに予期しないトークンが出現しました。{0}";}}
} // UnexpectedTokenAfterHtmlError

public class UnexpectedDoctypeError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "予期しない文書型宣言が出現しました。";}}
} // UnexpectedDoctypeError

public class UnexpectedInHeadElementError : ParserMessage{
public UnexpectedInHeadElementError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "{0}要素がhead要素の外で使われています。";}}
} // UnexpectedInHeadElementError

public class MultipleHtmlElementError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "html要素が複数あります。";}}
} // MultipleHtmlElementError

public class MultipleHeadElementError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "head要素が複数あります。";}}
} // MultipleHeadElementError

public class MultipleBodyElementError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "body要素が複数あります。";}}
} // MultipleBodyElementError

public class UnexpectedFramesetElementError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "body要素とframeset要素を併用することはできません。";}}
} // UnexpectedFramesetElementError

public class UnexpectedEndTagError : ParserMessage{
public UnexpectedEndTagError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "予期しない{0}要素の終了タグがあります。";}}
} // UnexpectedEndTagError

public class LonlyEndTagError : ParserMessage{
public LonlyEndTagError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "{0}要素の終了タグがありますが、対応する開始タグがありません。";}}
} // LonlyEndTagError

public class MissingEndTagError : ParserMessage{
public MissingEndTagError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "{0}要素の終了タグがありません。";}}
} // MissingEndTagError

public class BrEndTagError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "br要素の終了タグを書くことはできません。";}}
} // BrEndTagError

public class ColEndTagError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "col要素の終了タグを書くことはできません。";}}
} // ColEndTagError

public class SarcasmEndTagInformation : ParserMessage{
public override EventLevel Level{get{return EventLevel.Information;}}
public override string MessageTemplate{get{return "sarcasm要素の終了タグが出現しました。";}}
} // SarcasmEndTagInformation

public class NestedHeadingElementError : ParserMessage{
public NestedHeadingElementError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "{0}見出しの中に{1}の開始タグが出現しました。見出しを入れ子にすることはできません。";}}
} // NestedHeadingElementError

public class NestedFormElementError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "form要素を入れ子にすることはできません。";}}
} // NestedFormElementError

public class NestedButtonElementError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "button要素を入れ子にすることはできません。";}}
} // NestedButtonElementError

public class NestedAnchorElementError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "a要素を入れ子にすることはできません。";}}
} // NestedAnchorElementError

public class NestedNobrElementError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "nobr要素を入れ子にすることはできません。";}}
} // NestedNobrElementError

public class NestedSelectElementError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "select要素を入れ子にすることはできません。";}}
} // NestedSelectElementError

public class UnexpectedStartTagInSelectError : ParserMessage{
public UnexpectedStartTagInSelectError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "select要素の終了タグがなく、{0}要素が出現しました。";}}
} // UnexpectedStartTagInSelectError

public class UnexpectedStartTagInSelectInTableError : ParserMessage{
public UnexpectedStartTagInSelectInTableError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "table要素内のselect要素の終了タグがなく、{0}要素が出現しました。";}}
} // UnexpectedStartTagInSelectInTableError

public class UnexpectedEndTagInSelectInTableError : ParserMessage{
public UnexpectedEndTagInSelectInTableError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "table要素内のselect要素の終了タグがなく、{0}要素の終了タグが出現しました。";}}
} // UnexpectedEndTagInSelectInTableError

public class DisproportionalFormatEndTagError : ParserMessage{
public DisproportionalFormatEndTagError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "{0}要素が正しい入れ子になっていません。";}}
} // DisproportionalFormatEndTagError

public class CellWithoutTableRowError : ParserMessage{
public CellWithoutTableRowError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "tbody要素の直下に{0}要素の開始タグが出現しました。";}}
} // CellWithoutTableRowError

public class DoubleTableError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "table要素直下にtable要素の開始タグが出現しました。";}}
} // DoubleTableError

public class HiddenInputInTableError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "table要素直下にinput type=\"hidden\"が出現しました。";}}
} // HiddenInputInTableError

public class FormInTableError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "table要素直下にform要素の開始タグが出現しました。";}}
} // FormInTableError

public class FosterParentedTokenError : ParserMessage{
public FosterParentedTokenError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "table要素直下に予期しないトークン {0} があります。";}}
} // FosterParentedTokenError

public class FosterParentedTextError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "table要素直下に空白類以外のテキストがあります。";}}
} // FosterParentedTextError

public class UnexpectedTokenInSelectError : ParserMessage{
public UnexpectedTokenInSelectError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "select要素の中に予期しないトークン {0} があります。";}}
} // UnexpectedTokenInSelectError

public class ImageElementError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "HTML5ではimage要素を使用することはできません。";}}
} // ImageElementError

public class IsindexElementError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "HTML5ではisindex要素を使用することはできません。";}}
} // IsindexElementError

public class DirectParentRequiredError : ParserMessage{
public DirectParentRequiredError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "{0}要素は{1}要素の直接の子要素でなければなりませんが、{2}要素の子要素になっています。";}}
} // DirectParentRequiredError

public class ElementContextError : ParserMessage{
public ElementContextError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "{0}要素をこの文脈で使用することはできません。";}}
} // ElementContextError

public class UnexpectedTokenInFramesetError : ParserMessage{
public UnexpectedTokenInFramesetError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "frameset要素内に予期しないトークンが出現しました。";}}
} // UnexpectedTokenInFramesetError

public class UnexpectedStartTagInHeadNoscriptError : ParserMessage{
public UnexpectedStartTagInHeadNoscriptError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "{0}要素をhead内のnoscript要素内で使用することはできません。";}}
} // UnexpectedStartTagInHeadNoscriptError

public class UnexpectedStartTagInCaptionError : ParserMessage{
public UnexpectedStartTagInCaptionError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "caption要素終了タグがなく、{0}要素が現れました。";}}
} // UnexpectedStartTagInCaptionError

public class NamelessDoctypeFailure : ParserMessage{
public override EventLevel Level{get{return EventLevel.SystemFailure;}}
public override string MessageTemplate{get{return "名前のない文書型宣言を扱うことはできません。";}}
} // NamelessDoctypeFailure

public class NoDoctypeError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "文書型宣言がありません。";}}
} // NoDoctypeError

public class UnknownDoctypeError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "未知の文書型宣言が指定されています。";}}
} // UnknownDoctypeError

public class QuirksDoctypeError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "Quirksモードとなる文書型宣言が指定されています。";}}
} // QuirksDoctypeError

public class LimitedQuirksDoctypeError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "Limited Quirksモードとなる文書型宣言が指定されています。";}}
} // LimitedQuirksDoctypeError

} // namespace

