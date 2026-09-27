using TermPaper.Application.Dto.PositionAccessRuleDto;
using TermPaper.Domain.Models;

namespace TermPaper.Infrastructure.Mappers;

public class PositionAccessRuleMapper
{
    public static AccessRule ToEntity(CreatePositionAccessRuleDto dto)
    {
        return new AccessRule()
        {
            AttributeId = dto.AttributeId,
            Operator = dto.Operator,
            Value = dto.Value,
            PositionId = dto.PositionId,
        };
    }

    public static GetPositionAccessRuleDto ToDto(AccessRule accessRule)
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