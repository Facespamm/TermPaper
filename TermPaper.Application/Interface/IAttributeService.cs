using TermPaper.Application.Common;

namespace TermPaper.Application.Interface;

public interface IAttributeService
{
    public Task<Result> CreateCategory();
    
    public Task<Result> GetCategory();
    
    public Task<Result> UpdateCategory();
    
    public Task<Result> DeleteCategory();
    
    public Task<Result> CreateAttribute();
    
    public Task<Result> GetAttribute();
    
    public Task<Result> UpdateAttribute();
    
    public Task<Result> DeleteAttribute();
    
    public Task<Result> AddEnumValue();
    
    public Task<Result> GetEnumValue();
    
    
    
}