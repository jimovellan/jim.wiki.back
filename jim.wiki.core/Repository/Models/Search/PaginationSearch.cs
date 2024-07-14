using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.core.Repository.Models.Search
{
    public  class PaginationSearch
    {
        public int? Take { get; set; } = 20;
        public int? Page { get; set; } = 1;
    }
}
