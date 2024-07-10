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

public class CreateRolRequest: IRequest<CreateRolResponse>,IAuditableRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
}

public class CreateRolResponse
{
    public long Id { get; set; }

    public Guid Guid { get; set; }
}

public class CreateRolHandler : IRequestHandler<CreateRolRequest, CreateRolResponse>
{
    public IRepositoryBase<Rol> _rolRepository { get; set; }

    public CreateRolHandler(IRepositoryBase<Rol> rolRepository)
    {
        _rolRepository = rolRepository;
    }
    public async Task<CreateRolResponse> Handle(CreateRolRequest request, CancellationToken cancellationToken)
    {
        var rol = new Rol() { Description = request.Description, Name = request.Name };

        await _rolRepository.AddAndSaveAsync(rol);

        return new CreateRolResponse() { Guid = rol.Guid, Id = rol.Id };
    }
}
