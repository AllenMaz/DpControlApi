using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata.Builders;
using DpControl.Domain.Entities;

namespace DpControl.Domain.EFContext.Configurations
{
    public class GroupLocationConfiguration
    {
        public GroupLocationConfiguration(EntityTypeBuilder<GroupLocation> entityBuilder)
        {
            entityBuilder.ToTable("GroupLocations");
            entityBuilder.HasKey(gl => gl.GroupLocationId);

            entityBuilder.HasOne(gl => gl.Group).WithMany(gl => gl.GroupLocations)
                .HasForeignKey(gl=>gl.GroupId);
            entityBuilder.HasOne(gl => gl.Location).WithMany(gl => gl.GroupLocations)
                .HasForeignKey(gl=>gl.LocationId);
        }
    }
}
