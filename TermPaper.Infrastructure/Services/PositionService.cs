using Microsoft.EntityFrameworkCore;
using TermPaper.Application.Common;
using TermPaper.Application.Dto.PositionDto;
using TermPaper.Infrastructure.Context;
using TermPaper.Infrastructure.Mappers;

namespace TermPaper.Infrastructure.Services;

public class PositionService
{
    private readonly AppDbContext _context;
    
    public PositionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<GetPositionDto>> GetPositions(string search)
    {
        var records = await _context.Positions.Where(x => x.Title == search || x.ShortDescription == search)
            .ToListAsync();
        List<GetPositionDto> listDto  = new(); 
        foreach (var record in records)
        {
            listDto.Add(PositionMapper.GetToPosition(record));
        }
        return listDto;
    }

    public async Task<GetPositionInfoDto> GetPositionInfo(int id)
    {
        var info = await _context.Positions.FirstOrDefaultAsync(x => x.Id == id);
        if (info == null)
        {
            return new GetPositionInfoDto();
        }
        return PositionMapper.GetPositionToInfo(info);
    }

    public async Task<Result> CreatePosition(CreatePositionDto dto)
    {
        var entity = PositionMapper.CreateToPosition(dto);
        
        await _context.Positions.AddAsync(entity);
        await _context.SaveChangesAsync();
        return Result.Success();
    }
}