using Application.Common;
using Application.Models;
using AutoMapper;
using Domain.Common.Pagination.Response;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.BlockUser.Queries.GetAllBlockedUsers;

public class GetAllBlockedUsersQueryHandler : IRequestHandler<GetAllBlockedUsersQuery, Page<AllBlockedUsersDto>>
{
    private readonly IBlockUserRepository _blockUserRepository;
    private readonly IMapper _mapper;

    public GetAllBlockedUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _blockUserRepository = unitOfWork.BlockedUsers;
        _mapper = mapper;
    }
    
    public async Task<Page<AllBlockedUsersDto>> Handle(GetAllBlockedUsersQuery request, CancellationToken cancellationToken)
    {
        var pageRequest = PaginationHelper.Eval(request.PageRequestDto, new AllBlockedUsersDtoColumnSelector());
        var result = await _blockUserRepository.GetAllAsync(pageRequest);
        return Page<AllBlockedUsersDto>.From(result, _mapper.Map<IEnumerable<AllBlockedUsersDto>>(result.Data));
    }
}