using TermPaper.Application.Common;
using TermPaper.Application.Dto;
using TermPaper.Enum;

namespace TermPaper.Application.Interface;

public interface IAttributeService
{
    public Task<Result> CreateCategory(AttributeCreateCategoryDto dto);
    
    public Task<AttributeGetCategoryDto> GetCategory();
    
    public Task<Result> UpdateCategory(int categoryId);
    
    public Task<Result> DeleteCategory(int categoryId);
    
    public Task<Result> CreateAttribute(int categoryId, string name, DataType dataType, string description,List<string> values);
    
    public Task<AttributeGetDto> GetAttribute();
    
    public Task<Result> UpdateAttribute();
    
    public Task<Result> DeleteAttribute();
    
    public Task<Result> AddAttributeValue();
    
    public Task<Result> GetEnumValue();
    
    
    
}