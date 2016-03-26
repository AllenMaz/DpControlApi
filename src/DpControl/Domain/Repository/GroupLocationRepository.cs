using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.IRepository;
using DpControl.Domain.EFContext;
using DpControl.Domain.Entities;
using Microsoft.Data.Entity;
using DpControl.Domain.Models;
using DpControl.Domain.Execptions;

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

        public int Add(GroupLocationAddModel mGroupLocation)
        {
            var group = _context.Groups.FirstOrDefault(c => c.GroupId == mGroupLocation.GroupId);
            if (group == null)
                throw new ExpectException("Could not find Group data which GroupId equal to " + mGroupLocation.GroupId);

            var location = _context.Locations.FirstOrDefault(c => c.LocationId == mGroupLocation.LocationId);
            if (location == null)
                throw new ExpectException("Could not find Location data which LocationId equal to " + mGroupLocation.LocationId);

            //GroupId and LocationId must be unique
            var checkData =  _context.GroupLocations
                .Where(c => c.GroupId == mGroupLocation.GroupId
                                    && c.LocationId == mGroupLocation.LocationId).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("There is already exist data which GroupId equal to "
                    + mGroupLocation.GroupId + " and LocationId equal to " + mGroupLocation.LocationId);


            var model = new GroupLocation
            {
                GroupId = mGroupLocation.GroupId,
                LocationId = mGroupLocation.LocationId
            };
            _context.GroupLocations.Add(model);
            _context.SaveChanges();
            return model.GroupLocationId;
        }

        public async Task<int> AddAsync(GroupLocationAddModel mGroupLocation)
        {
            var group = _context.Groups.FirstOrDefault(c => c.GroupId == mGroupLocation.GroupId);
            if (group == null)
                throw new ExpectException("Could not find Group data which GroupId equal to " + mGroupLocation.GroupId);

            var location = _context.Locations.FirstOrDefault(c => c.LocationId == mGroupLocation.LocationId);
            if (location == null)
                throw new ExpectException("Could not find Location data which LocationId equal to " + mGroupLocation.LocationId);

            //GroupId and LocationId must be unique
            var checkData = await _context.GroupLocations
                .Where(c => c.GroupId == mGroupLocation.GroupId
                                    && c.LocationId == mGroupLocation.LocationId).ToListAsync();
            if (checkData.Count >0)
                throw new ExpectException("There is already exist data which GroupId equal to "
                    + mGroupLocation.GroupId+" and LocationId equal to "+mGroupLocation.LocationId);


            var model = new GroupLocation
            {
                GroupId = mGroupLocation.GroupId,
                LocationId = mGroupLocation.LocationId
            };
            _context.GroupLocations.Add(model);
            await _context.SaveChangesAsync();
            return model.GroupLocationId;
        }

        public GroupLocationSearchModel FindById(int groupLocationId)
        {
            var groupLocation = _context.GroupLocations
                .Where(v => v.GroupLocationId == groupLocationId)
                .Select(v => new GroupLocationSearchModel()
                {
                    GroupLocationId = v.GroupLocationId,
                    GroupId = v.GroupId,
                    LocationId = v.LocationId
                }).FirstOrDefault();

            return groupLocation;
        }

        public async Task<GroupLocationSearchModel> FindByIdAsync(int groupLocationId)
        {
            var groupLocation = await _context.GroupLocations
                .Where(v => v.GroupLocationId == groupLocationId)
                .Select(v => new GroupLocationSearchModel()
                {
                    GroupLocationId = v.GroupLocationId,
                    GroupId = v.GroupId,
                    LocationId = v.LocationId
                }).FirstOrDefaultAsync();

            return groupLocation;
        }

        public IEnumerable<GroupLocationSearchModel> GetAll()
        {
            var queryData = from G in _context.GroupLocations
                            select G;

            var result = QueryOperate<GroupLocation>.Execute(queryData);

            //以下执行完后才会去数据库中查询
            var groupLocations = result.ToList();

            var groupLocationsSearch = groupLocations.Select(v => new GroupLocationSearchModel
            {
                GroupLocationId = v.GroupLocationId,
                GroupId = v.GroupId,
                LocationId = v.LocationId
            });

            return groupLocationsSearch;
        }

        public async Task<IEnumerable<GroupLocationSearchModel>> GetAllAsync()
        {
            var queryData = from G in _context.GroupLocations
                            select G;

            var result = QueryOperate<GroupLocation>.Execute(queryData);

            //以下执行完后才会去数据库中查询
            var groupLocations = await result.ToListAsync();

            var groupLocationsSearch = groupLocations.Select(v => new GroupLocationSearchModel
            {
                GroupLocationId = v.GroupLocationId,
                GroupId = v.GroupId,
                LocationId = v.LocationId
            });

            return groupLocationsSearch;
        }

        public void RemoveById(int groupLocationId)
        {
            var groupLocation = _context.GroupLocations.FirstOrDefault(c => c.GroupLocationId == groupLocationId);
            if (groupLocation == null)
                throw new ExpectException("Could not find data which groupLocationId equal to " + groupLocationId);

            _context.Remove(groupLocation);
            _context.SaveChanges();
        }

        public async Task RemoveByIdAsync(int groupLocationId)
        {
            var groupLocation = _context.GroupLocations.FirstOrDefault(c => c.GroupLocationId == groupLocationId);
            if (groupLocation == null)
                throw new ExpectException("Could not find data which groupLocationId equal to " + groupLocationId);

            _context.Remove(groupLocation);
            await _context.SaveChangesAsync();
        }

        public int UpdateById(int groupLocationId, GroupLocationUpdateModel mGroupLocation)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateByIdAsync(int groupLocationId, GroupLocationUpdateModel mGroupLocation)
        {
            throw new NotImplementedException();
        }

        


    }
}
