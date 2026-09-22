namespace TermPaper.Application.Dto;

public class UserDto
{
    public string Id { get; set; } = "";
    public string? Email { get; set; } = "";
    public string? UserName { get; set; } = "";
    public List<string> Roles { get; set; } = new();
    public bool EmailConfirmed { get; set; }
    public bool IsLockedOut { get; set; }

}