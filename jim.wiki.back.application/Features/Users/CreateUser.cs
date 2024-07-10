using jim.wiki.back.model.Models.Users;
using jim.wiki.core.Pipelines.Abstrantions;
using jim.wiki.core.Repository.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace jim.wiki.back.application.Features.Users;

public class CreateUserRequest:IRequest<CreateUserResponse>,ITransactionalRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public IList<Guid> Roles { get; set; }
}

public class CreateUserResponse
{
   public long UserId { get; set; }
   public Guid Guid { get; set; }
}

public class CreateUserHandler : IRequestHandler<CreateUserRequest, CreateUserResponse>
{
    private readonly IRepositoryBase<User> _userRepository;
    private readonly IRepositoryBase<Rol> _rolRepository;

    public CreateUserHandler( IRepositoryBase<User> userRepository, IRepositoryBase<Rol> rolRepository)
    {
        this._userRepository = userRepository;
        this._rolRepository = rolRepository;
    }
    public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var entity = new User()
        {
            Name = request.Name,
            Email = request.Email,
        };

        var rols = await _rolRepository.Query().Where(r => request.Roles.Any(a => a == r.Guid)).ToListAsync();


        foreach (var rol in rols)
        {
            entity.RolesUser.Add(new UserRole() { RolGuid = rol.Guid});
        }

        await _userRepository.AddAsync(entity);

        await _userRepository.SaveChangesAsync();

        var response = new CreateUserResponse() { Guid = entity.Guid, UserId = entity.Id };

        return response;
    }
}
