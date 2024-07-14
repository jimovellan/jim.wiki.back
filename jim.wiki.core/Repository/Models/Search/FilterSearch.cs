using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.core.Repository.Models.Search
{
    public class FilterSearch
    {
        public List<FieldSearch>? Filter { get; set; } = new List<FieldSearch>();
        public PaginationSearch? Pagination { get; set; } = new PaginationSearch();
        public IEnumerable<OrderField>? Order { get; set; } = new List<OrderField>();

        public bool Validate() 
        {
            Filter ??= new List<FieldSearch>();
            foreach (var field in Filter)
            {
                field.IsValid();
            }
            return true;
        }

    }
}
