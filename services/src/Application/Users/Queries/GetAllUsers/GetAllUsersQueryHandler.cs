using Application.Common;
using Application.Models;
using AutoMapper;
using Domain.Common.Pagination.Response;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Users.Queries.GetAllUsers;

public sealed class GetAllUserQueryHandler : IRequestHandler<GetAllUsersQuery, Page<UserDetailsDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetAllUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _userRepository = unitOfWork.Users;
        _mapper = mapper;
    }
    
    public async Task<Page<UserDetailsDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var pageRequest = PaginationHelper.Eval(request.PageRequestDto, new UserDetailColumnSelector());
        var filter = request.Filter;
        var result = await _userRepository.GetAllAsync(filter.Role, filter.AccountStatus, filter.SearchPhrase, pageRequest);
        return Page<UserDetailsDto>.From(result, _mapper.Map<IEnumerable<UserDetailsDto>>(result.Data));
    }
}