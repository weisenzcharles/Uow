using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uow.Entities;

namespace Uow.Data.Mapping
{
    public class UserMap: EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            ToTable("User");
            this.HasKey(bp => bp.Id);
            this.Property(bp => bp.Name).IsRequired();
            this.Property(bp => bp.Password).IsRequired();
            //this.Property(bp => bp.MetaKeywords).HasMaxLength(400);
            //this.Property(bp => bp.MetaTitle).HasMaxLength(400);

        }
    }
}
