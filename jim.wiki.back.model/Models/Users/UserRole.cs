using jim.wiki.back.core.Repository.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.back.model.Models.Users
{
    public class UserRole:Entity
    {
        public Guid RolGuid { get; set; }
        public Guid UserGuid { get; set; }

        [ForeignKey("RolGuid")]
        public virtual Rol Rol { get; set; }
        [ForeignKey("UserGuid")]
        public virtual User User { get; set; }
    }
}
