using DpControl.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;
using DpControl.Domain.EFContext;
using DpControl.Domain.Execptions;
using Microsoft.Data.Entity;
using DpControl.Domain.Entities;

namespace DpControl.Domain.Repository
{
    public class UserLocationRepository : IUserLocationRepository
    {
        private ShadingContext _context;

        #region Constructors
        public UserLocationRepository()
        {
        }

        public UserLocationRepository(ShadingContext dbContext)
        {
            _context = dbContext;
        }

        #endregion

        public int Add(UserLocationAddModel mUserLocation)
        {
            var location = _context.Locations.FirstOrDefault(c => c.LocationId == mUserLocation.LocationId);
            if (location == null)
                throw new ExpectException("Could not find Location data which LocationId equal to " + mUserLocation.LocationId);

            var user = _context.Users.FirstOrDefault(c => c.Id == mUserLocation.UserId);
            if (user == null)
                throw new ExpectException("Could not find User data which UserId equal to '" + mUserLocation.UserId + "'");

            //LocationId and UserId must be unique
            var checkData = _context.UserLocations
                .Where(c => c.LocationId == mUserLocation.LocationId
                                    && c.UserId == mUserLocation.UserId).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("There is already exist data which LocationId equal to "
                    + mUserLocation.LocationId + " and UserId equal to '" + mUserLocation.UserId + "'");


            var model = new UserLocation
            {
                LocationId = mUserLocation.LocationId,
                UserId = mUserLocation.UserId
            };
            _context.UserLocations.Add(model);
            _context.SaveChanges();
            return model.UserLocationId;
        }

        public async Task<int> AddAsync(UserLocationAddModel mUserLocation)
        {
            var location = _context.Locations.FirstOrDefault(c => c.LocationId == mUserLocation.LocationId);
            if (location == null)
                throw new ExpectException("Could not find Location data which LocationId equal to " + mUserLocation.LocationId);

            var user = _context.Users.FirstOrDefault(c => c.Id == mUserLocation.UserId);
            if (user == null)
                throw new ExpectException("Could not find User data which UserId equal to '" + mUserLocation.UserId + "'");

            //LocationId and UserId must be unique
            var checkData = await _context.UserLocations
                .Where(c => c.LocationId == mUserLocation.LocationId
                                    && c.UserId == mUserLocation.UserId).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("There is already exist data which LocationId equal to "
                    + mUserLocation.LocationId + " and UserId equal to '" + mUserLocation.UserId + "'");


            var model = new UserLocation
            {
                LocationId = mUserLocation.LocationId,
                UserId = mUserLocation.UserId
            };
            _context.UserLocations.Add(model);
            await _context.SaveChangesAsync();
            return model.UserLocationId;
        }

        public UserLocationSearchModel FindById(int userLocationId)
        {
            var userLocation = _context.UserLocations
               .Where(v => v.UserLocationId == userLocationId)
               .Select(v => new UserLocationSearchModel()
               {
                   UserLocationId = v.UserLocationId,
                   LocationId = v.LocationId,
                   UserId = v.UserId
               }).FirstOrDefault();

            return userLocation;
        }

        public async Task<UserLocationSearchModel> FindByIdAsync(int userLocationId)
        {
            var userLocation = await _context.UserLocations
               .Where(v => v.UserLocationId == userLocationId)
               .Select(v => new UserLocationSearchModel()
               {
                   UserLocationId = v.UserLocationId,
                   LocationId = v.LocationId,
                   UserId = v.UserId
               }).FirstOrDefaultAsync();

            return userLocation;
        }

        public IEnumerable<UserLocationSearchModel> GetAll(Query query)
        {
            var queryData = from U in _context.UserLocations
                            select U;

            var result = QueryOperate<UserLocation>.Execute(queryData, query);

            //以下执行完后才会去数据库中查询
            var userLocations = result.ToList();

            var userLocationsSearch = userLocations.Select(v => new UserLocationSearchModel
            {
                UserLocationId = v.UserLocationId,
                LocationId = v.LocationId,
                UserId = v.UserId
            });

            return userLocationsSearch;
        }

        public async Task<IEnumerable<UserLocationSearchModel>> GetAllAsync(Query query)
        {
            var queryData = from U in _context.UserLocations
                            select U;

            var result = QueryOperate<UserLocation>.Execute(queryData, query);

            //以下执行完后才会去数据库中查询
            var userLocations = await result.ToListAsync();

            var userLocationsSearch = userLocations.Select(v => new UserLocationSearchModel
            {
                UserLocationId = v.UserLocationId,
                LocationId = v.LocationId,
                UserId = v.UserId
            });

            return userLocationsSearch;
        }

        public void RemoveById(int userLocationId)
        {
            var userLocation = _context.UserLocations.FirstOrDefault(c => c.UserLocationId == userLocationId);
            if (userLocation == null)
                throw new ExpectException("Could not find data which UserLocationId equal to " + userLocationId);

            _context.Remove(userLocation);
            _context.SaveChanges();
        }

        public async Task RemoveByIdAsync(int userLocationId)
        {
            var userLocation = _context.UserLocations.FirstOrDefault(c => c.UserLocationId == userLocationId);
            if (userLocation == null)
                throw new ExpectException("Could not find data which UserLocationId equal to " + userLocationId);

            _context.Remove(userLocation);
            await _context.SaveChangesAsync();
        }

        public int UpdateById(int userLocationId, UserLocationUpdateModel mUserLocation)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateByIdAsync(int userLocationId, UserLocationUpdateModel mUserLocation)
        {
            throw new NotImplementedException();
        }
    }
}
