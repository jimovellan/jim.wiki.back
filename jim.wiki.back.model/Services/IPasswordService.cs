using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.back.model.Services;

public interface IPasswordService
{
    string GenerateHash(string password);
    bool Valid(string password, string hash);
}
