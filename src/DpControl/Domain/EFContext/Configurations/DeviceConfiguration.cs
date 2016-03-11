using DpControl.Domain.Entities;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.EFContext.Configurations
{
    public class DeviceConfiguration
    {
        public DeviceConfiguration(EntityTypeBuilder<Device> entityBuilder)
        {
            entityBuilder.ToTable("Devices");
            entityBuilder.HasKey(g => g.DeviceId);
            entityBuilder.Property(g => g.RowVersion).ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();

            //when delete Device,set null for location's foreign key
            entityBuilder.HasMany(d => d.Locations).WithOne(d => d.Device).HasForeignKey(d => d.DeviceId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
