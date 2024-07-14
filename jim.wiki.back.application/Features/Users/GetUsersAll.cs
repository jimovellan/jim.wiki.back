using jim.wiki.back.application.Features.Users.Dto;
using jim.wiki.back.model.Models.Users;
using jim.wiki.core.Pipelines.Abstrantions;
using jim.wiki.core.Repository.Interfaces;
using jim.wiki.core.Repository.Models.Search;
using jim.wiki.core.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace jim.wiki.back.application.Features.Users;

public class GetUserAllRequest : IRequest<ResultSearch<User>>
{
    public FilterSearch Filter { get; set; }
}

public class GetUserAllResponse
{
    public List<UserDto> Users { get; set; } = new List<UserDto>();
}

public class GetUserAllHandler : IRequestHandler<GetUserAllRequest, ResultSearch<User>>
{
    private readonly IRepositoryBase<User> _userRepository;

    public GetUserAllHandler(IRepositoryBase<User> userRepository)
    {
        this._userRepository = userRepository;
    }
    public async Task<ResultSearch<User>> Handle(GetUserAllRequest request, CancellationToken cancellationToken)
    {
        return  await _userRepository.ApplyFilterToSearch(request.Filter);
    }
}
