using jim.wiki.core.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.core.Exceptions
{
    public class CustomDomainValidationException:CustomDomainExceptions
    {
        public CustomDomainValidationException(params Error[] errors):base(errors)
        {
            
        }
    }
}
