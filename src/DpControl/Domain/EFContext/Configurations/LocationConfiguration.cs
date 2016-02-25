using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata.Builders;
using DpControl.Domain.Entities;

namespace DpControl.Domain.EFContext.Configurations
{
    public class LocationConfiguration
    {
        public LocationConfiguration(EntityTypeBuilder<Location> entityBuilder)
        {
            entityBuilder.HasKey(l => l.LocationId);
            entityBuilder.ToTable("DeviceLocations", "ControlSystem");

            entityBuilder.Property(l => l.Building).HasMaxLength(80).IsRequired();
            entityBuilder.Property(l => l.Floor).HasMaxLength(20);
            entityBuilder.Property(l => l.RoomNo).HasMaxLength(50);
            entityBuilder.Property(l => l.CommAddress).HasMaxLength(40);
            entityBuilder.Property(i => i.DeviceSerialNo).HasMaxLength(16);
            entityBuilder.Property(i => i.Description).HasMaxLength(200);

            entityBuilder.Property(l => l.ModifiedDate).IsRequired();
            entityBuilder.Property(l => l.RowVersion).ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();
        }
    }
}
