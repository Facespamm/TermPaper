using System.Text.Json.Serialization;
using TermPaper.Domain.Enum;

namespace TermPaper.Application.Dto.SupportTicketDto;

public class SupportTicketDto
{
    [JsonPropertyName("reportedBy")]
    public string ReportedBy { get; set; } = string.Empty;

    [JsonPropertyName("position")]
    public string Position { get; set; } = string.Empty;

    [JsonPropertyName("link")]
    public string Link { get; set; } = string.Empty;

    [JsonPropertyName("priority")]
    public string Priority { get; set; } = string.Empty;

    [JsonPropertyName("summary")]
    public string Summary { get; set; } = string.Empty;

    [JsonPropertyName("adminEmails")]
    public List<string> AdminEmails { get; set; } = new();
}