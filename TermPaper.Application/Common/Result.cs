using TermPaper.Enum;

namespace TermPaper.Application.Common;

public class Result
{
    public bool IsSuccess { get; }
    public ErrorCode ErrorCode { get; }
    public string? ErrorMessage { get; }

    public Result(bool isSuccess, ErrorCode errorCode, string? errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }
    
    public static Result Success() => new (true,ErrorCode.None,null);
    public static Result Failure(ErrorCode errorCode) => new (false,errorCode,null);
    
}

