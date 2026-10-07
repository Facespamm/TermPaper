using TermPaper.Application.Dto.SalesforceDto;
using TermPaper.Infrastructure.Identity;

namespace TermPaper.Infrastructure.Mappers;

public class SalesforceMapper
{
    public static SalesforceCompositeRequestDto ToCompositeRequestDto(
        SalesforceAccountCreateDto dto, AppUser user, Dictionary<string, string?> profile)
    {
        string? Get(string key) =>
            profile.TryGetValue(key, out var value) && !string.IsNullOrWhiteSpace(value)
                ? value.Trim()
                : null;

        string? birthdate = DateTime.TryParse(Get("Date of Birth"), out var date)
            ? date.ToString("yyyy-MM-dd")
            : null;

        return new SalesforceCompositeRequestDto
        {
            Records = new List<SalesforceAccountRecordDto>
            {
                new SalesforceAccountRecordDto
                {
                    Name = dto.CompanyName,
                    Website = dto.Website,
                    Industry = dto.Industry,
                    Contacts = new SalesforceContactListDto
                    {
                        Record = new List<SalesforceContactRecordDto>
                        {
                            new SalesforceContactRecordDto
                            {
                                FirstName = Get("First Name") ?? string.Empty,
                                LastName = Get("Last Name") ?? user.UserName ?? "Unknown",
                                Email = user.Email!,
                                Phone = Get("Phone"),
                                Birthdate = birthdate,
                                MailingCity = Get("Location"),
                                Description = Get("About Me")
                            }
                        }
                    }
                }
            }
        };
    }
}