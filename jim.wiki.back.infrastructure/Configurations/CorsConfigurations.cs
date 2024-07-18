using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.back.infrastructure.Configurations
{
    public class CorsConfigurations
    {
        public bool Enabled { get; set; }
        public string[]? OriginsAllowed { get; set; } = new string[0];
    }
}
