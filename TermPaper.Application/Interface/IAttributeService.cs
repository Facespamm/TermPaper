using TermPaper.Application.Common;
using TermPaper.Application.Dto;
using TermPaper.Application.Dto.AttributeValueDto;
using TermPaper.Application.Dto.UsersAttributesDto;
using TermPaper.Enum;

namespace TermPaper.Application.Interface;

public interface IAttributeService
{
    // Attribute
    public Task<List<AttributeGetDto>> GetAttribute();

    public Task<Result> CreateAttribute(CreateAttributeDto dto);

    public Task<Result> UpdateAttribute(UpdateAttributeDto dto);

    public Task<Result> DeleteAttribute(List<int> attributeIds);

    public Task<List<AttributeGetDto>> SearchByPrefixAsync(string prefix);

    public Task<List<AttributeGetDto>> GetByCategoryAsync(int categoryId);
    public Task<List<AttributeGetDto>> GetBuiltInAttributes();
    
    public Task<List<AttributeGetDto>> GetRecentlyUsedAsync(string userId);

    public Task<List<GetUserAttributeDto>> GetUserBuiltInAttributeValuesAsync(string userId);


    // Attribute Category
    public Task<Result> CreateCategory(AttributeCreateCategoryDto dto);

    public Task<List<AttributeGetCategoryDto>> GetCategory();

    public Task<Result> UpdateAttributeCategory(AttributeUpdateCategoryDto dto);

    public Task<Result> DeleteAttributeCategory(List<int> attributeCategoryIds);

    // Attribute Value Option
    public Task<Result> AddAttributeValueOption(AttributeValueAddDto addDto);

    public Task<Result> UpdateAttributeValue(UpdateAttributeValueDto dto);

    public Task<Result> DeleteAttributeValueOption(List<int> attributeValueOptionIds);

    public Task<List<GetUserAttributeDto>> GetUserAttributeValuesAsync(string userId);
    
    // User Attribute
    public Task<Result> AddToUsersValue(UserAttributesAddDto addDto);

    public Task<Result> UpdateUserAttribute(UpdateUserAttributeDto dto);

    public Task<Result> DeleteUserAttribute(List<int> attributeIds);

    Task<List<AttributeValueOptionDto>> GetOptionValuesAsync(int attributeId);}