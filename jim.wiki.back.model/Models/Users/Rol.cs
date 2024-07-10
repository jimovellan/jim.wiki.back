using jim.wiki.back.core.Repository.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.back.model.Models.Users;

public class Rol:Entity
{
    public Guid Guid { get; set; }
    public string Name { get; set; }

    public string Description { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; }

    
}
