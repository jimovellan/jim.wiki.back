using jim.wiki.back.model.Models.Users;
using jim.wiki.back.model.Services;
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
    private readonly IPasswordService _passwordService;

    public CreateUserHandler( IRepositoryBase<User> userRepository, 
                              IRepositoryBase<Rol> rolRepository,
                              IPasswordService passwordService)
    {
        this._userRepository = userRepository;
        this._rolRepository = rolRepository;
        this._passwordService = passwordService;
    }
    public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        

       


        if(await _userRepository.Query().Include(i=>i.Tokens).AnyAsync(x => EF.Functions.Like(x.Name,request.Name)))
        {
            throw new Exception("Usuario existente");
        }

        var hash = _passwordService.GenerateHash(request.Password);
        
        var entity = User.Create(request.Name, new model.Models.ObjectsValue.Email(request.Email), hash);
        

        

        await _userRepository.AddAsync(entity);

        await _userRepository.SaveChangesAsync();

        var response = new CreateUserResponse() { Guid = entity.Guid, UserId = entity.Id };

        return response;
    }
}
