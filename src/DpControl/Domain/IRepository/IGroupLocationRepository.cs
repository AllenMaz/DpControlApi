using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.IRepository
{
    public interface IGroupLocationRepository
    {
        Task Add(int groupId, int locationId);
        //Task UpdateByGroupId(int groupId, int locationId);
    }
}
