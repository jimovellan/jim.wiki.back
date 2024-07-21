using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.core.Repository.Interfaces
{
    public interface IDataBaseSeeder<T>
    {
        Task SeedData(T seeder);
    }
}
