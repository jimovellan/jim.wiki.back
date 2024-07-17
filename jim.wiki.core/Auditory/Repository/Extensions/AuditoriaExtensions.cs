using jim.wiki.core.Auditory.Repository.Common;
using jim.wiki.core.Auditory.Repository.Context.Postgress;
using jim.wiki.core.Auditory.Repository.Context.SqlServer;
using jim.wiki.core.Auditory.Repository.Interfaces;
using jim.wiki.core.Enumerations;
using jim.wiki.core.Pipelines.Behaviors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.core.Auditory.Repository.Extensions
{
    public static class AuditoriaExtensions
    {
        public static IServiceCollection AddAudit(this IServiceCollection serviceCollection, string connectionString, DatabaseTypeEnum databaseType)
        {
            if (databaseType == DatabaseTypeEnum.SqlSever)
            {
                serviceCollection.AddScoped<AuditContextSqlSever>();

                serviceCollection.AddScoped<DbContextOptions>(prov =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<AuditContextSqlSever>();

                    // Configurar la cadena de conexión a SQL Server

                    optionsBuilder. UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", Constantes.SCHEMA_BBDD)); // Especifica el esquema para la tabla de migraciones);

                    // Crear DbContextOptions
                    return optionsBuilder.Options;
                });

                serviceCollection.AddScoped<IAuditContext, AuditContextSqlSever>();
                
            }

            if (databaseType == DatabaseTypeEnum.Postgress)
            {

                serviceCollection.AddScoped<AuditContextPostgres>();

                serviceCollection.AddScoped<DbContextOptions<AuditContextPostgres>>(prov =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<AuditContextPostgres>();

                    // Configurar la cadena de conexión a SQL Server

                    optionsBuilder.UseNpgsql(connectionString, sqlOptions => sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", Constantes.SCHEMA_BBDD));

                    // Crear DbContextOptions
                    return optionsBuilder.Options;
                });
                serviceCollection.AddScoped<IAuditContext, AuditContextPostgres>();
                
            }

            //Registro el repositorio de Auditoria
            serviceCollection.AddScoped<IAuditRepository, AuditRepository>();

            using (var scope = serviceCollection.BuildServiceProvider().CreateAsyncScope())
            {
                var ctx = scope.ServiceProvider.GetService<IAuditContext>();

                ctx.ApplyMigrations();
            }
            serviceCollection.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
            serviceCollection.AddScoped(typeof(IPipelineBehavior<,>), typeof(AuditablePipelineBehavior<,>));

            return serviceCollection;
        }
    }
}
