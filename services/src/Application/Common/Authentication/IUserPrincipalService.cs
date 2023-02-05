namespace Application.Common.Authentication;

public interface IUserPrincipalService
{
    string? GetUserName();
    int? GetUserPrincipalId();
}