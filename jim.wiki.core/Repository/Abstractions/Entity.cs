using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.back.core.Repository.Abstractions;

public abstract class Entity
{
    public long Id { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;

        if (obj is Entity entitiy)
        {
            return entitiy.Id == this.Id;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return this.Id.GetHashCode();
    }
}
