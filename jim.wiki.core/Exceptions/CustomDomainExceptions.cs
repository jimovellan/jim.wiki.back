using jim.wiki.core.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.core.Exceptions
{
    public class CustomDomainExceptions: Exception
    {
        public ICollection<Error> Errors { get; set; }
        public CustomDomainExceptions(params Error[] errors)
        {
            Errors ??= new List<Error>();
            foreach (var error in errors)
            {
                Errors.Add(error);
            }
        }
    }
}
