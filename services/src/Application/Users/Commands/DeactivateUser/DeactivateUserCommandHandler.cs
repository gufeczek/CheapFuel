using Application.Common.Authentication;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Users.Commands.DeactivateUser;

public class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IUserPrincipalService _userPrincipalService;

    public DeactivateUserCommandHandler(IUnitOfWork unitOfWork, IUserPrincipalService userPrincipalService)
    {

    }
    
    public async Task<Unit> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        return Unit.Value;
    }
}