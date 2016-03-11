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
    public class LogDescriptionConfiguration
    {
        public LogDescriptionConfiguration(EntityTypeBuilder<LogDescription> entityBuilder)
        {
            entityBuilder.HasKey(m => m.LogDescriptionId);
            entityBuilder.ToTable("LogDescription");
            entityBuilder.Property(m => m.Description).HasMaxLength(500);

            entityBuilder.HasMany(l => l.Logs).WithOne(d => d.Description).HasForeignKey(d => d.LogDescriptionId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
