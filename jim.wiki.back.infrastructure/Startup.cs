using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using jim.wiki.back.infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.back.infrastructure
{
    public static class Startup
    {
        /// <summary>
        /// Establece toda la configuración de la inyección de dependencias del proyecto
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configuration"></param>
        public static void ApplyAppConfiguration(this IServiceCollection serviceCollection, IConfigurationManager configuration)
        {
            serviceCollection
                .AddApplicationOptions(configuration)
                .AddDDBBConection(configuration)
                .RegisterAplicationServices(configuration)
                .GenerateAdminUser(configuration);

                
                
        }
    }
}
