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
    public class OperatorRepository
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
            var query = await _context.Customers
                    .Include(c => c.Operators)
                    .Where(c => c.ProjectNo == projectNo)
                    .SingleAsync();
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
            var query = await _context.Customers
                .Include(c => c.Operators)
                .Where(c => c.ProjectNo == projectNo)
                .SingleAsync();

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

        public async Task Remove(string fullName, string projectNo)
        {
            var query = await _context.Customers
                        .Include(c => c.Operators)
                        .Where(c => c.ProjectNo == projectNo)
                        .SingleAsync();
            var removeItem = query.Operators.Single(o => (o.FirstName + o.LastName) == fullName);
            if (removeItem == null)
            {
                throw new Exception("The group does not exist.");
            }
            else
            {
                _context.Operators.Remove(removeItem);
                await _context.SaveChangesAsync();
            }
        }

    }
}
