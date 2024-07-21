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
    public class UserEntityConfigurator : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(k => k.Id);

            builder.Property(k => k.Id).UseIdentityColumn(2, 1);
            builder.HasAlternateKey(x => x.Guid);
            builder.Property(p=>p.Guid).HasDefaultValueSql("gen_random_uuid()");
            builder.HasOne(x=>x.Rol).WithMany(x=>x.Users).HasForeignKey(x=>x.Id);
        }
    }
}
