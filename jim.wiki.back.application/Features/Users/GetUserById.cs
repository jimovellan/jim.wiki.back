using jim.wiki.back.model.Models.Users;
using jim.wiki.core.Pipelines.Abstrantions;
using jim.wiki.core.Repository.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.back.application.Features.Users;
    public class GetUserByIdRequest: IRequest<GetUserByIdResponse> , IAuditableRequest
    {
    public long Id { get; set; }
}

    public class GetUserByIdResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

public class GetUserByIdHandler : IRequestHandler<GetUserByIdRequest, GetUserByIdResponse>
{
    private readonly IRepositoryBase<User> _userRepository;

    public GetUserByIdHandler(IRepositoryBase<User> userRepository)
    {
        this._userRepository = userRepository;
    }
    public async Task<GetUserByIdResponse> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetById(request.Id);

        if (user == null)
        {
            return null;
        }

        return new GetUserByIdResponse { Id = request.Id, Email = user.Email, Name = user.Name };
    }


}