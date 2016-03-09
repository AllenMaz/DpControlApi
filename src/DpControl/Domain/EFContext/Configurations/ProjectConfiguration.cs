using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata.Builders;
using DpControl.Domain.Entities;
using Microsoft.Data.Entity.Metadata;

namespace DpControl.Domain.EFContext.Configurations
{
    public class ProjectConfiguration
    {
        public ProjectConfiguration(EntityTypeBuilder<Project> entityBuilder)
        {
            entityBuilder.ToTable("Projects");
            entityBuilder.HasKey(p => p.ProjectId);
            entityBuilder.Property(p => p.ProjectName).HasMaxLength(50).IsRequired();            // Name
            entityBuilder.Property(p => p.ProjectNo).HasMaxLength(50).IsRequired();
            entityBuilder.HasIndex(p => p.ProjectNo).IsUnique();

            entityBuilder.Property(p => p.Creator).IsRequired();
            entityBuilder.Property(p => p.CreateDate).IsRequired();
            entityBuilder.Property(p => p.RowVersion).ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();      //.IsRowVersion();
            entityBuilder.Property(p => p.Completed).HasDefaultValue(false).IsRequired();
            
            //when delete project ,do not cascade delete Groups
            entityBuilder.HasMany(p => p.Groups).WithOne(l => l.Project).HasForeignKey(l => l.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);
            //when delete project ,do not cascade delete DeviceLocations
            entityBuilder.HasMany(p => p.DeviceLocations).WithOne(l => l.Project).HasForeignKey(l => l.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);
            //when delete project ,do not cascade delete AspNetUsers
            entityBuilder.HasMany(p => p.Users).WithOne(o => o.Project).HasForeignKey(o => o.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasMany(p => p.Scenes).WithOne(l => l.Project).HasForeignKey(l => l.ProjectId);
            entityBuilder.HasMany(p => p.Holidays).WithOne(l => l.Project).HasForeignKey(l => l.ProjectId);
        }
    }
}
