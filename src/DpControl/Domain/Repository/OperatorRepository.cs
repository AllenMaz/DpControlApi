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
            if (string.IsNullOrWhiteSpace(projectNo) || mOperator==null)
            {
                throw new ArgumentNullException();
            }

            // get projectNo from Operator
            var _customer = await _context.Customers
                .Include(c => c.Operators)
                .Where(c => c.ProjectNo == projectNo)
                .SingleAsync();

            Operator _operator = new Operator
            {
                FirstName=mOperator.FirstName,
                LastName=mOperator.LastName,
                NickName=mOperator.NickName,
                Description=mOperator.Description,
                Password=mOperator.Password,
                ModifiedDate = DateTime.Now,
                CustomerId = _customer.CustomerId
            };
            _context.Operators.Add(_operator);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MOperator>> GetAllAsync(string projectNo)
        {
            if (string.IsNullOrWhiteSpace(projectNo) )
            {
                throw new ArgumentNullException();
            }

            // get projectNo from Operator
            var _customer = await _context.Customers
                .Include(c => c.Operators)
                .Where(c => c.ProjectNo == projectNo)
                .SingleAsync();

            return _customer.Operators.Select(o => new MOperator
            {
                OperatorId=  o.OperatorId,
                FirstName = o.LastName,
                LastName=o.LastName,
                NickName=o.NickName,
                Description=o.Description
            })
            .ToList().OrderBy(o=>o.LastName);
        }

        public async Task Remove(int Id)
        {
            if (Id == 0)
            {
                throw new Exception("The group does not exist.");
            }

            var toDelete = new Operator { OperatorId = Id };
            _context.Operators.Attach(toDelete);

            // remove data in related table - OperatorLocation  - optional relationship with data to be deleted
            var _operatorLocation = _context.OperatorLocation.Where(ol => ol.OperatorId == Id);
            foreach (var ol in _operatorLocation)
            {
                _context.OperatorLocation.Remove(ol);
            }

            // remove data in related table - GroupOperator - optional relationship with data to be deleted
            var _groupOperator = _context.GroupOperators.Where(ol => ol.OperatorId == Id);
            foreach (var ol in _groupOperator)
            {
                _context.GroupOperators.Remove(ol);
            }

            //remove data in related table - Logs - optional relationship with data undeleted (set to Null), just load data into memory
            _context.Logs.Where(l => l.OperatorId == Id).Load();
            
            _context.Operators.Remove(toDelete);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateById(MOperator mOperator, string projectNo)
        {
            var _customer = await _context.Customers
                .Include(c => c.Operators)
                .Where(c => c.ProjectNo == projectNo)
                .SingleAsync();

            var _single = _customer.Operators.Where(g => g.OperatorId == mOperator.OperatorId).Single();

            _single.OperatorId = mOperator.OperatorId;
            _single.FirstName = mOperator.FirstName;
            _single.LastName = mOperator.LastName;
            _single.NickName = mOperator.NickName;
            _single.Description = mOperator.Description;

            await _context.SaveChangesAsync();
        }

        //async Task<Customer> GetCustomerByProjectNo(string projectNo)
        //{
        //    var query = await _context.Customers
        //                .Include(c => c.Operators)
        //                .Where(c => c.ProjectNo == projectNo)
        //                .SingleAsync();
        //    if (query == null)
        //    {
        //        throw new KeyNotFoundException();
        //    }
        //    return query;
        //}
    }
}
