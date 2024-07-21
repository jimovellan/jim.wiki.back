using jim.wiki.back.infrastructure.Repository.Seeds;
using jim.wiki.back.model.Models.Users;
using jim.wiki.back.model.Models.Users.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.back.infrastructure.Repository.Configurations
{
    public class RolEntityConfigurator : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> builder)
        {
            builder.Property(p => p.Id).ValueGeneratedNever();
            builder.HasAlternateKey(p => p.Guid);
            builder.Property(p => p.Guid).HasDefaultValueSql("gen_random_uuid()");
           
            
        }
    }
}
