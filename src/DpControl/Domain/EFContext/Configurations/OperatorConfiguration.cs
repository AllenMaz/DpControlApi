using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata.Builders;
using DpControl.Domain.Entities;

namespace DpControl.Domain.EFContext.Configurations
{
    public class OperatorConfiguration
    {
        public OperatorConfiguration(EntityTypeBuilder<Operator> entityBuilder)
        {
            entityBuilder.ToTable("Operators", "ControlSystem");
            entityBuilder.HasKey(o => o.OperatorId);

            entityBuilder.Property(o => o.FirstName).HasMaxLength(30).IsRequired();
            entityBuilder.Property(o => o.LastName).HasMaxLength(30).IsRequired();
            entityBuilder.Property(o => o.Description).HasMaxLength(50);
            entityBuilder.Property(o => o.NickName).HasMaxLength(20);
            entityBuilder.Property(o => o.Password).HasMaxLength(20);
 //           entityBuilder.Property(o => o.Type).IsRequired();

            entityBuilder.Property(o => o.ModifiedDate).IsRequired();
            entityBuilder.Property(o => o.RowVersion).IsConcurrencyToken();

        }

    }
}
