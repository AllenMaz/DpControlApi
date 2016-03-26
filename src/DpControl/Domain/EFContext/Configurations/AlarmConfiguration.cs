using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata.Builders;
using DpControl.Domain.Entities;

namespace DpControl.Domain.EFContext.Configurations
{
    public class AlarmConfiguration
    {
        public AlarmConfiguration(EntityTypeBuilder<Alarm> entityBuilder)
        {
            entityBuilder.ToTable("Alarms");
            entityBuilder.HasKey(a => a.AlarmId);
            entityBuilder.Property(a => a.CreateDate).IsRequired();
            entityBuilder.Property(a => a.RowVersion).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

            entityBuilder.HasOne(a=>a.Location).WithMany(o => o.Alarms).HasForeignKey(a=>a.LocationId);
        }
    }
}
