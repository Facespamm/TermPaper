using TermPaper.Application.Common;
using TermPaper.Application.Dto.PositionAccessRuleDto;
using TermPaper.Application.Dto.PositionAttributeDto;
using TermPaper.Application.Dto.PositionDto;
using TermPaper.Application.Dto.PositionProjectTagDto;
using TermPaper.Enum;

namespace TermPaper.Application.Interface;

public interface IPositionService
{
    //Position
    public Task<List<GetPositionDto>> GetPositions(string search);
    
    public Task<GetPositionDto?> GetPositionInfo(int positionId);
    
    public Task<Result> CreatePosition(CreatePositionDto createPositionDto);
    
    public Task<Result> UpdatePosition(UpdatePositionDto dto);
    
    public Task<Result> DeletePosition(List<int> positionId);
    
    //PositionTag
    
    public Task<List<GetPositionProjectTagDto>> GetPositionProjectTag(int positionId);
    
    public Task<Result> CreatePositionProjectTag(CreatePositionProjectTagDto dto);
    
    public Task<Result> DeletePositionProjectTag(List<int> positionId);
    
    //PositionAttribute
    
    public Task<List<GetPositionAttributeDto>> GetPositionAttribute(int positionId);
    
    public Task<Result> CreatePositionAttribute(CreatePositionAttributeDto dto);
    
    public Task<Result> DeletePositionAttribute(List<int> positionId);
    
    //PositionAccessRule
    
    public Task<List<Operator>> GetOperators();
    
    public Task<Result> CreatePositionAccessRule(CreatePositionAccessRuleDto dto);
    
    public Task<List<GetPositionAccessRuleDto>> GetPositionAccessRules(int positionId);
    
}