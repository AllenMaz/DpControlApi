using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata.Builders;
using DpControl.Domain.Entities;

namespace DpControl.Domain.EFContext.Configurations
{
    public class HolidayConfiguration
    {
        public HolidayConfiguration(EntityTypeBuilder<Holiday> entityBuilder)
        {
            entityBuilder.HasKey(c => c.HolidayId);
            entityBuilder.ToTable("Holidays", "ControlSystem");

            entityBuilder.Property(h => h.ModifiedDate).IsRequired();
            entityBuilder.Property(h => h.RowVersion).ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();
        }
    }
}
