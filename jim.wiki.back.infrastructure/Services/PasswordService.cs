using BCrypt.Net;
using jim.wiki.back.model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.back.infrastructure.Services
{
    public class PasswordService : IPasswordService
    {
        public string GenerateHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool Valid(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
