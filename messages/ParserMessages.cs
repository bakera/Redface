using System;
namespace Bakera.RedFace{
public class NonCharactersError : ParserMessage{
public NonCharactersError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "����R�[�h�A���邢�͖���`�̕��� (noncharacters) ���܂܂�Ă��܂��B: {0}";}}
} // NonCharactersError

public class ZWNBSPInformation : ParserMessage{
public ZWNBSPInformation(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.Warning;}}
public override string MessageTemplate{get{return "������ U+FEFF (BYTE ORDER MARK / ZERO WIDTH NO BREAK SPACE) �����o���܂������A�������܂��B: {0}";}}
} // ZWNBSPInformation

public class NullInElementNameError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�v�f����NULL�������܂܂�Ă��܂��B";}}
} // NullInElementNameError

public class NullInAttributeNameError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "��������NULL�������܂܂�Ă��܂��B";}}
} // NullInAttributeNameError

public class NullInAttributeValueError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�����l��NULL�������܂܂�Ă��܂��B";}}
} // NullInAttributeValueError

public class NullInDoctypeError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�����^�錾��NULL�������܂܂�Ă��܂��B";}}
} // NullInDoctypeError

public class NullInCommentError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�R�����g�̒���NULL�������܂܂�Ă��܂��B";}}
} // NullInCommentError

public class NullInDataError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�f�[�^��NULL�������܂܂�Ă��܂��B";}}
} // NullInDataError

public class NullInScriptError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�X�N���v�g�̃f�[�^��NULL�������܂܂�Ă��܂��B";}}
} // NullInScriptError

public class InvaridCharAtBeforeAttributeNameError : ParserMessage{
public InvaridCharAtBeforeAttributeNameError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "���������Ȃ��A{0} ���o�����܂����B";}}
} // InvaridCharAtBeforeAttributeNameError

public class InvaridCharAtAfterAttributeNameError : ParserMessage{
public InvaridCharAtAfterAttributeNameError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "������ {0} �̌�� = ���Ȃ��A{1} ���o�����܂����B";}}
} // InvaridCharAtAfterAttributeNameError

public class MissingAttributeValueError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "������ = �̌��ɑ����l���w�肳��Ă��܂���B";}}
} // MissingAttributeValueError

public class MissingSpaceAfterAttributeValueError : ParserMessage{
public MissingSpaceAfterAttributeValueError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�����l�̌�ɃX�y�[�X���Ȃ��A{1} ���o�����܂����B";}}
} // MissingSpaceAfterAttributeValueError

public class InvalidCharInUnquotedAttributeValueError : ParserMessage{
public InvalidCharInUnquotedAttributeValueError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "���p���ł������Ă��Ȃ������l�̒��ɁA�g�p�ł��Ȃ����� {0} ���o�����܂����B";}}
} // InvalidCharInUnquotedAttributeValueError

public class SuddenlyEndAtAttributeError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�������I�����Ă��܂���B";}}
} // SuddenlyEndAtAttributeError

public class SuddenlyEndAtDoctypeError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�����^�錾���I�����Ă��܂���B";}}
} // SuddenlyEndAtDoctypeError

public class SuddenlyEndAtCommentError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�R�����g���I�����Ă��܂���B";}}
} // SuddenlyEndAtCommentError

public class SuddenlyEndAtTagError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�^�O���I�����Ă��܂���B";}}
} // SuddenlyEndAtTagError

public class SuddenlyEndAtScriptError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�X�N���v�g���I�����Ă��܂���B";}}
} // SuddenlyEndAtScriptError

public class SuddenlyEndAtElementError : ParserMessage{
public SuddenlyEndAtElementError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "{0}�v�f���I�����Ă��܂���B";}}
} // SuddenlyEndAtElementError

public class EmptyDoctypeError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�����^�錾����ł��B";}}
} // EmptyDoctypeError

public class UnknownIdentifierInDoctypeError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�����^�錾�̒��ɕs���ȕ���������܂��B";}}
} // UnknownIdentifierInDoctypeError

public class UnknownIdentifierAfterDoctypeError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�����^�錾�̖����ɕs���ȕ���������܂��B";}}
} // UnknownIdentifierAfterDoctypeError

public class MissingSpaceBeforeDoctypeIdentifierError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�����^�錾�̎��ʎq�̑O�ɃX�y�[�X������܂���B";}}
} // MissingSpaceBeforeDoctypeIdentifierError

public class MissingPublicIdentifierError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�����^�錾�̌��J���ʎq������܂���B";}}
} // MissingPublicIdentifierError

public class GreaterThanSignInIdentifierError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�����^�錾�̎��ʎq�̈��p�������Ă��܂���B";}}
} // GreaterThanSignInIdentifierError

public class MissingSystemIdentifierError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�����^�錾�̃V�X�e�����ʎq������܂���B";}}
} // MissingSystemIdentifierError

public class MissingQuoteIdentifierError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�����^�錾�̎��ʎq�����p���Ŋ����Ă��܂���B";}}
} // MissingQuoteIdentifierError

public class UnknownMarkupDeclarationError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�s���ȃ}�[�N�錾�ł��B";}}
} // UnknownMarkupDeclarationError

public class ProcessingInstructionError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�������߂����o���܂����B";}}
} // ProcessingInstructionError

public class HyphenTooMuchCommentError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�R�����g�̒��ɗ]�v�ȃn�C�t��������܂��B";}}
} // HyphenTooMuchCommentError

public class DoubleHyphenInCommentError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�R�����g�̓r���� -- ���܂߂邱�Ƃ͂ł��܂���B";}}
} // DoubleHyphenInCommentError

public class LessHyphenCommentError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�R�����g�ɂ̓n�C�t����4�K�v�ł��B";}}
} // LessHyphenCommentError

public class DuplicateAttributeError : ParserMessage{
public DuplicateAttributeError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�������O�̑����������w�肳��Ă��܂��B: {0}";}}
} // DuplicateAttributeError

public class InvaridAttributeInSelfClosingTagError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "SelfClosingTag�� / �̌�ɑ�����������Ă��܂��B";}}
} // InvaridAttributeInSelfClosingTagError

public class EmptyEndTagError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "��̏I���^�O���g���Ă��܂��B";}}
} // EmptyEndTagError

public class UnknownEndTagError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�s���ȏI���^�O������܂��B";}}
} // UnknownEndTagError

public class UnknownMarkupError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�s���� < ������܂��B";}}
} // UnknownMarkupError

public class EmptyNumericCharacterReferenceError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "���l�����Q�Ƃ̐��l���w�肳��Ă��܂���B";}}
} // EmptyNumericCharacterReferenceError

public class NamedCharacterReferenceWithoutSemicolonError : ParserMessage{
public NamedCharacterReferenceWithoutSemicolonError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "���l�����Q�Ƃ̖����ɂ̓Z�~�R�������K�v�ł��B";}}
} // NamedCharacterReferenceWithoutSemicolonError

public class UnknownNamedCharacterReferenceWithSemicolonError : ParserMessage{
public UnknownNamedCharacterReferenceWithSemicolonError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "���O�������Q�� {0} �͕s���ȕ����Q�Ƃł��B";}}
} // UnknownNamedCharacterReferenceWithSemicolonError

public class UnknownNamedCharacterReferenceWithoutSemicolonWarning : ParserMessage{
public UnknownNamedCharacterReferenceWithoutSemicolonWarning(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "���O�������Q�Ƃ炵�������񂪂���܂����A���O {0} �͑��݂��Ȃ����ߖ������܂��B";}}
} // UnknownNamedCharacterReferenceWithoutSemicolonWarning

public class ReplacedNumericCharacterReferenceError : ParserMessage{
public ReplacedNumericCharacterReferenceError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�g�p�ł��Ȃ����� {0} ���Q�Ƃ��悤�Ƃ��܂����B������ {1} �ɒu���������܂��B";}}
} // ReplacedNumericCharacterReferenceError

public class SurrogateNumericCharacterReferenceError : ParserMessage{
public SurrogateNumericCharacterReferenceError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�T���Q�[�g�̈�̕��� {0} ���Q�Ƃ��悤�Ƃ��܂����B";}}
} // SurrogateNumericCharacterReferenceError

public class TooLargeNumericCharacterReferenceError : ParserMessage{
public TooLargeNumericCharacterReferenceError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "���l�����Q�Ƃ�0x10FFFF�ȍ~�̃R�[�h {0} ���w�肳��Ă��܂��B";}}
} // TooLargeNumericCharacterReferenceError

public class NoncharactersNumericCharacterReferenceError : ParserMessage{
public NoncharactersNumericCharacterReferenceError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "���l�����Q�ƂŔ�Unicode���� (noncharacters) �̃R�[�h {0} ���w�肳��Ă��܂��B";}}
} // NoncharactersNumericCharacterReferenceError

public class NumericCharacterReferenceWithoutSemicolonError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "���l�����Q�Ƃ̖����ɂ̓Z�~�R�������K�v�ł��B";}}
} // NumericCharacterReferenceWithoutSemicolonError

public class RawAmpersandWarning : ParserMessage{
public override EventLevel Level{get{return EventLevel.Warning;}}
public override string MessageTemplate{get{return "���� & ���g���Ă��܂��B";}}
} // RawAmpersandWarning

public class IgnoredCharacterReferenceInAttributeWarning : ParserMessage{
public IgnoredCharacterReferenceInAttributeWarning(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.Warning;}}
public override string MessageTemplate{get{return "�����l���ɃZ�~�R�����Ȃ��̖��O�������Q�� {0} ���o�����܂������A���̕����Q�Ƃ͓W�J����܂���B";}}
} // IgnoredCharacterReferenceInAttributeWarning

public class NotAcknowledgedSelfClosingTagError : ParserMessage{
public NotAcknowledgedSelfClosingTagError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "{0} �v�f�̊J�n�^�O��self-closing�^�O�ɂ��邱�Ƃ͂ł��܂���B";}}
} // NotAcknowledgedSelfClosingTagError

public class SelfClosingEndTagError : ParserMessage{
public SelfClosingEndTagError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�I���^�O��self-closing�^�O�ɂ��邱�Ƃ͂ł��܂���B";}}
} // SelfClosingEndTagError

public class EndTagWithAttributeError : ParserMessage{
public EndTagWithAttributeError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "{0}�v�f�̏I���^�O�ɑ������w�肳��Ă��܂��B";}}
} // EndTagWithAttributeError

public class UnexpectedNamespaceError : ParserMessage{
public UnexpectedNamespaceError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "{0}�v�f�͖��O���{1}�ɑ�����v�f�ł����Axmlns�����Ŗ��O���{2}���w�肳��Ă��܂��B";}}
} // UnexpectedNamespaceError

public class UnexpectedXlinkNamespaceError : ParserMessage{
public UnexpectedXlinkNamespaceError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "xmlns:xlink�����̒l��{0}�łȂ���΂Ȃ�܂��񂪁A{1}���w�肳��Ă��܂��B";}}
} // UnexpectedXlinkNamespaceError

public class UnexpectedTokenAfterHtmlError : ParserMessage{
public UnexpectedTokenAfterHtmlError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "html�v�f�̌��ɗ\�����Ȃ��g�[�N�����o�����܂����B{0}";}}
} // UnexpectedTokenAfterHtmlError

public class UnexpectedDoctypeError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�\�����Ȃ������^�錾���o�����܂����B";}}
} // UnexpectedDoctypeError

public class UnexpectedInHeadElementError : ParserMessage{
public UnexpectedInHeadElementError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "{0}�v�f��head�v�f�̊O�Ŏg���Ă��܂��B";}}
} // UnexpectedInHeadElementError

public class MultipleHtmlElementError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "html�v�f����������܂��B";}}
} // MultipleHtmlElementError

public class MultipleHeadElementError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "head�v�f����������܂��B";}}
} // MultipleHeadElementError

public class MultipleBodyElementError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "body�v�f����������܂��B";}}
} // MultipleBodyElementError

public class UnexpectedFramesetElementError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "body�v�f��frameset�v�f�𕹗p���邱�Ƃ͂ł��܂���B";}}
} // UnexpectedFramesetElementError

public class UnexpectedEndTagError : ParserMessage{
public UnexpectedEndTagError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�\�����Ȃ�{0}�v�f�̏I���^�O������܂��B";}}
} // UnexpectedEndTagError

public class LonlyEndTagError : ParserMessage{
public LonlyEndTagError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "{0}�v�f�̏I���^�O������܂����A�Ή�����J�n�^�O������܂���B";}}
} // LonlyEndTagError

public class MissingEndTagError : ParserMessage{
public MissingEndTagError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "{0}�v�f�̏I���^�O������܂���B";}}
} // MissingEndTagError

public class BrEndTagError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "br�v�f�̏I���^�O���������Ƃ͂ł��܂���B";}}
} // BrEndTagError

public class ColEndTagError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "col�v�f�̏I���^�O���������Ƃ͂ł��܂���B";}}
} // ColEndTagError

public class SarcasmEndTagInformation : ParserMessage{
public override EventLevel Level{get{return EventLevel.Information;}}
public override string MessageTemplate{get{return "sarcasm�v�f�̏I���^�O���o�����܂����B";}}
} // SarcasmEndTagInformation

public class NestedHeadingElementError : ParserMessage{
public NestedHeadingElementError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "{0}���o���̒���{1}�̊J�n�^�O���o�����܂����B���o�������q�ɂ��邱�Ƃ͂ł��܂���B";}}
} // NestedHeadingElementError

public class NestedFormElementError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "form�v�f�����q�ɂ��邱�Ƃ͂ł��܂���B";}}
} // NestedFormElementError

public class NestedButtonElementError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "button�v�f�����q�ɂ��邱�Ƃ͂ł��܂���B";}}
} // NestedButtonElementError

public class NestedAnchorElementError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "a�v�f�����q�ɂ��邱�Ƃ͂ł��܂���B";}}
} // NestedAnchorElementError

public class NestedNobrElementError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "nobr�v�f�����q�ɂ��邱�Ƃ͂ł��܂���B";}}
} // NestedNobrElementError

public class ImageElementError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "HTML5�ł�image�v�f���g�p���邱�Ƃ͂ł��܂���B";}}
} // ImageElementError

public class IsindexElementError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "HTML5�ł�isindex�v�f���g�p���邱�Ƃ͂ł��܂���B";}}
} // IsindexElementError

public class DirectParentRequiredError : ParserMessage{
public DirectParentRequiredError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "{0}�v�f��{1}�v�f�̒��ڂ̎q�v�f�łȂ���΂Ȃ�܂��񂪁A{2}�v�f�̎q�v�f�ɂȂ��Ă��܂��B";}}
} // DirectParentRequiredError

public class ElementContextError : ParserMessage{
public ElementContextError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "{0}�v�f�����̕����Ŏg�p���邱�Ƃ͂ł��܂���B";}}
} // ElementContextError

public class UnexpectedTokenInFramesetError : ParserMessage{
public UnexpectedTokenInFramesetError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "frameset�v�f���ɗ\�����Ȃ��g�[�N�����o�����܂����B";}}
} // UnexpectedTokenInFramesetError

public class UnexpectedStartTagInHeadNoscriptError : ParserMessage{
public UnexpectedStartTagInHeadNoscriptError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "{0}�v�f��head����noscript�v�f���Ŏg�p���邱�Ƃ͂ł��܂���B";}}
} // UnexpectedStartTagInHeadNoscriptError

public class UnexpectedStartTagInCaptionError : ParserMessage{
public UnexpectedStartTagInCaptionError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "caption�v�f�I���^�O���Ȃ��A{0}�v�f������܂����B";}}
} // UnexpectedStartTagInCaptionError

public class NamelessDoctypeFailure : ParserMessage{
public override EventLevel Level{get{return EventLevel.SystemFailure;}}
public override string MessageTemplate{get{return "���O�̂Ȃ������^�錾���������Ƃ͂ł��܂���B";}}
} // NamelessDoctypeFailure

public class NoDoctypeError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�����^�錾������܂���B";}}
} // NoDoctypeError

public class UnknownDoctypeError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "���m�̕����^�錾���w�肳��Ă��܂��B";}}
} // UnknownDoctypeError

public class QuirksDoctypeError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "Quirks���[�h�ƂȂ镶���^�錾���w�肳��Ă��܂��B";}}
} // QuirksDoctypeError

public class LimitedQuirksDoctypeError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "Limited Quirks���[�h�ƂȂ镶���^�錾���w�肳��Ă��܂��B";}}
} // LimitedQuirksDoctypeError

} // namespace

