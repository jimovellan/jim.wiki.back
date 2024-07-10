using jim.wiki.core.Auditory.Models;
using jim.wiki.core.Auditory.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.core.Auditory.Repository
{
    public class AuditRepository : IAuditRepository
    {
        private readonly IAuditContext dbContext;

        public AuditRepository(IAuditContext dbContext)
        {
            Guid = Guid.NewGuid();
            this.dbContext = dbContext;
        }

        public Guid Guid { get ; set ; }

        public async Task Save(Audit audit)
        {
            await dbContext.Add(audit); 
            var x = await dbContext.SaveChangesAsync();
        }
    }
}
