using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata.Builders;
using DpControl.Domain.Entities;

namespace DpControl.Domain.EFContext.Configurations
{
    public class ProjectConfiguration
    {
        public ProjectConfiguration(EntityTypeBuilder<Project> entityBuilder)
        {
            entityBuilder.HasKey(c => c.ProjectId);
            entityBuilder.ToTable("Customers", "ControlSystem");
            entityBuilder.Property(c => c.ProjectName).HasMaxLength(50).IsRequired();            // Name
            entityBuilder.Property(c => c.ProjectNo).HasMaxLength(50).IsRequired();
            entityBuilder.Property(p => p.ModifiedDate).IsRequired();
            entityBuilder.Property(p => p.RowVersion).ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();      //.IsRowVersion();

            entityBuilder.HasMany(p => p.Operators).WithOne(o => o.Project).HasForeignKey(o => o.ProjectId);
            entityBuilder.HasMany(p => p.DeviceLocations).WithOne(l => l.Project).HasForeignKey(l => l.ProjectId);
            entityBuilder.HasMany(p => p.Groups).WithOne(l => l.Project).HasForeignKey(l => l.ProjectId);
            entityBuilder.HasMany(p => p.Scenes).WithOne(l => l.Project).HasForeignKey(l => l.ProjectId);
            entityBuilder.HasMany(p => p.Holidays).WithOne(l => l.Project).HasForeignKey(l => l.ProjectId);
        }
    }
}
