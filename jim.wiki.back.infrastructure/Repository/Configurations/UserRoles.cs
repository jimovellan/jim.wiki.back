using jim.wiki.back.model.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.back.infrastructure.Repository.Configurations
{
    public class UserRolesEntityConfigurator : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasAlternateKey(p => new { p.UserGuid, p.RolGuid });

            builder.HasOne(p=>p.User).WithMany(x=>x.RolesUser).HasForeignKey(x=>x.UserGuid).HasPrincipalKey(x=>x.Guid);
            builder.HasOne(p => p.Rol).WithMany(x => x.UserRoles).HasForeignKey(x => x.RolGuid).HasPrincipalKey(x=>x.Guid);
        }
    }
}
