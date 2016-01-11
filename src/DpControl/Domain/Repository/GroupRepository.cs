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
    public class GroupRepository 
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
            if (groupName == null || projectNo==null)
            {
                throw new ArgumentNullException();
            }

            int _customerId;

            // get groups with projectNo = projectNo
            var query = await GetCustomerByProjectNo(projectNo);

            _customerId = query.CustomerId;

            // does the Name exist?
            if (query.Groups.Select(g => g.GroupName).Contains(groupName))
            {
                throw new Exception("The group already exist.");
            }

            // create new Group
            Group group = new Group
            {
                GroupName = groupName,
                ModifiedDate = DateTime.Now,
                CustomerId = _customerId
            };
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MGroup>> GetAllAsync(string projectNo)
        {
            // get groups by the projectNo
            var query = await GetCustomerByProjectNo(projectNo);

            return query.Groups.Select(g => new MGroup
            {
                GroupId = g.GroupId,
                GroupName = g.GroupName
            })
            .ToList<MGroup>();
        }

        public async Task Remove(string groupName, string projectNo)
        {
            var query = await GetCustomerByProjectNo(projectNo);

            var removeItem = query.Groups.Single(g => g.GroupName == groupName);
            if (removeItem == null)
            {
                throw new Exception("The group does not exist.");
            }
            else
            {
                _context.Remove(removeItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Remove(int Id)
        {
            if (Id == 0)
            {
                throw new Exception("The group does not exist.");
            }

            var toDelete = new Group { GroupId = Id };
            _context.Groups.Attach(toDelete);
            _context.Groups.Remove(toDelete);
            await _context.SaveChangesAsync();

            //            _context.Database.ExecuteSqlCommandAsync("Delete From operators where OperatorId = Id");
        }


        public async Task UpdateById(MGroup mGroup, string projectNo)
        {
            // get groups by the projectNo
            var query = await GetCustomerByProjectNo(projectNo);

            var _single = query.Groups.Where(g => g.GroupId == mGroup.GroupId).Single();
            if (_single == null)
            {
                throw new KeyNotFoundException();
            }
            _single.GroupName = mGroup.GroupName;
            _context.Groups.Update(_single);
            await _context.SaveChangesAsync();
        }

        async Task<Customer> GetCustomerByProjectNo(string projectNo)
        {
            var query = await _context.Customers
                        .Include(c => c.Groups)
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
