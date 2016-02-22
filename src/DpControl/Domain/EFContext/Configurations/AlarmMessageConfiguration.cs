using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata.Builders;
using DpControl.Domain.Entities;

namespace DpControl.Domain.EFContext.Configurations
{
    public class AlarmMessageConfiguration
    {
        public AlarmMessageConfiguration(EntityTypeBuilder<AlarmMessage> entityBuilder)
        {
            entityBuilder.HasKey(m => m.AlarmMessageId);
            entityBuilder.ToTable("AlarmMessages", "ControlSystem");
            entityBuilder.Property(m => m.Message).HasMaxLength(100);

            entityBuilder.HasMany(m => m.Alarms).WithOne(a => a.AlarmMessage).HasForeignKey(a => a.AlarmMessageId);
        }
    }
}
