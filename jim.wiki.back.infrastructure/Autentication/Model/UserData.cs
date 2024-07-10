using jim.wiki.core.Authentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.back.infrastructure.Autentication.Model
{
    public class UserData : IUserData
    {
        public long? Id { get ; set ; }
        public string? Name { get  ; set ; }
        public string? Email { get ; set ; }
        public string? IP { get ; set ; }
    }
}
