using System.Text.Json.Serialization;

namespace TermPaper.Application.Dto.SalesforceDto;

public class SalesforceAccountRecordDto
{
    [JsonPropertyName("attributes")]
    public SalesforceAttributesDto Attributes { get; set; } = new() { Type = "Account", ReferenceId = "acc1" };
    
    [JsonPropertyName("Name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("Phone")]
    public string Phone { get; set; } = string.Empty;
    
    [JsonPropertyName("Industry")]
    public string Industry { get; set; } = string.Empty;

    [JsonPropertyName("Contacts")] 
    public SalesforceContactListDto Contacts { get; set; } = new();
    
    [JsonPropertyName("Website")]
    public string Website { get; set; } = string.Empty;

}