using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata.Builders;
using DpControl.Domain.Entities;

namespace DpControl.Domain.EFContext.Configurations
{
    public class DeviceLocationConfiguration
    {
        public DeviceLocationConfiguration(EntityTypeBuilder<DeviceLocation> entityBuilder)
        {
            entityBuilder.HasKey(l => l.DeviceLocationId);
            entityBuilder.ToTable("DeviceLocations");

            entityBuilder.Property(l => l.Building).HasMaxLength(10).IsRequired();
            entityBuilder.Property(l => l.Floor).HasMaxLength(20);
            entityBuilder.Property(l => l.RoomNo).HasMaxLength(50);
            entityBuilder.Property(l => l.CommAddress).HasMaxLength(40);
            entityBuilder.Property(i => i.DeviceSerialNo).HasMaxLength(16);
            entityBuilder.Property(i => i.Description).HasMaxLength(2000);

            entityBuilder.Property(l => l.Creator).HasMaxLength(50).IsRequired();
            entityBuilder.Property(l => l.CreateDate).IsRequired();
            entityBuilder.Property(l => l.RowVersion).ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();

        }
    }
}
