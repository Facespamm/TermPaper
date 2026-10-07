using TermPaper.Application.Dto.SalesforceDto;
using TermPaper.Infrastructure.Identity;

namespace TermPaper.Infrastructure.Mappers;

public class SalesforceMapper
{
    public static SalesforceCompositeRequestDto ToCompositeRequestDto(SalesforceAccountCreateDto dto, AppUser user)
    {
        return new SalesforceCompositeRequestDto
        {
            Records = new List<SalesforceAccountRecordDto>
            {
                new SalesforceAccountRecordDto
                {
                    Name = dto.CompanyName,
                    Phone = dto.Phone,
                    Industry = dto.Industry,
                    Website = dto.Website,
                    Contacts = new SalesforceContactListDto
                    {
                        Record = new List<SalesforceContactRecordDto>
                        {
                            new SalesforceContactRecordDto
                            {
                                FirstName = dto.FirstName,
                                LastName = dto.LastName,
                                Email = user.Email!
                            }
                        }
                    }
                }
            }
        };
    }
}