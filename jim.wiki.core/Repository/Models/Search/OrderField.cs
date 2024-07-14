using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.core.Repository.Models.Search
{
    public class OrderField
    {
        public string Field { get; set; }
        public string? Direction { get; set; } = "Asc";
    }
}
