﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.IRepository;
using DpControl.Domain.EFContext;
using Microsoft.Data.Entity;

namespace DpControl.Domain.Repository
{
    public class EFContextFactory : IEFContextFactory
    {
        private readonly ShadingContext dbContext;
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
