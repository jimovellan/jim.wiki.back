using jim.wiki.back.model.Models.Users;
using jim.wiki.core.Authentication.Interfaces;
using jim.wiki.core.Authentication.Models;
using jim.wiki.core.Errors;
using jim.wiki.core.Pipelines.Abstrantions;
using jim.wiki.core.Repository.Interfaces;
using jim.wiki.core.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace jim.wiki.back.application.Features.Users;

public class RefreshTokenRequest:IRequest<Result<RefreshTokenResponse>>,ITransactionalRequest
{
    public string Name { get; set; }
   
    public string Token { get; set; }
    public string RefreshToken { get; set; }

}

public class RefreshTokenResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}

public class RefreshTokenHandler : IRequestHandler<RefreshTokenRequest, Result<RefreshTokenResponse>>
{
    private readonly IRepositoryBase<User> _userRepository;
    private readonly IUserDataService userDataService;

    public RefreshTokenHandler( IRepositoryBase<User> userRepository,
        IUserDataService userDataService
                              )
    {
        this._userRepository = userRepository;
        this.userDataService = userDataService;
    }
    public async Task<Result<RefreshTokenResponse>> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
    {

        return await SearchUserAndValidTokenAndRefreshToken(request, cancellationToken)
                     .ThenAsync(GenerateNewTokenAndRefreshToken);
    }

    public async Task<Result<User>> SearchUserAndValidTokenAndRefreshToken(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository
                    .Query()
                    .Include(x => x.Tokens)
                    .FirstOrDefaultAsync(f => f.Name == request.Name,cancellationToken);

        if (user is null)
        {
            return Result.Fail<User>(new Error[] { new Error("User_NotFound", "The user has not found") },cancellationToken);
        }

        if (!user.IsRefreshTokenValid(request.Token, request.RefreshToken))
        {
            return Result.Fail<User>(new Error[] { new Error("RefreshToken_Error", "The operation has failed") }, cancellationToken);
        }

        return user;

    }

    public async Task<Result<RefreshTokenResponse>> GenerateNewTokenAndRefreshToken(User user, CancellationToken cancellationToken)
    {
        var token = userDataService.GetToken(new UserData() { Email = user.Email, Name = user.Email });

        var refreshToken = user.GenerateNewRefreshToken(token);

        await _userRepository.SaveChangesAsync(cancellationToken);

        var response = new RefreshTokenResponse() { Token = token, RefreshToken = refreshToken };

        return response;
    }
}
