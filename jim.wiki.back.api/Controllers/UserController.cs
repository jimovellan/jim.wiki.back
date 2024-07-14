using jim.wiki.back.application.Features.Users;
using jim.wiki.back.model.Models.Users;
using jim.wiki.core.Repository.Models.Search;
using jim.wiki.core.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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


        [HttpPost("Users")]
        [AllowAnonymous]
        public async Task<ResultSearch<User>> GetAll(FilterSearch filter)
        {
            return await sender.Send(new GetUserAllRequest() { Filter = filter});
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteUserResponse>> CreateUser(Guid id)
        {
            return await sender.Send(new DeleteUserRequest() { Guid = id});
        }

        
    }
}
