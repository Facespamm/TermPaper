using TermPaper.Application.Common;
using TermPaper.Application.Dto.PositionDto;

namespace TermPaper.Application.Interface;

public interface IPositionService
{
    public Task<List<GetPositionDto>> GetPositions(string search);
    
    public Task<GetPositionDto?> GetPositionInfo(int positionId);
    
    public Task<Result> CreatePosition(CreatePositionDto createPositionDto);
    
    
    
}