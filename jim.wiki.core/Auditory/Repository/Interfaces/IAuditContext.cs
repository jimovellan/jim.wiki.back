using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.core.Auditory.Repository.Interfaces
{
    public interface IAuditContext
    {
        void ApplyMigrations();
        Task Add<T>(T entity);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
