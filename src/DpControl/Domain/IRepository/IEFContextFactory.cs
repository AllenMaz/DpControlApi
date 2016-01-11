using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.IRepository
{
    public interface IEFContextFactory
    {
        Microsoft.Data.Entity.DbContext GetContext();
    }
}
