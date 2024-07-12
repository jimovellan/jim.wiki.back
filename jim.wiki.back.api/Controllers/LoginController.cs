using jim.wiki.back.application.Features.Users;
using jim.wiki.core.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace jim.wiki.back.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ISender sender;

        public LoginController(ISender sender)
        {
            
            this.sender = sender;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<Result<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
        {
            return await sender.Send(request, cancellationToken);
        }

        [HttpPost("RefreshToken")]
        [AllowAnonymous]
        public async Task<Result<RefreshTokenResponse>> RefreshToken(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            return await sender.Send(request, cancellationToken);
        }
    }
}
