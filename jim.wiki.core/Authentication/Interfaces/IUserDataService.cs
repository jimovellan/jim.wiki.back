using jim.wiki.core.Authentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.core.Authentication.Interfaces
{
    public interface IUserDataService
    {
        IUserData GetUser();
    }
}
