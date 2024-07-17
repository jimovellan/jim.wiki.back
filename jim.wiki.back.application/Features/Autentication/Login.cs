using FluentValidation;
using jim.wiki.back.application.Features.Autentication.Errors;
using jim.wiki.back.model.Models.Users;
using jim.wiki.back.model.Services;
using jim.wiki.core.Authentication.Interfaces;
using jim.wiki.core.Authentication.Models;
using jim.wiki.core.Errors;
using jim.wiki.core.Pipelines.Abstrantions;
using jim.wiki.core.Repository.Interfaces;
using jim.wiki.core.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace jim.wiki.back.application.Features.Users;

public class LoginRequest:IRequest<Result<LoginResponse>>,ITransactionalRequest
{
    public string Name { get; set; }
   
    public string Password { get; set; }

}

public class LoginResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}

public class LoginRequestValidation: AbstractValidator<LoginRequest>
{
    public LoginRequestValidation()
    {
        RuleFor(x=>x.Name).NotNull().NotEmpty();
        RuleFor(x=>x.Password).NotNull().NotEmpty();
    }
}

public class LoginHandler : IRequestHandler<LoginRequest, Result<LoginResponse>>
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
    public async Task<Result<LoginResponse>> Handle(LoginRequest request, CancellationToken cancellationToken)
    {


        return await SearchUserAndValidatePassword(request, cancellationToken)
                     .ThenAsync(GenerateNewTokenAndRefreshTokenAndSave);

    }

    /// <summary>
    /// Busca el usuario y valida su password
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<User>> SearchUserAndValidatePassword(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository
                    .Query()
                    .Include(x => x.Tokens)
                    .FirstOrDefaultAsync(f => f.Name == request.Name);

        if (user is null)
        {
            return Result.Fail<User>(AuthenticationErrors.UserNotFound);
        }

        if (!_passwordService.Valid(request.Password, user.Hash))
        {
            
                return Result.Fail<User>(AuthenticationErrors.LoginError);
            
        }

        return user;

    }


    /// <summary>
    /// Genera Token y refresh token y lo guarda
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<LoginResponse>> GenerateNewTokenAndRefreshTokenAndSave(User user, CancellationToken cancellationToken)
    {
        var token = _dataService.GetToken(new UserData() { Email = user.Email, Name = user.Email });

        var refreshToken = user.GenerateNewRefreshToken(token);

        await _userRepository.SaveChangesAsync(cancellationToken);

        var response = new LoginResponse() { Token = token, RefreshToken = refreshToken };

        return response;
    }


}
