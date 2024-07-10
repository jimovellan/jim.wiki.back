using jim.wiki.core.Auditory.Models;
using jim.wiki.core.Auditory.Repository.Interfaces;
using jim.wiki.core.Authentication.Interfaces;
using jim.wiki.core.Pipelines.Abstrantions;
using jim.wiki.core.Repository.Interfaces;
using MediatR;
using System.Text.Json;

namespace jim.wiki.core.Pipelines.Behaviors;

public class AuditablePipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditRepository _auditRepository;
    private readonly IUserDataService userDataService;

    public AuditablePipelineBehavior(IUnitOfWork unitOfWork,
                                     IAuditRepository auditRepository,
                                     IUserDataService userDataService)
    {
        this._unitOfWork = unitOfWork;
        this._auditRepository = auditRepository;
        this.userDataService = userDataService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {

        

        var isAuditable = request.GetType().IsAssignableTo(typeof(IAuditableRequest));
        try
        {
           

            if (isAuditable)
            {
                var type = typeof(TRequest);

                var audit = new Audit()
                {
                    Method = type.Name,
                    CreatedAt = DateTime.UtcNow,
                    Ip = userDataService!.GetUser()!.IP!,
                    User = userDataService!.GetUser()!.Name!,
                    Parameters = JsonSerializer.Serialize(request),
                    Guid = _auditRepository.Guid
                };

               
                await _auditRepository.Save(audit);
               
               

                var resul = await next();

               

                return resul;
            }
            else
            {
                return await next();
            }

           
        }
        catch (Exception ex)
        {
            
            throw ex;
        }
        
    }

   
}
