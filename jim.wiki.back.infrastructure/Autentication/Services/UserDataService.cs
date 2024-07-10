using jim.wiki.back.infrastructure.Autentication.Model;
using jim.wiki.core.Authentication.Interfaces;
using jim.wiki.core.Authentication.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.back.infrastructure.Autentication.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly IHttpContextAccessor httpContext;

        public UserDataService(IHttpContextAccessor httpContext)
        {
            this.httpContext = httpContext;
        }
        public IUserData GetUser()
        {
            return new UserData()
            {
                Email = "",
                Name = "Prueba",
                IP = "192.168.1.1",
                Id = 1000
            };
        }
    }
}
