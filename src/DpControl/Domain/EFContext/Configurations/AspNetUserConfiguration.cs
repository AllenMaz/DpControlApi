using DpControl.Domain.Entities;
using Microsoft.Data.Entity.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.EFContext.Configurations
{
    public class AspNetUserConfiguration
    {
        public AspNetUserConfiguration(EntityTypeBuilder<ApplicationUser> entityBuilder)
        {
           
        }
    }

}
