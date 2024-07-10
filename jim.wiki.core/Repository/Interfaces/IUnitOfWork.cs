using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.core.Repository.Interfaces;

public interface IUnitOfWork
{
    public void BeginTransaction();

    public void Commit();

    public void Rollback();

    public Task<int> SaveAsync();

    public bool HasTransaction();
}
