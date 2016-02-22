using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata.Builders;
using DpControl.Domain.Entities;

namespace DpControl.Domain.EFContext.Configurations
{
    public class CustomerConfiguration
    {
        public CustomerConfiguration(EntityTypeBuilder<Customer> entityBuilder)
        {
            entityBuilder.HasKey(c => c.CustomerId);
            entityBuilder.ToTable("Customers", "ControlSystem");
            entityBuilder.Property(c => c.CustomerName).HasMaxLength(60).IsRequired();            // Name
            entityBuilder.Property(c => c.CustomerNo).HasMaxLength(20).IsRequired();
            entityBuilder.Property(c => c.ProjectName).HasMaxLength(60).IsRequired();            // Name
            entityBuilder.Property(c => c.ProjectNo).HasMaxLength(20).IsRequired();
            entityBuilder.Property(p => p.ModifiedDate).IsRequired();
            entityBuilder.Property(p => p.RowVersion).ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();      //.IsRowVersion();

            entityBuilder.HasMany(p => p.Operators).WithOne(o => o.Customer).HasForeignKey(o => o.CustomerId);
            entityBuilder.HasMany(p => p.DeviceLocations).WithOne(l => l.Customer).HasForeignKey(l => l.CustomerId);
            entityBuilder.HasMany(p => p.Groups).WithOne(l => l.Customer).HasForeignKey(l => l.CustomerId);
            entityBuilder.HasMany(p => p.Scenes).WithOne(l => l.Customer).HasForeignKey(l => l.CustomerId);
            entityBuilder.HasMany(p => p.Holidays).WithOne(l => l.Customer).HasForeignKey(l => l.CustomerId);
        }
    }
}
