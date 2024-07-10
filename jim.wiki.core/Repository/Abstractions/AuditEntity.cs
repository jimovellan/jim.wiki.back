using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.back.core.Repository.Abstractions;

public abstract class AuditEntity:Entity
{
    public string  LastAction { get; set; }
    public DateTime CreateadAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
}
