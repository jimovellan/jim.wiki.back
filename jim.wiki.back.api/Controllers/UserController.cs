using jim.wiki.back.application.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace jim.wiki.back.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController:ControllerBase
    {
        private readonly ISender sender;

        public UserController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost]
        public async Task<ActionResult<CreateUserResponse>> CreateUser(CreateUserRequest createUserRequest)
        {
            return await sender.Send(createUserRequest);
        }


        [HttpGet]
        public async Task<ActionResult<GetUserAllResponse>> GetAll()
        {
            return await sender.Send(new GetUserAllRequest());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteUserResponse>> CreateUser(Guid id)
        {
            return await sender.Send(new DeleteUserRequest() { Guid = id});
        }
    }
}
