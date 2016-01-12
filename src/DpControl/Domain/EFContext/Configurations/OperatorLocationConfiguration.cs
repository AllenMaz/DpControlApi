using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata.Builders;
using DpControl.Domain.Entities;

namespace DpControl.Domain.EFContext.Configurations
{
    public class OperatorLocationConfiguration
    {
        public OperatorLocationConfiguration(EntityTypeBuilder<OperatorLocation> entityBuilder)
        {
            entityBuilder.ToTable("OperatorLocations", "ControlSystem");
            entityBuilder.HasKey(gl => new { gl.LocationId, gl.OperatorId });

            entityBuilder.HasOne(gl => gl.Location).WithMany(g => g.OperatorLocations).IsRequired(false);//.HasForeignKey(gl => gl.LocationId);
            entityBuilder.HasOne(gl => gl.Operator).WithMany(l => l.OperatorLocations).IsRequired(false);//.HasForeignKey(global => global.OperatorId);
        }
    }
}
