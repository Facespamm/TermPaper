using System.ComponentModel.DataAnnotations;

namespace TermPaper.Application.Dto.SupportTicketDto;

public class CreateTicketDto
{
    [Required, StringLength(1000)]
    public string Summary { get; set; } = string.Empty;

    [Required]
    public string Priority { get; set; } = string.Empty;  

    public string? Link { get; set; }
    public int? PositionId { get; set; }
}