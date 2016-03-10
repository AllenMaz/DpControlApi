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
    public class UserLocationConfiguration
    {
        public UserLocationConfiguration(EntityTypeBuilder<UserLocation> entityBuilder)
        {
            entityBuilder.ToTable("UserLocations");
            entityBuilder.HasKey(gl => gl.UserLocationId);

            entityBuilder.HasOne(gl => gl.Location).WithMany(g => g.UserLocations)
                .HasForeignKey(g=>g.LocationId);
            entityBuilder.HasOne(gl => gl.User).WithMany(l => l.UserLocations)
                .HasForeignKey(g=>g.UserId).IsRequired();
        }
    }
}
