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
            entityBuilder.ToTable("GroupLocations", "ControlSystem");
            entityBuilder.HasKey(gl => gl.GroupLocationId);

            entityBuilder.HasOne(gl => gl.Group).WithMany(g => g.GroupLocations).HasForeignKey(gl=>gl.GroupId).IsRequired(false);
            entityBuilder.HasOne(gl => gl.Location).WithMany(g => g.GroupLocations).HasForeignKey(gl=>gl.LocationId).IsRequired(false);
        }
    }
}
