using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.back.model.Services
{
    public interface IUserInfo
    {
        Task<String> GetName();

        
    }
}
