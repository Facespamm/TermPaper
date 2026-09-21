using TermPaper.Enum;

namespace TermPaper.Application.Dto.PositionAccessRuleDto;

public class CreatePositionAccessRuleDto
{
    public int AttributeId { get; set; }
    
    public int PositionId  { get; set; }
    
    public Operator Operator { get; set; }
    
    public string Value { get; set; }

}