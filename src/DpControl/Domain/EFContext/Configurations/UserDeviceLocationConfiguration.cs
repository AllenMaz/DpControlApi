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
    public class UserDeviceLocationConfiguration
    {
        public UserDeviceLocationConfiguration(EntityTypeBuilder<UserDeviceLocation> entityBuilder)
        {
            entityBuilder.ToTable("UserDeviceLocations");
            entityBuilder.HasKey(gl => new { gl.UserId,gl.DeviceLocationId});

            entityBuilder.HasOne(gl => gl.DeviceLocation).WithMany(g => g.UserDeviceLocation)
                .HasForeignKey(g=>g.DeviceLocationId);
            entityBuilder.HasOne(gl => gl.User).WithMany(l => l.UserDeviceLocations)
                .HasForeignKey(g=>g.UserId);
        }
    }
}
