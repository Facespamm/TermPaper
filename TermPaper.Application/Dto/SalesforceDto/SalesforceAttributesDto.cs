using System.Text.Json.Serialization;

namespace TermPaper.Application.Dto.SalesforceDto;

public class SalesforceAttributesDto
{
    [JsonPropertyName("type")] public string Type { get; set; } = string.Empty;

    [JsonPropertyName("reference_id")] public string ReferenceId { get; set; } = string.Empty;
}
