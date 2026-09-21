using TermPaper.Application.Dto.PositionAccessRuleDto;
using TermPaper.Application.Dto.PositionAttributeDto;
using TermPaper.Domain.Models;

namespace TermPaper.Infrastructure.Mappers;

public class PositionAccessRuleMapper
{
    public static AccessRule CreateAccessRule(CreatePositionAccessRuleDto dto)
    {
        return new AccessRule()
        {
            AttributeId = dto.AttributeId,
            Operator = dto.Operator,
            Value = dto.Value,
            PositionId = dto.PositionId,
        };
    }

    public static GetPositionAccessRuleDto GetPositionAccessRule(AccessRule accessRule)
    {
        return new GetPositionAccessRuleDto()
        {
            Id = accessRule.Id,
            AttributeId = accessRule.AttributeId,
            Operator = accessRule.Operator,
            Value = accessRule.Value,
            PositionId = accessRule.PositionId,
        };
    }
}