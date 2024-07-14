using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.core.Repository.Models.Search
{
    public enum OperatorEnum
    {
        None = 0,
        Equal = 1,
        NotEqual = 2,
        In = 3,
        GreaterThan = 4,
        GreaterThanOrEqual = 5,
        LessThan = 6,
        LessThanOrEqual = 7,
        Like = 8
    }
}
