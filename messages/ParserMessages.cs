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

public class ProcessingInstructionError : ParserMessage{
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�������߂����o���܂����B";}}
} // ProcessingInstructionError

public class DuplicateAttributeError : ParserMessage{
public DuplicateAttributeError(params Object[] o){this.Params = o;}
public override EventLevel Level{get{return EventLevel.ParseError;}}
public override string MessageTemplate{get{return "�������d�����Ă��܂��B: {0}";}}
} // DuplicateAttributeError

} // namespace

