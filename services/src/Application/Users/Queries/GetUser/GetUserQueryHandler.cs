using Application.Common.Exceptions;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Users.Queries.GetUser;

public sealed class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public GetUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByUsernameAsync(request.Username) 
                                    ?? throw new NotFoundException(nameof(User), nameof(User.Username), request.Username);

        return _mapper.Map<UserDto>(user);
    }
}