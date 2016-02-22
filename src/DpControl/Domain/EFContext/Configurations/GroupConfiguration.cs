using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata.Builders;
using DpControl.Domain.Entities;

namespace DpControl.Domain.EFContext.Configurations
{
    public class GroupConfiguration
    {
        public GroupConfiguration(EntityTypeBuilder<Group> entityBuilder)
        {
            entityBuilder.ToTable("Groups", "ControlSystem");
            entityBuilder.HasKey(g => g.GroupId);
            entityBuilder.HasIndex(g => g.GroupName).IsUnique();
            entityBuilder.Property(g => g.GroupName).HasMaxLength(50).IsRequired();
            entityBuilder.Property(g => g.ModifiedDate).IsRequired();
            entityBuilder.Property(g => g.RowVersion).ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();

            entityBuilder.HasOne(g => g.Scene).WithMany(s => s.LocationGroups).IsRequired(false);
        }
    }
}
