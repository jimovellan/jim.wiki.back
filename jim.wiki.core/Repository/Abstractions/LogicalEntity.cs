using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.back.core.Repository.Abstractions;

public class LogicalEntity:AuditEntity
{
    public bool IsDeleted { get; set; }
}
