using jim.wiki.back.core.Repository.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.core.Repository.Interfaces
{
    public abstract class Aggregate:Entity
    {

    }

    public abstract class AggregateLogical : Aggregate, ILogicalEntity
    {
        public bool IsDeleted { get ; set ; }
        public string LastAction { get ; set ; }
        public DateTime CreateadAt { get ; set ; }
        public DateTime ModifiedAt { get ; set ; }
        public string CreatedBy { get ; set ; }
        public string ModifiedBy { get ; set ; }
    }
}
