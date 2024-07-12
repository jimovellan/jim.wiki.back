using jim.wiki.back.model.Models.Users;
using jim.wiki.core.Repository.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace jim.wiki.back.application.Features.Users;

public class AddUserRolRequest:IRequest<AddUserRolResponse>
{
    public Guid UserId { get; set; }
    public Guid RolId { get; set; }
}

public class AddUserRolResponse
{
    public bool Success { get; set; }
}

public class AddUserRolHandler : IRequestHandler<AddUserRolRequest, AddUserRolResponse>
{
    
    
    private readonly IRepositoryBase<User> userRepository;
    private readonly IRepositoryBase<Rol> rolRepository;

    public AddUserRolHandler( IRepositoryBase<User> userRepository, IRepositoryBase<Rol> rolRepository )
    {
        
        this.userRepository = userRepository;
        this.rolRepository = rolRepository;
    }
    public async Task<AddUserRolResponse> Handle(AddUserRolRequest request, CancellationToken cancellationToken)
    {

        

        var userRol = new UserRole() { RolGuid = request.RolId, UserGuid = request.UserId };

        

        return new AddUserRolResponse() { Success = true };

    }
}
