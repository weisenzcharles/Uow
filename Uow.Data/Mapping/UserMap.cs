using System.Data.Entity.ModelConfiguration;
using Uow.Domain;

namespace Uow.Data.Mapping
{
    public class UserMap : EntityTypeConfiguration<UserDomain>
    {
        public UserMap()
        {
            ToTable("User");
            HasKey(bp => bp.Id);
            Property(bp => bp.Name).IsRequired();
            Property(bp => bp.Password).IsRequired();
            //this.Property(bp => bp.MetaKeywords).HasMaxLength(400);
            //this.Property(bp => bp.MetaTitle).HasMaxLength(400);
        }
    }
}