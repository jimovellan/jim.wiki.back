using jim.wiki.back.model.Models.Users;
using jim.wiki.back.model.Services;
using jim.wiki.core.Authentication.Interfaces;
using jim.wiki.core.Pipelines.Abstrantions;
using jim.wiki.core.Repository.Interfaces;
using jim.wiki.core.Authentication.Models;
using MediatR;

namespace jim.wiki.back.application.Features.Users;

public class LoginRequest:IRequest<LoginResponse>,ITransactionalRequest
{
    public string Name { get; set; }
   
    public string Password { get; set; }

}

public class LoginResponse
{
    public string Token { get; set; }
}

public class LoginHandler : IRequestHandler<LoginRequest, LoginResponse>
{
    private readonly IRepositoryBase<User> _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IUserDataService _dataService;

    public LoginHandler( IRepositoryBase<User> userRepository, 
                              IPasswordService passwordService,
                              IUserDataService dataService)
    {
        this._userRepository = userRepository;
        this._passwordService = passwordService;
        this._dataService = dataService;
    }
    public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        

        var user = _userRepository.Query().FirstOrDefault(f=>f.Name == request.Name);

        if(user is null) throw new UnauthorizedAccessException();
        
        if(!_passwordService.Valid(request.Password, user.Hash)) throw new UnauthorizedAccessException();

       

        var response = new LoginResponse() { Token = _dataService.GetToken(new UserData() { Email = user.Email, Name = user.Email})};

        return response;
    }
}
