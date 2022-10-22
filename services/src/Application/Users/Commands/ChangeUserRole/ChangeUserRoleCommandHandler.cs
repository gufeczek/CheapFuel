using Application.Common.Exceptions;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Users.Commands.ChangeUserRole;

public class ChangeUserRoleCommandHandler : IRequestHandler<ChangeUserRoleCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public ChangeUserRoleCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(ChangeUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username)
                   ?? throw new NotFoundException($"User not found for username = {request.Username}");

        user.Role = request.Role;
        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}