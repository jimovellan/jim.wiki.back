using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.back.core.Repository.Abstractions;

public interface ILogicalEntity:IAuditEntity
{
    public bool IsDeleted { get; set; }
}
