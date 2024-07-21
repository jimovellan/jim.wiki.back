using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.back.application.Services
{
    public interface IMapperServices
    {
        TDestiny Map<TDestiny,TOrigin>(TOrigin origin);
        TDestiny Map<TDestiny>(object origin);
    }
}
