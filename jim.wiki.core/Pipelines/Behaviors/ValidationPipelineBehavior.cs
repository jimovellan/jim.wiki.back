using FluentValidation;
using jim.wiki.core.Exceptions;
using jim.wiki.core.Extensions;
using MediatR;

namespace jim.wiki.core.Pipelines.Behaviors;

public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> validators;

    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        this.validators = validators;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationFailures = await Task.WhenAll(validators.Select(validators => validators.ValidateAsync(context)));

        var errors = validationFailures
                     .Where(validationresult => !validationresult.IsValid)
                     .SelectMany(validationResult => validationResult.Errors)
                     .Select(s => new Errors.Error(s.PropertyName, s.ErrorMessage))
                     .ToList();

        if (errors.ContainElements())
        {
            throw new CustomDomainValidationException(errors.ToArray());
        }

        return await next();
    }
}
