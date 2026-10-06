using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TermPaper.Domain.Enum;

namespace TermPaper.Application.Dto.SupportTicketDto;

public class CreateTicketDto
{
    [Required, StringLength(1000)]
    public string Summary { get; set; } = string.Empty;
    [Required]
    public SupportTicketPriority Priority { get; set; }

    public string? Link { get; set; }
    public int? PositionId { get; set; }
}