using TermPaper.Enum;

namespace TermPaper.Domain.Models;

public class Result
{
    private bool IsSuccess { get; }
    private ErrorCode ErrorCode { get; }
    private string? ErrorMessage { get; }

    public Result(bool isSuccess, ErrorCode errorCode, string? errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }
    
    public static Result Success() => new (true,ErrorCode.None,null);
    public static Result Failure(ErrorCode errorCode) => new (false,errorCode,null);
    
}

