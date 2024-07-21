using FluentAssertions;
using jim.wiki.back.application;
using jim.wiki.core.Results;
using MediatR;

namespace jim.wiki.back.infrastructure.test.Application
{
    public class Application_Handlers
    {
        [Fact]
        public void All_IRequestHandler_Implementations_Should_Return_ResultOfT()
        {
            // Get all types in the jim.wiki.back.infrastructure assembly
            var assembly = typeof(Startup).Assembly;

            // Get all types that implement IRequestHandler<>
            var requestHandlerTypes = assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)))
                .ToList();

            // Verify the return type of the Handle method
            foreach (var handlerType in requestHandlerTypes)
            {
                var handleMethod = handlerType.GetInterface("IRequestHandler`2")
                                              .GetMethod("Handle");

                var returnType = handleMethod.ReturnType;

                returnType.IsGenericType.Should().BeTrue($" {handlerType.Name} Deberia ser una clase generica");

                var returnedClass = returnType.GenericTypeArguments.First();


                returnedClass.Should().BeAssignableTo(typeof(Result), $"la clase ${handlerType.Name} deberia devolver un Result");
            }
        }
    }
}
