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

public class UnknownNamedCharacterWithSemicolonError : ParserMessage{
public UnknownNamedCharacterWithSemicolonError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "���O�������Q�� {0} �͕s���ȕ����Q�Ƃł��B";}}
} // UnknownNamedCharacterWithSemicolonError

public class UnknownNamedCharacterWithoutSemicolonWarning : ParserMessage{
public UnknownNamedCharacterWithoutSemicolonWarning(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "���O�������Q�Ƃ炵�������񂪂���܂����A���O {0} �͑��݂��Ȃ����ߖ������܂��B";}}
} // UnknownNamedCharacterWithoutSemicolonWarning

public class NamedCharacterWithoutSemicolonError : ParserMessage{
public NamedCharacterWithoutSemicolonError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "���O�������Q�� {0} �̖����ɂ̓Z�~�R�������K�v�ł��B";}}
} // NamedCharacterWithoutSemicolonError

public class RawAmpersandWarning : ParserMessage{
public override EventLevel Level{get{return EventLevel.Warning;}}
public override string MessageTemplate{get{return "���� & ���g���Ă��܂��B";}}
} // RawAmpersandWarning

public class IgnoredCharacterReferenceInAttributeWarning : ParserMessage{
public IgnoredCharacterReferenceInAttributeWarning(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.Warning;}}
public override string MessageTemplate{get{return "�����l���ɃZ�~�R�����Ȃ��̖��O�������Q�� {0} ���o�����܂������A���̕����Q�Ƃ͓W�J����܂���B";}}
} // IgnoredCharacterReferenceInAttributeWarning

} // namespace

