using System.ComponentModel.DataAnnotations;

namespace TermPaper.Application.Dto.SalesforceDto;

public class SalesforceAccountCreateDto
{
    [Required,StringLength(255)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required,StringLength(255)]
    public string LastName { get; set; } = string.Empty;
    
    [Required, StringLength(255)]
    public string CompanyName { get; set; } = string.Empty;
    
    [Phone, StringLength(40)]
    public string? Phone { get; set; } = string.Empty;
    
    [Url,StringLength(255)]
    public string? Website { get; set; } = string.Empty;
    
    public string? Industry { get; set; } = string.Empty;
}