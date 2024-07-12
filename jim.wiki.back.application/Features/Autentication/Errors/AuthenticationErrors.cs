using jim.wiki.core.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.back.application.Features.Autentication.Errors
{
    public static class AuthenticationErrors
    {
        public static Error UserNotFound => new Error("User_NotFound", "The user not found");
        public static Error LoginError => new Error("Login_Wrong", "The login not is valid");
        public static Error RefreshTokenError => new Error("RefreshToken_Wrong", "The operation is not valid");
    }
}
