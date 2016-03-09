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
    public class UserGroupConfiguration
    {
        public UserGroupConfiguration(EntityTypeBuilder<UserGroup> entityBuilder)
        {
            entityBuilder.ToTable("UserGroups");
            entityBuilder.HasKey(gl => new { gl.GroupId,gl.UserId});

            entityBuilder.HasOne(gl => gl.Group).WithMany(g => g.UserGroups)
                .HasForeignKey(g=>g.GroupId);
            entityBuilder.HasOne(gl => gl.User).WithMany(l => l.UserGroups)
                .HasForeignKey(g=>g.UserId);
        }
    }
}
