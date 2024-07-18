using FluentValidation;
using jim.wiki.back.application;
using jim.wiki.back.application.Features.Users;
using jim.wiki.back.infrastructure.Autentication.Services;
using jim.wiki.back.infrastructure.Configurations;
using jim.wiki.back.infrastructure.Repository;
using jim.wiki.back.infrastructure.Services;
using jim.wiki.back.model.Models.Users;
using jim.wiki.back.model.Services;
using jim.wiki.core.Auditory.Repository.Extensions;
using jim.wiki.core.Authentication.Interfaces;
using jim.wiki.core.Extensions;
using jim.wiki.core.Json;
using jim.wiki.core.Pipelines.Abstrantions;
using jim.wiki.core.Pipelines.Behaviors;
using jim.wiki.core.Repository.Interfaces;
using jim.wiki.core.Repository.Models.Search;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Reflection;

namespace jim.wiki.back.infrastructure.Extensions;

internal static class ServiceCollectionExtension
{
    internal const string DATA_BASE_SETTINGS_KEY = "DataBase";
    internal const string LANGUAGES_SETTINS_KEY = "Languages";
    internal const string CORS_SETTINGS_KEY = "Cors";



    /// <summary>
    /// Registro de todos los IConfiguration
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <returns></returns>
    internal static IServiceCollection AddApplicationOptions(this IServiceCollection serviceCollection, IConfigurationManager configuration)
    {
        Console.WriteLine("- Cargando parametros de configuracion de settings");
        serviceCollection.AddOptions<DatabaseConfiguration>().Bind(configuration.GetSection(DATA_BASE_SETTINGS_KEY));
        serviceCollection.AddOptions<LanguagesConfiguration>().Bind(configuration.GetSection(LANGUAGES_SETTINS_KEY));
        serviceCollection.AddOptions<CorsConfigurations>().Bind(configuration.GetSection(CORS_SETTINGS_KEY));

        return serviceCollection;
    }


    public static IServiceCollection ConfigureCors(this IServiceCollection services, IConfiguration configuration)
    {
        using (var scoped = services.BuildServiceProvider().CreateScope())
        {
            var corsConfig = scoped.ServiceProvider.GetService<IOptions<CorsConfigurations>>();

            if (corsConfig?.Value?.Enabled ?? false)
            {
                Console.WriteLine("- Configurando Cors");
                Console.WriteLine($"- urls permitidas {string.Join(",",corsConfig.Value.OriginsAllowed)}");
                services.AddCors(opt =>
                {
                    opt.AddPolicy("AllowedSpecificOrigins", bulder =>
                    {
                        bulder.WithOrigins(corsConfig.Value.OriginsAllowed)
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
                });
            }
        }

        return services;
    }

    /// <summary>
    /// Configuación de los lenguages de la aplicación
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    /// <exception cref="CultureNotFoundException"></exception>
    public static IServiceCollection ConfigureLenguages(this IServiceCollection services, IConfiguration configuration)
    {

        using (var scoped = services.BuildServiceProvider().CreateScope())
        {
            var langConfig = scoped.ServiceProvider.GetService<IOptions<LanguagesConfiguration>>();

            if(langConfig?.Value?.Enabled ?? false)
            {
                Console.WriteLine("Activada configuración de lenguajes");

                services.AddLocalization(conf =>
                {
                    conf.ResourcesPath = "Resources";
                });

                services.Configure<RequestLocalizationOptions>(opt =>
                {
                    if (!String.IsNullOrWhiteSpace(langConfig.Value.Default))
                    {
                        Console.WriteLine($"    - Cultura por defecto : {langConfig.Value.Default}");
                        opt.DefaultRequestCulture = new RequestCulture(langConfig.Value.Default);
                    }

                    if (langConfig!.Value!.Accepted!.ContainElements())
                    {
                        Console.WriteLine($"    - Idiomas aceptados : {String.Join(",", langConfig!.Value!.Accepted!)}");
                        var supportedCultures = new List<CultureInfo>();
                        foreach (var lang in langConfig.Value.Accepted)
                        {
                            supportedCultures.Add(new CultureInfo(lang));
                        }

                        opt.SupportedCultures = supportedCultures;
                        opt.SupportedUICultures = supportedCultures;

                        //Configuración lanzar excepción si se solicita un lenguaje no permitido
                        if (langConfig!.Value!.ThrowExceptionIFNotAllowed)
                        {
                            opt.RequestCultureProviders = new List<IRequestCultureProvider>
                        {
                            new CustomRequestCultureProvider(context =>
                            {
                                // Obtén la cultura de los encabezados de la solicitud
                                var userLanguages = context.Request.Headers["Accept-Language"].ToString();
                                if (string.IsNullOrWhiteSpace(userLanguages))
                                {
                                    return Task.FromResult(new ProviderCultureResult(opt.DefaultRequestCulture.Culture.Name, opt.DefaultRequestCulture.Culture.Name));
                                }
                                var firstLang = userLanguages?.Split(',').FirstOrDefault();
                                var culture = new CultureInfo(firstLang ?? opt.DefaultRequestCulture.Culture.Name);

                                // Si la cultura no está soportada, lanza una excepción
                                if (!supportedCultures.Contains(culture))
                                {
                                    throw new CultureNotFoundException($"Culture {culture} is not supported.");
                                }

                                return Task.FromResult(new ProviderCultureResult(culture.Name, culture.Name));
                            })
                        };

                        }


                    }
                });
            }
        }

        return services;

    }

    /// <summary>
    /// registrar todos los servicios de la aplicacion
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <returns></returns>
    internal static IServiceCollection RegisterAplicationServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        Console.WriteLine("- Registrando servicios en inyección de dependencias"); 

        serviceCollection.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryGeneric<>));

        serviceCollection.AddMediatR(Assembly.GetAssembly(typeof(jim.wiki.back.application.Startup)));

        serviceCollection.AddAudit(GenerateConnectionString(serviceCollection, configuration), databaseType: wiki.core.Enumerations.DatabaseTypeEnum.Postgress);

        serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionalPipelineBehavior<,>));

        serviceCollection.AddValidatorsFromAssemblies(new Assembly[] { typeof(jim.wiki.back.application.Startup).Assembly });
        

        serviceCollection.AddScoped<IUserDataService, UserDataService>();

        serviceCollection.AddTransient<IPasswordService, PasswordService>();

        //Configuramos comporatmineto de json para que use Strings en vez de los int de los enumerados
        serviceCollection.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new FlexibleStatusJsonConverter<LogicalOperation>(true));
            options.JsonSerializerOptions.Converters.Add(new FlexibleStatusJsonConverter<OperatorEnum>(true));
        });

        return serviceCollection;
    }

    internal static IServiceCollection AddDDBBConection(this IServiceCollection serviceCollection, IConfigurationManager configuration)
    {
        Console.WriteLine("- Cargando configuración de BBDD");

        var connectiontring = serviceCollection.GenerateConnectionString(configuration);

        serviceCollection.AddDbContext<ApplicationContext>(config =>
       {
           config.UseNpgsql(connectiontring);
       });

        serviceCollection.AddScoped<IUnitOfWork, ApplicationContext>();


        using (var scope = serviceCollection.BuildServiceProvider())
        {
            var ctx = scope.GetService<ApplicationContext>();
            ctx.Database.Migrate();
        }


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

        using (var scope = serviceCollection.BuildServiceProvider().CreateScope())
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


    internal static void GenerateAdminUser(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        using (var scope = serviceCollection.BuildServiceProvider())
        {
            var repositoryUser = scope.GetService<IRepositoryBase<User>>();
            var adminUser = repositoryUser.Query().FirstOrDefault(x => x.Name == "Admin");
            var sender = scope.GetService<ISender>();
            if(adminUser is null)
            {
                sender.Send(new CreateUserRequest() { Name = "Admin", 
                                                      Email = "Admin@mail.com", 
                                                      Password = "@12345678!" })
                                                      .Wait();
            }
        }
    }
}


