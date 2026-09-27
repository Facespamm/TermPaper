namespace TermPaper.Domain.Enum;

public enum ErrorCode
{
    None,
    PasswordsDoNotMatch,
    UserAlreadyExists,
    UserNotFound,
    EmailNotConfirmed,
    InvalidCredentials,

    NotFound,
    AlreadyExists,
    ValidationFailed,
    AccessDenied,
    ConcurrencyConflict   
}