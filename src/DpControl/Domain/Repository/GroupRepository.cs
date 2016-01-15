using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;
using DpControl.Domain.IRepository;
using DpControl.Domain.Entities;
using DpControl.Domain.EFContext;
using Microsoft.Data.Entity;

namespace DpControl.Domain.Repository
{
    public class GroupRepository : IGroupRepository
    {
        private ShadingContext _context;

        #region Constructors
        public GroupRepository()
        {
        }

        public GroupRepository(ShadingContext dbContext)
        {
            _context = dbContext;
        }
        #endregion

        public async Task Add(string groupName, string projectNo)
        {
            if (groupName == null || string.IsNullOrEmpty(projectNo))
            {
                throw new ArgumentNullException();
            }

            // the GroupName is set as  an Index, so the same name will be validated in database

            var _customer = _context.Customers.Single(c => c.ProjectNo == projectNo);

            // create new Group
            _context.Groups.Add(new Group
            {
                GroupName = groupName,
                ModifiedDate = DateTime.Now,
                CustomerId = _customer.CustomerId
            });
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MGroup>> GetAllAsync(string projectNo)
        {
            var _customer= await _context.Customers
                        .Include(c => c.Groups)
                        .Where(c => c.ProjectNo == projectNo)
                        .SingleAsync();

            return _customer.Groups.Select(g => new MGroup
                    {
                        GroupId = g.GroupId,
                        GroupName = g.GroupName
                    }).ToList<MGroup>();
        }

        public async Task RemoveByName(string groupName, string projectNo)
        {
            //           var query = await GetCustomerByProjectNo(projectNo);
            var _single = _context.Customers
                       .Include(c => c.Groups)
                       .Where(c => c.ProjectNo == projectNo)
                       .Single()
                       .Groups.Where(g => g.GroupName == groupName).Single();       // InvalidOperationException, if no element, 

            // remove data in related table - GroupLocation
            var _groupLocation = _context.GroupLocations.Where(gl => gl.GroupId == _single.GroupId);
            foreach(var gl in _groupLocation)
            {
                _context.GroupLocations.Remove(gl);
            }

            // remove data in related table - GroupOperator
            var _groupOperator = _context.GroupOperators.Where(gl => gl.GroupId == _single.GroupId);
            foreach (var gl in _groupOperator)
            {
                _context.GroupOperators.Remove(gl);
            }

            _context.Remove(_single);
            await _context.SaveChangesAsync();

            // remove 
        }

        public async Task RemoveById(int Id)
        {
            if (Id == 0)
            {
                throw new Exception("The group does not exist.");
            }

            var toDelete = new Group { GroupId = Id };
            _context.Groups.Attach(toDelete);

            // remove data in related table - GroupLocation
            var _groupLocation = _context.GroupLocations.Where(gl => gl.GroupId ==Id);
            foreach (var gl in _groupLocation)
            {
                _context.GroupLocations.Remove(gl);
            }

            // remove data in related table - GroupOperator
            var _groupOperator = _context.GroupOperators.Where(gl => gl.GroupId == Id);
            foreach (var gl in _groupOperator)
            {
                _context.GroupOperators.Remove(gl);
            }

            _context.Groups.Remove(toDelete);
            await _context.SaveChangesAsync();

            //            _context.Database.ExecuteSqlCommandAsync("Delete From operators where OperatorId = Id");
        }

        public async Task Update(MGroup mGroup, string projectNo)
        {
            var _single =_context.Customers
                        .Include(c => c.Groups)
                        .Where(c => c.ProjectNo == projectNo)
                        .Single()
                        .Groups.Where(g => g.GroupId == mGroup.GroupId).Single();       // InvalidOperationException, if no element, 
            _single.GroupName = mGroup.GroupName;
            _context.Groups.Update(_single);
            await _context.SaveChangesAsync();
        }

        //Customer GetCustomerByProjectNo(string projectNo)
        //{
        //    var query =  _context.Customers
        //                .Include(c => c.Groups)
        //                .Where(c => c.ProjectNo == projectNo)
        //                .Single();
        //    return query;
        //}
    }
}
