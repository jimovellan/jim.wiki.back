using jim.wiki.back.application.Features.Users.Dto;
using jim.wiki.back.application.Services;
using jim.wiki.back.model.Models.Users;
using jim.wiki.core.Pipelines.Abstrantions;
using jim.wiki.core.Repository.Interfaces;
using jim.wiki.core.Repository.Models.Search;
using jim.wiki.core.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection.Metadata.Ecma335;

namespace jim.wiki.back.application.Features.Users;

public class GetUserAllRequest : IRequest<ResultSearch<UserDto>>
{
    public FilterSearch Filter { get; set; }
}

public class GetUserAllResponse
{
    public List<UserDto> Users { get; set; } = new List<UserDto>();
}

public class GetUserAllHandler : IRequestHandler<GetUserAllRequest, ResultSearch<UserDto>>
{
    private readonly IRepositoryBase<User> _userRepository;
    private readonly IMapperServices _mapper;

    public GetUserAllHandler(IRepositoryBase<User> userRepository, 
                             IMapperServices mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    public async Task<ResultSearch<UserDto>> Handle(GetUserAllRequest request, CancellationToken cancellationToken)
    {

        var query = _userRepository
                    .Query()
                    .Include(p => p.Rol)
                    .Select(s=> new { Id = s.Id, Guid = s.Guid, Name = s.Name, Email = s.Email, RolId = s.RolId, RolName = s.Rol.Name });
        return  await _userRepository.ApplyFilterToSearch(query, 
                                                          request.Filter, 
                                                          x=> new UserDto(x.Id, 
                                                                          x.Guid,
                                                                          x.Name,
                                                                          x.Email, 
                                                                          x.RolId, 
                                                                          x.RolName));
    }

    
}
