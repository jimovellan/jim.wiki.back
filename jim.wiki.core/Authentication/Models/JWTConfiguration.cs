using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.core.Authentication.Models
{
    public class JWTConfiguration
    {
        public bool? Enable { get; set; }
        public string Secret { get; set; }
        public bool VerifyAudience { get; set; }
        public bool VerifyIssuer { get; set; }
        public int MinutesExpiration { get; set; }
        public List<string> Audience { get; set; }
    }
}
