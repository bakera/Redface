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

public class NullInAttributeNameError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "��������NULL�������܂܂�Ă��܂��B";}}
} // NullInAttributeNameError

public class NullInAttributeValueError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�����l��NULL�������܂܂�Ă��܂��B";}}
} // NullInAttributeValueError

public class InvaridCharAtAfterAttributeNameError : ParserMessage{
public InvaridCharAtAfterAttributeNameError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "������ {0} �̌�� = ���Ȃ��A{1} ���o�����܂����B";}}
} // InvaridCharAtAfterAttributeNameError

public class MissingSpaceAfterAttributeValueError : ParserMessage{
public MissingSpaceAfterAttributeValueError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�����l�̌�ɃX�y�[�X���Ȃ��A{1} ���o�����܂����B";}}
} // MissingSpaceAfterAttributeValueError

public class SuddenlyEndAtAttributeError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�����̉�͒��Ƀt�@�C���I�[�ɒB���܂����B";}}
} // SuddenlyEndAtAttributeError

public class SuddenlyEndAtDoctypeError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�����^�錾�̉�͒��Ƀt�@�C���I�[�ɒB���܂����B";}}
} // SuddenlyEndAtDoctypeError

public class ProcessingInstructionError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�������߂����o���܂����B";}}
} // ProcessingInstructionError

public class DuplicateAttributeError : ParserMessage{
public DuplicateAttributeError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�������O�̑����������w�肳��Ă��܂��B: {0}";}}
} // DuplicateAttributeError

} // namespace
