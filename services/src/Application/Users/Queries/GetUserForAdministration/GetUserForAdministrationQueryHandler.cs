using Application.Common.Exceptions;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Users.Queries.GetUserForAdministration;

public class GetUserForAdministrationQueryHandler : IRequestHandler<GetUserForAdministrationQuery, UserDetailsDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserForAdministrationQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _userRepository = unitOfWork.Users;
        _mapper = mapper;
    }
    
    public async Task<UserDetailsDto> Handle(GetUserForAdministrationQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username) 
                   ?? throw new NotFoundException(nameof(User), nameof(User.Username), request.Username);

        return _mapper.Map<UserDetailsDto>(user);
    }
}