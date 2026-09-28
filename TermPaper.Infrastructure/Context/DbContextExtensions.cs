using Microsoft.EntityFrameworkCore;
using TermPaper.Application.Common;
using TermPaper.Domain.Enum;

namespace TermPaper.Infrastructure.Context;

public static class DbContextExtensions
{
    public static async Task<Result> SaveWithConcurrencyAsync(this AppDbContext context)
    {
        try
        {
            await context.SaveChangesAsync();
            return Result.Success();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Result.Failure(ErrorCode.ConcurrencyConflict);
        }
    }
}