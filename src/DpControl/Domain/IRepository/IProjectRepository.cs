using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DpControl.Domain.Models;

namespace DpControl.Domain.IRepository
{
    public interface IProjectRepository:IBaseRepository<ProjectAddModel,ProjectUpdateModel,ProjectSearchModel>
    {
    }
}
