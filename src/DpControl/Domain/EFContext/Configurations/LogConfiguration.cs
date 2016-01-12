﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata.Builders;
using DpControl.Domain.Entities;

namespace DpControl.Domain.EFContext.Configurations
{
    public class LogConfiguration
    {
        public LogConfiguration(EntityTypeBuilder<Log> entityBuilder)
        {
            entityBuilder.ToTable("Logs", "ControlSystem");
            entityBuilder.HasKey(p => p.LogId);
            entityBuilder.Property(l => l.Comment).HasMaxLength(50);
            entityBuilder.Property(o => o.ModifiedDate).IsRequired();
            entityBuilder.Property(o => o.RowVersion).IsConcurrencyToken();

            entityBuilder.HasOne(l => l.Description).WithMany(m => m.Logs);
            entityBuilder.HasOne(l => l.LogOf).WithMany(l => l.Logs).IsRequired(false);
            entityBuilder.HasOne(l => l.Operator).WithMany(o => o.Logs).IsRequired(false);
        }
    }
}
