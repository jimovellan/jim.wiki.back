using jim.wiki.back.application.Features.Users;
using jim.wiki.back.infrastructure.Autentication.Services;
using jim.wiki.back.infrastructure.Repository;
using jim.wiki.back.infrastructure.Repository.Models;
using jim.wiki.core.Auditory.Repository.Extensions;
using jim.wiki.core.Authentication.Interfaces;
using jim.wiki.core.Extensions;
using jim.wiki.core.Pipelines.Abstrantions;
using jim.wiki.core.Pipelines.Behaviors;
using jim.wiki.core.Repository.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace jim.wiki.back.infrastructure.Extensions;

internal static class ServiceCollectionExtension
    {
        internal const string DATA_BASE_SETTINGS_KEY = "DataBase";

    /// <summary>
    /// Registro de todos los IConfiguration
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <returns></returns>
    internal static IServiceCollection AddApplicationOptions(this IServiceCollection serviceCollection, IConfigurationManager configuration)
        {
            serviceCollection.AddOptions<DatabaseConfiguration>().Bind(configuration.GetSection(DATA_BASE_SETTINGS_KEY));
           
            return serviceCollection;
        }

    /// <summary>
    /// registrar todos los servicios de la aplicacion
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <returns></returns>
    internal static IServiceCollection RegisterAplicationServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryGeneric<>));

            serviceCollection.AddMediatR(Assembly.GetAssembly(typeof(CreateUserRequest)));

            serviceCollection.AddAudit(GenerateConnectionString(serviceCollection,configuration), databaseType: wiki.core.Enumerations.DatabaseTypeEnum.Postgress);
            
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionalPipelineBehavior<,>));

            serviceCollection.AddScoped<IUserDataService, UserDataService>();
            
            return serviceCollection;
        }

    internal static IServiceCollection AddDDBBConection(this IServiceCollection serviceCollection, IConfigurationManager configuration)
        {

            var connectiontring = serviceCollection.GenerateConnectionString(configuration);

             serviceCollection.AddDbContext<ApplicationContext>(config =>
            {
                config.UseNpgsql(connectiontring);
            });
           
            serviceCollection.AddScoped<IUnitOfWork,ApplicationContext>();
            return serviceCollection;
        }

    internal static IServiceCollection RegisterApplicationServices(IServiceCollection serviceCollection, IConfigurationBuilder configuration)
        {
            return serviceCollection;
        }

    /// <summary>
    /// crea la cadena de conexion
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="Exception"></exception>
    internal static string GenerateConnectionString(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var connectionStringPattern = configuration.GetSection("ConnectionString").Value;

            if (String.IsNullOrWhiteSpace(connectionStringPattern)) throw new ArgumentNullException(nameof(connectionStringPattern)); 

            using(var scope = serviceCollection.BuildServiceProvider().CreateScope())
            {
                var dbOptions = scope.ServiceProvider.GetService<IOptions<DatabaseConfiguration>>();

                if (dbOptions == null || dbOptions.Value == null) throw new Exception("No encontrada cadena de conexión");

                var properties = typeof(DatabaseConfiguration).GetProperties();

                foreach (var item in properties)
                {
                    var propertyName = $"{{{item.Name.ToUpper()}}}";
                    var value = item.GetValue(dbOptions.Value);
                    connectionStringPattern = connectionStringPattern.Replace(propertyName, value?.ToString() ?? "");
                }
            }

            return connectionStringPattern;
        }
    }

