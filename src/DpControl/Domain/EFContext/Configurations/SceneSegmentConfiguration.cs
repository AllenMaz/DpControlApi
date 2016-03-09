using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata.Builders;
using DpControl.Domain.Entities;
    
namespace DpControl.Domain.EFContext.Configurations
{
    public class SceneSegmentConfiguration
    {
        public SceneSegmentConfiguration(EntityTypeBuilder<SceneSegment> entityBuilder)
        {
            entityBuilder.ToTable("SceneSegments");
            entityBuilder.HasKey(s => s.SceneSegmentId);

            entityBuilder.Property(s => s.SequenceNo).IsRequired();
            entityBuilder.Property(s => s.StartTime).HasMaxLength(10).IsRequired();
            entityBuilder.Property(s => s.Creator).IsRequired();
            entityBuilder.Property(s => s.CreateDate).IsRequired();
            entityBuilder.Property(s => s.RowVersion).ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();

        }
    }
}
