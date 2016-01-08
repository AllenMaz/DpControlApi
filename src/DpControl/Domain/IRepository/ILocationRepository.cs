using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;

namespace DpControl.Domain.IRepository
{
    public interface ILocationRepository
    {
        Task Add(MLocation item, string projectNo);
        Task<IEnumerable<MLocation>> GetAllByProjectNo(string projectNo);
        Task<MLocationOnly> Find(string deviceNo, string projectNo);
        Task Remove(int  key, string projectNo);
        Task Update(MLocation mcustomer, string projectNo);
    }
}
