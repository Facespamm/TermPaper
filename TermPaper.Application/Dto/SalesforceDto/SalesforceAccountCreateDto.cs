using System.ComponentModel.DataAnnotations;

namespace TermPaper.Application.Dto.SalesforceDto;

public class SalesforceAccountCreateDto
{
    [Required, StringLength(255)]
    public string CompanyName { get; set; } = string.Empty;

    [Url, StringLength(255)]
    public string? Website { get; set; }

    public string? Industry { get; set; }
}