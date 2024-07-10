using jim.wiki.core.Pipelines.Abstrantions;
using jim.wiki.core.Repository.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.core.Pipelines.Behaviors;

public class TransactionalPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public TransactionalPipelineBehavior(IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var isTransactional = request.GetType().IsAssignableTo(typeof(ITransactionalRequest));
        try
        {

            if(isTransactional && !_unitOfWork.HasTransaction()) _unitOfWork.BeginTransaction();

            var result = await next();

            if (isTransactional)
            {
                _unitOfWork.Commit();
            }
            return result;
        }
        catch (Exception ex)
        {
            if(isTransactional)
            {
                _unitOfWork.Rollback();
            }
            throw ex;
        }
        
    }
}
