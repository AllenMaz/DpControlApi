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
            entityBuilder.HasKey(gl => new { gl.GroupId, gl.OperatorId });
            entityBuilder.HasOne(gl => gl.Group).WithMany(g => g.GroupOperators).IsRequired(false);//.HasForeignKey(gl => gl.GroupId);
            entityBuilder.HasOne(gl => gl.Operator).WithMany(l => l.GroupOperators).IsRequired(false);//.HasForeignKey(global => global.OperatorId);
        }
    }
}
