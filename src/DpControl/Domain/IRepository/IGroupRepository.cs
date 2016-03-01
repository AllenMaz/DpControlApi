﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;

namespace DpControl.Domain.IRepository
{
    public interface IGroupRepository:IBaseRepository<GroupAddModel,GroupUpdateModel,GroupSearchModel>
    {
    }
}   
