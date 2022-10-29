using Application.Common;
using Application.Common.Exceptions;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Users.Commands.ChangeUserRole;

public sealed class ChangeUserRoleCommandHandler : IRequestHandler<ChangeUserRoleCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public ChangeUserRoleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _userRepository = _unitOfWork.Users;
    }

    public async Task<Unit> Handle(ChangeUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username)
                   ?? throw new NotFoundException($"User not found for username = {request.Username}");

        user.Role = request.Role.OrElseThrow();
        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}