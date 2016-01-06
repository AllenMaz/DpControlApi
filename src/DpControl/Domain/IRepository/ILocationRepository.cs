using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;

namespace DpControl.Domain.IRepository
{
    public interface ILocationRepository
    {
        void Add(MLocation item);
        Task<IEnumerable<MLocation>> GetAllByProjectNo(string projectNo);
        Task<MLocationOnly> Find(string deviceNo, string projectNo);
        Task Remove(int  key, string projectNo);
        void Update(MLocation mcustomer);

    }
}
