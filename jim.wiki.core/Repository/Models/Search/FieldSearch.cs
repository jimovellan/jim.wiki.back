using jim.wiki.core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.core.Repository.Models.Search
{
    public class FieldSearch
    {
        public string Name { get; set; }
        public OperatorEnum Operation { get; set; }
        public object? Value { get; set; }
        public List<object>? Values { get; set; }

        public LogicalOperation? LogicalOperation { get; set; } = Search.LogicalOperation.And;

        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(Name)) throw new InvalidDataException("The name of field cant be null");

            if (Operation == OperatorEnum.None) throw new InvalidOperationException("The operation must be specified");

            if (Operation == OperatorEnum.In && Values.NoContainElements()) throw new InvalidDataException($"Must specify the field 'Values' to opeate with de Field {Name}");

            if (Operation != OperatorEnum.In && Value is null) throw new InvalidDataException($"Must specify the field 'Value' to opeate with de Field {Name}");

            return true;
        }

    }
}
