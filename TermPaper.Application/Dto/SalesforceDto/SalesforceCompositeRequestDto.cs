using System.Text.Json.Serialization;

namespace TermPaper.Application.Dto.SalesforceDto;

public class SalesforceCompositeRequestDto
{
    [JsonPropertyName("records")]
    public List<SalesforceAccountRecordDto> Records { get; set; } = new();
}