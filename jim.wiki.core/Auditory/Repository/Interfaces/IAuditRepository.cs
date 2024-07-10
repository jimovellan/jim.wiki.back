using jim.wiki.core.Auditory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.core.Auditory.Repository.Interfaces
{
    public interface IAuditRepository
    {
        public Guid Guid { get; set; }
        public Task Save(Audit audit);
    }
}
