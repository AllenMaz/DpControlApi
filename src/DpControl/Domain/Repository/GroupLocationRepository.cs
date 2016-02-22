using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.IRepository;
using DpControl.Domain.EFContext;
using DpControl.Domain.Entities;
using Microsoft.Data.Entity;

namespace DpControl.Domain.Repository
{
    public class GroupLocationRepository : IGroupLocationRepository
    {
        private ShadingContext _context;

        #region Constructors
        public GroupLocationRepository()
        {
        }

        public GroupLocationRepository(ShadingContext dbContext)
        {
            _context = dbContext;
        }

        #endregion

        public async Task Add(int groupId, int locationId)
        {
            _context.Add(new GroupLocation
            {
                GroupId = groupId,
                LocationId = locationId
            });

            await _context.SaveChangesAsync();
        }
        //public async Task UpdateByGroupId(int groupId, int locationId)
        //{
        //}

    }
}
