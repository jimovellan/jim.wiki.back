using jim.wiki.core.Authentication.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.core.Authentication.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection ConfigureJWTAuthentication(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            ConfigureOptions(serviceCollection, configuration);
            ConfigureAuthentication(serviceCollection);
            return serviceCollection;
        }

        private static void ConfigureOptions(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<JWTConfiguration>(configuration.GetSection("Autentication"));
           
        }
        private static void ConfigureAuthentication(IServiceCollection serviceCollection)
        {
            using (var scope = serviceCollection.BuildServiceProvider())
            {
                var optJwtConfiguration = scope.GetService<IOptions<JWTConfiguration>>();

                if (optJwtConfiguration == null || optJwtConfiguration.Value == null || !(optJwtConfiguration.Value.Enable ?? false)) return;

                var key = Encoding.ASCII.GetBytes(optJwtConfiguration.Value.Secret);

                serviceCollection.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    
                }).AddJwtBearer(x =>
                {
                    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = optJwtConfiguration.Value.VerifyAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateLifetime = true,

                    };
                });

                serviceCollection.AddAuthorization(options =>
                {
                    options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                        .RequireAuthenticatedUser()
                        .Build();
                });


            }
           
        }
    }
}
