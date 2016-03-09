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
    public class LogConfiguration
    {
        public LogConfiguration(EntityTypeBuilder<Log> entityBuilder)
        {
            entityBuilder.ToTable("Logs");
            entityBuilder.HasKey(p => p.LogId);
            entityBuilder.Property(l => l.Comment).HasMaxLength(50);
            entityBuilder.Property(o => o.ModifiedDate).IsRequired();
            entityBuilder.Property(o => o.RowVersion).ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();

            entityBuilder.HasOne(l => l.DeviceLocation).WithMany(l => l.Logs).IsRequired(false);
            entityBuilder.HasOne(l => l.User).WithMany(o => o.Logs);
        }
    }
}
