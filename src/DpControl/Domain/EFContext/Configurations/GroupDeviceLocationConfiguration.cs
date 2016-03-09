using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata.Builders;
using DpControl.Domain.Entities;

namespace DpControl.Domain.EFContext.Configurations
{
    public class GroupDeviceLocationConfiguration
    {
        public GroupDeviceLocationConfiguration(EntityTypeBuilder<GroupDeviceLocation> entityBuilder)
        {
            entityBuilder.ToTable("GroupDeviceLocations");
            entityBuilder.HasKey(gl => new { gl.GroupId,gl.DeviceLocationId});

            entityBuilder.HasOne(gl => gl.Group).WithMany(gl => gl.GroupDeviceLocations)
                .HasForeignKey(gl=>gl.GroupId);
            entityBuilder.HasOne(gl => gl.DeviceLocation).WithMany(gl => gl.GroupDeviceLocations)
                .HasForeignKey(gl=>gl.DeviceLocationId);
        }
    }
}
