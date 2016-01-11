using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.IRepository;
using DpControl.Domain.Entities;
using DpControl.Domain.EFContext;
using Microsoft.Data.Entity;
using DpControl.Domain.Models;


namespace DpControl.Domain.Repository
{
    public class OperatorRepository : IOperatorRepository
    {
        ShadingContext _context;
        public OperatorRepository()
        {

        }
        public OperatorRepository(ShadingContext context)
        {
            _context = context;
        }

        public async Task Add(MOperator mOperator, string projectNo)
        {
            string _fullname = mOperator.FirstName + mOperator.LastName;
            int _customerId;

            // get groups with projectNo = projectNo
            var query = await GetCustomerByProjectNo(projectNo);

            _customerId = query.CustomerId;
            // does the Name exist?
            if (query.Operators.Select(o => o.FirstName + o.LastName).Contains(_fullname))
            {
                throw new Exception("The group already exist.");
            }

            // create new Group
            Operator _operator = new Operator
            {
                FirstName=mOperator.FirstName,
                LastName=mOperator.LastName,
                NickName=mOperator.NickName,
                Description=mOperator.Description,
                Password=mOperator.Password,
                ModifiedDate = DateTime.Now,
                CustomerId = _customerId
            };
            _context.Operators.Add(_operator);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MOperator>> GetAllAsync(string projectNo)
        {
            // get groups by the projectNo
            var query = await GetCustomerByProjectNo(projectNo);

            return query.Operators.Select(o => new MOperator
            {
                OperatorId=  o.OperatorId,
                FirstName = o.LastName,
                LastName=o.LastName,
                NickName=o.NickName,
                Description=o.Description
            })
            .ToList<MOperator>();
        }

        public async Task Remove(int Id)
        {
            if (Id == 0)
            {
                throw new Exception("The group does not exist.");
            }

            var toDelete = new Operator { OperatorId = Id };
            _context.Operators.Attach(toDelete);
            _context.Operators.Remove(toDelete);
            await _context.SaveChangesAsync();

//            _context.Database.ExecuteSqlCommandAsync("Delete From operators where OperatorId = Id");
        }

        public async Task UpdateById(MOperator mOperator, string projectNo)
        {
            // get groups by the projectNo
            var query = await GetCustomerByProjectNo(projectNo);

            var _single = query.Operators.Where(g => g.OperatorId == mOperator.OperatorId).Single();
            if (_single == null)
            {
                throw new KeyNotFoundException();
            }
            _single.OperatorId = mOperator.OperatorId;
            _single.FirstName = mOperator.FirstName;
            _single.LastName = mOperator.LastName;
            _single.NickName = mOperator.NickName;
            _single.Description = mOperator.Description;
//            _context.Operators.Update(_single);
            await _context.SaveChangesAsync();
        }

        async Task<Customer> GetCustomerByProjectNo(string projectNo)
        {
            var query = await _context.Customers
                        .Include(c => c.Operators)
                        .Where(c => c.ProjectNo == projectNo)
                        .SingleAsync();
            if (query == null)
            {
                throw new KeyNotFoundException();
            }
            return query;
        }
    }
}
