using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata.Builders;
using DpControl.Domain.Entities;

namespace DpControl.Domain.EFContext.Configurations
{
    public class GroupLocationConfiguration
    {
        public GroupLocationConfiguration(EntityTypeBuilder<GroupLocation> entityBuilder)
        {
            entityBuilder.ToTable("GroupLocations", "ControlSystem");
            entityBuilder.HasKey(gl => new { gl.GroupId, gl.LocationId });
            entityBuilder.HasOne(gl => gl.Group).WithMany(g => g.GroupLocations).IsRequired(false);//.HasForeignKey(gl => gl.GroupId);
            entityBuilder.HasOne(gl => gl.Location).WithMany(l => l.GroupLocations).IsRequired(false);//.HasForeignKey(global => global.LocationId);
        }
    }
}
