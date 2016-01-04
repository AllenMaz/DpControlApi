using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.IRepository;
using DpControl.Domain.EFContext;

namespace DpControl.Domain.Repository
{
    public class EFContextFactory : IEFContextFactory
    {
        private readonly Microsoft.Data.Entity.DbContext dbContext;
        public EFContextFactory()
        {
            dbContext = new ShadingContext();
        }

        public Microsoft.Data.Entity.DbContext GetContext()
        {
            return dbContext;
        }

    }
}
