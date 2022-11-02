using Application.Common.Authentication;
using Application.Common.Interfaces;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Users.Commands.GeneratePasswordResettingToken;

public class GeneratePasswordResettingTokenHandler : IRequestHandler<GeneratePasswordResettingTokenCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IEmailSenderService _emailSenderService;

    public GeneratePasswordResettingTokenHandler(
        IUnitOfWork unitOfWork, 
        ITokenService tokenService, 
        IEmailSenderService emailSenderService)
    {
        _unitOfWork = unitOfWork;
        _userRepository = unitOfWork.Users;
        _tokenService = tokenService;
        _emailSenderService = emailSenderService;
    }

    public Task<Unit> Handle(GeneratePasswordResettingTokenCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}