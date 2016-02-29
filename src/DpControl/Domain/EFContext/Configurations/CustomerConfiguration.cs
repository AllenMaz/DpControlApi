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
            entityBuilder.ToTable("Customers", "ControlSystem");
            entityBuilder.HasKey(c => c.CustomerId);
            entityBuilder.Property(c => c.CustomerName).HasMaxLength(50).IsRequired();
            entityBuilder.Property(c => c.CustomerNo).HasMaxLength(50).IsRequired();
            entityBuilder.HasIndex(c => c.CustomerNo).IsUnique();
            entityBuilder.Property(c => c.CreateDate).IsRequired();
            entityBuilder.Property(c => c.RowVersion).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

            entityBuilder.HasMany(c => c.Projects).WithOne(p => p.Customer).HasForeignKey(p => p.CustomerId);
        }
    }
}
