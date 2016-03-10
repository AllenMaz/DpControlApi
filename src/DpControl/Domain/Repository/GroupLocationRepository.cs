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

            //As primary key ,GroupId and LocationId must be unique
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

        public GroupLocationSearchModel FindById(int groupDeviceLocationId)
        {
            var groupDeviceLocation = _context.GroupLocations
                .Where(v => v.GroupLocationId == groupDeviceLocationId)
                .Select(v => new GroupLocationSearchModel()
                {
                    GroupLocationId = v.GroupLocationId,
                    GroupId = v.GroupId,
                    LocationId = v.LocationId
                }).FirstOrDefault();

            return groupDeviceLocation;
        }

        public async Task<GroupLocationSearchModel> FindByIdAsync(int groupDeviceLocationId)
        {
            var groupDeviceLocation = await _context.GroupLocations
                .Where(v => v.GroupLocationId == groupDeviceLocationId)
                .Select(v => new GroupLocationSearchModel()
                {
                    GroupLocationId = v.GroupLocationId,
                    GroupId = v.GroupId,
                    LocationId = v.LocationId
                }).FirstOrDefaultAsync();

            return groupDeviceLocation;
        }

        public IEnumerable<GroupLocationSearchModel> GetAll(Query query)
        {
            var queryData = from G in _context.GroupLocations
                            select G;

            var result = QueryOperate<GroupLocation>.Execute(queryData, query);

            //以下执行完后才会去数据库中查询
            var scenes = result.ToList();

            var groupDeviceLocationsSearch = scenes.Select(v => new GroupLocationSearchModel
            {
                GroupLocationId = v.GroupLocationId,
                GroupId = v.GroupId,
                LocationId = v.LocationId
            });

            return groupDeviceLocationsSearch;
        }

        public async Task<IEnumerable<GroupLocationSearchModel>> GetAllAsync(Query query)
        {
            var queryData = from G in _context.GroupLocations
                            select G;

            var result = QueryOperate<GroupLocation>.Execute(queryData, query);

            //以下执行完后才会去数据库中查询
            var scenes = await result.ToListAsync();

            var groupDeviceLocationsSearch = scenes.Select(v => new GroupLocationSearchModel
            {
                GroupLocationId = v.GroupLocationId,
                GroupId = v.GroupId,
                LocationId = v.LocationId
            });

            return groupDeviceLocationsSearch;
        }

        public void RemoveById(int groupDeviceLocationId)
        {
            var groupDeviceLocation = _context.GroupLocations.FirstOrDefault(c => c.GroupLocationId == groupDeviceLocationId);
            if (groupDeviceLocation == null)
                throw new ExpectException("Could not find data which groupDeviceLocationId equal to " + groupDeviceLocationId);

            _context.Remove(groupDeviceLocation);
            _context.SaveChanges();
        }

        public async Task RemoveByIdAsync(int groupDeviceLocationId)
        {
            var groupDeviceLocation = _context.GroupLocations.FirstOrDefault(c => c.GroupLocationId == groupDeviceLocationId);
            if (groupDeviceLocation == null)
                throw new ExpectException("Could not find data which groupDeviceLocationId equal to " + groupDeviceLocationId);

            _context.Remove(groupDeviceLocation);
            await _context.SaveChangesAsync();
        }

        public int UpdateById(int groupDeviceLocationId, GroupLocationUpdateModel mGroupLocation)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateByIdAsync(int groupDeviceLocationId, GroupLocationUpdateModel mGroupLocation)
        {
            throw new NotImplementedException();
        }

        #endregion


    }
}
