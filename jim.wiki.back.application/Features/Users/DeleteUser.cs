using jim.wiki.back.model.Models.Users;
using jim.wiki.core.Pipelines.Abstrantions;
using jim.wiki.core.Repository.Interfaces;
using MediatR;

namespace jim.wiki.back.application.Features.Users;

public class DeleteUserRequest:IRequest<DeleteUserResponse>,IAuditableRequest
{
    public Guid Guid { get; set; }
    
}

public class DeleteUserResponse
{
    public bool Success { get; set; }
}

public class DeleteUserHandler : IRequestHandler<DeleteUserRequest, DeleteUserResponse>
{
    private readonly IRepositoryBase<User> _userRepository;
    private readonly ISender _sender;

    public DeleteUserHandler( IRepositoryBase<User> userRepository, 
                              ISender sender)
    {
        this._userRepository = userRepository;
        this._sender = sender;
    }
    public async Task<DeleteUserResponse> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        var user = _userRepository.Query().FirstOrDefault(f => f.Guid == request.Guid);

        if (user == null) throw new Exception("No encontrado");

        

        await _userRepository.DeleteAsync(user);

        var results = await _userRepository.SaveChangesAsync();


        return new DeleteUserResponse { Success = results > 0 };
    }
}
