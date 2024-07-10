using jim.wiki.back.application.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace jim.wiki.back.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly ISender sender;

        public RolController(ISender sender)
        {
            this.sender = sender;
        }
        [HttpPost]
        public async Task<CreateRolResponse> CreateRol(CreateRolRequest request, CancellationToken cancellationToken)
        {
            return await sender.Send(request, cancellationToken);
        }

        [HttpPost("AddUserRol")]
        public async Task<AddUserRolResponse> AddUserRol(AddUserRolRequest request, CancellationToken cancellation)
        {
            return await sender.Send(request, cancellation);
        }
    }
}
