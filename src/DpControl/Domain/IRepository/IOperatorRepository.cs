using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;

namespace DpControl.Domain.IRepository
{
    interface IOperatorRepository
    {
        Task Add(MOperator mOperator, string ProjectNo);
        Task<IEnumerable<MOperator>> GetAllAsync(string projectNo);
        Task Remove(int Id);
        Task UpdateById(MOperator mOperator, string projectNo);
    }
}
