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
    public class SceneConfiguration
    {
        public SceneConfiguration(EntityTypeBuilder<Scene> entityBuilder)
        {
            entityBuilder.ToTable("Scenes", "ControlSystem");
            entityBuilder.HasKey(s => s.SceneId);
            entityBuilder.Property(s => s.SceneName).IsRequired().HasMaxLength(50);
            entityBuilder.Property(s => s.Enable).IsRequired().HasDefaultValue(false);
            entityBuilder.Property(s => s.Creator).IsRequired();
            entityBuilder.Property(s => s.CreateDate).IsRequired();
            entityBuilder.Property(s => s.RowVersion).ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();

            //SetNull will be set for optional relationships
            entityBuilder.HasMany(s => s.Groups).WithOne(s => s.Scene).HasForeignKey(s => s.SceneId).IsRequired(false);
            //Cascade delete will be set to Cascade for required relationships
            entityBuilder.HasMany(s => s.SceneSegments).WithOne(s => s.Scene).HasForeignKey(s => s.SceneId);
        }
    }
}
