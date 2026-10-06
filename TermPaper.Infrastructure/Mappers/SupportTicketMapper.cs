using TermPaper.Application.Dto.SupportTicketDto;
using TermPaper.Infrastructure.Identity;

namespace TermPaper.Infrastructure.Mappers;

public static class SupportTicketMapper
{
    public static SupportTicketDto ToTicket(
        CreateTicketDto form, AppUser user, IList<string> roles,
        string? positionTitle, List<string> adminEmails)
    {
        var role = roles.Count > 0 ? string.Join(", ", roles) : "User";

        return new SupportTicketDto
        {
            ReportedBy = $"{user.UserName} ({role})",
            Position = positionTitle ?? string.Empty,
            Link = form.Link ?? string.Empty,
            Priority = form.Priority,
            Summary = form.Summary,
            AdminEmails = adminEmails
        };
    }
}