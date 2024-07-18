using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.back.infrastructure.Configurations
{
    public class LanguagesConfiguration
    {
        public bool Enabled { get; set; }
        public IEnumerable<string>? Accepted { get; set; }
        public string? Default { get; set; }
        public bool ThrowExceptionIFNotAllowed { get; set; }
    }
}
