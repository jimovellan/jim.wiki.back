using jim.wiki.back.application.Features.Users;
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

        [HttpPost]
        [AllowAnonymous]
        public async Task<LoginResponse> Login(LoginRequest request, CancellationToken cancellationToken)
        {
            return await sender.Send(request, cancellationToken);
        }
    }
}
