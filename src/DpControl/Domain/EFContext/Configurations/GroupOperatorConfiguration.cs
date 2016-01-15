using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata.Builders;
using DpControl.Domain.Entities;

namespace DpControl.Domain.EFContext.Configurations
{
    public class GroupOperatorConfiguration
    {
        public GroupOperatorConfiguration(EntityTypeBuilder<GroupOperator> entityBuilder)
        {
            entityBuilder.ToTable("GroupOperators", "ControlSystem");
            entityBuilder.HasKey(gl => gl.GroupOperatorId);

            entityBuilder.HasOne(gl => gl.Group).WithMany(g => g.GroupOperators).HasForeignKey(gl=>gl.GroupId).IsRequired(false);
            entityBuilder.HasOne(gl => gl.Operator).WithMany(l => l.GroupOperators).HasForeignKey(gl=>gl.OperatorId).IsRequired(false);
        }
    }
}
