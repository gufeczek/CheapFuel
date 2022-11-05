using MediatR;

namespace Application.Users.Commands.ChangePassword;

public sealed record ChangePasswordCommand(
    string OldPassword, 
    string NewPassword, 
    string ConfirmNewPassword) 
    : IRequest<Unit>;