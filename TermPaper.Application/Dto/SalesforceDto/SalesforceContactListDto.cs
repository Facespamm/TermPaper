using System.Text.Json.Serialization;

namespace TermPaper.Application.Dto.SalesforceDto;

public class SalesforceContactListDto
{
    [JsonPropertyName("records")]
    public List<SalesforceContactRecordDto> Record { get; set; } = new();
}