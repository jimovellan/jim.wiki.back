
using FluentValidation.AspNetCore;
using jim.wiki.back.api.Middlewares;
using jim.wiki.back.infrastructure;
using jim.wiki.back.infrastructure.Repository;
using jim.wiki.core.Authentication.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace jim.wiki.back.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHttpContextAccessor();
            // Add services to the container
            builder.Services.ApplyAppConfiguration(builder.Configuration);

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.ConfigureJWTAuthentication(builder.Configuration);

            


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement{
    {
        new OpenApiSecurityScheme{
            Reference = new OpenApiReference{
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[]{}
    }});
            });



            Console.Write(@"





     _ ___ __  __   ____   ___  _ _  _   
    | |_ _|  \/  | |___ \ / _ \/ | || |  
 _  | || || |\/| |   __) | | | | | || |_ 
| |_| || || |  | |  / __/| |_| | |__   _|
 \___/|___|_|  |_| |_____|\___/|_|  |_|  

                                    |`-.__
                                    / ' _/
                                   ****` 
                                  /    }
                                 /  \ /
                             \ /`   \\\
                        lua   `\    /_\\
                               `~~~~~``~`





            
");




            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }



            app.UseMiddleware<HandlingExceptionMidleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpsRedirection();

            


            app.MapControllers();

            app.Run();
        }
    }
}
