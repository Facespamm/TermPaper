using System.Text.Json.Serialization;

namespace TermPaper.Application.Dto.SalesforceDto;

public class SalesforceContactRecordDto
{
    [JsonPropertyName("attributes")]
    public SalesforceAttributesDto Attributes { get; set; } = new() { Type = "Contact", ReferenceId = "con1" };

    [JsonPropertyName("FirstName")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("LastName")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("Email")]
    public string Email { get; set; } = string.Empty;
}