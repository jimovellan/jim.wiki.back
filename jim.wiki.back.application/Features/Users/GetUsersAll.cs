using jim.wiki.back.application.Features.Users.Dto;
using jim.wiki.back.model.Models.Users;
using jim.wiki.core.Pipelines.Abstrantions;
using jim.wiki.core.Repository.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace jim.wiki.back.application.Features.Users;

public class GetUserAllRequest : IRequest<GetUserAllResponse>, IAuditableRequest
{
}

public class GetUserAllResponse
{
    public List<UserDto> Users { get; set; } = new List<UserDto>();
}

public class GetUserAllHandler : IRequestHandler<GetUserAllRequest, GetUserAllResponse>
{
    private readonly IRepositoryBase<User> _userRepository;

    public GetUserAllHandler(IRepositoryBase<User> userRepository)
    {
        this._userRepository = userRepository;
    }
    public async Task<GetUserAllResponse> Handle(GetUserAllRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.Query().Include(i => i.RolesUser).ThenInclude(i=>i.Rol)
            .ToListAsync();

        var usersDto = user.Select(s => new UserDto()
        {
            Name = s.Name,
            Guid = s.Guid,
            Roles = s.RolesUser.Select(s => new RolDto() { Guid = s.Rol.Guid, Name = s.Rol.Name, Descrition = s.Rol.Description }).ToList()
        }).ToList();

        return new GetUserAllResponse() { Users = usersDto };
    }
}
