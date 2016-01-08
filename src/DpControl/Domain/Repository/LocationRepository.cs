using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using DpControl.Domain.Entities;
using DpControl.Domain.Models;
using DpControl.Domain.IRepository;
using DpControl.Domain.EFContext;

namespace DpControl.Domain.Repository
{
    public class LocationRepository : ILocationRepository
    {
        ShadingContext _context;
        public LocationRepository()
        {

        }
        public LocationRepository(ShadingContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MLocation>> GetAllByProjectNo(string projectNo)
        {
            // get projectNo from Customer
            var query = await GetCustomerByProjectNo(projectNo);

            return query.DeviceLocations.Select(l => new MLocation
            {
                LocationId = l.LocationId,
                Building = l.Building,
                Floor = l.Floor,
                RoomNo = l.RoomNo,
                Orientation = l.Orientation,
                InstallationNumber = l.InstallationNumber,

                DeviceType = l.DeviceType,
                CommMode = l.CommMode,
                DeviceSerialNo = l.DeviceSerialNo,
                CommAddress = l.CommAddress,
                CurrentPosition = l.CurrentPosition,
                FavorPositionFirst = l.FavorPositionFirst,
                FavorPositionrSecond = l.FavorPositionrSecond,
                FavorPositionThird = l.FavorPositionThird
            }).ToList<MLocation>().OrderBy(m=>m.Building).ThenByDescending(m=>m.Floor).ThenByDescending(m=>m.Orientation);
        }

        //finding an device location through device serial no
        public async Task<MLocationOnly> Find(string serialNo, string projectNo)
        {
            int custId;
            var query = await GetCustomerByProjectNo(projectNo);

            custId = query.CustomerId;
            var location = query.DeviceLocations.FirstOrDefault(l => (l.DeviceSerialNo == serialNo) );
            if (location == null)
                throw new KeyNotFoundException();
            return new MLocationOnly
            {
                LocationId = location.LocationId,
                Building = location.Building,
                Floor = location.Floor,
                Orientation = location.Orientation,
                RoomNo = location.RoomNo,
                InstallationNumber = location.InstallationNumber
            };

        }

        public async Task Add(MLocation mLocation, string projectNo)
        {
            if (mLocation == null)
            {
                throw new ArgumentNullException();
            }

            int _customerId;

            // get groups with projectNo = projectNo
            var query = await GetCustomerByProjectNo(projectNo);
            _customerId = query.CustomerId;

            //// does the Name exist?
            //if (query.DeviceLocations.Select(g => g.GroupName).Contains(groupName))
            //{
            //    throw new Exception("The group already exist.");
            //}

            query.DeviceLocations.Add(new Location
            {
                Building = mLocation.Building,
                Floor = mLocation.Floor,
                RoomNo = mLocation.RoomNo,
                Orientation = mLocation.Orientation,
                InstallationNumber = mLocation.InstallationNumber,

                DeviceType = mLocation.DeviceType,
                CommMode = mLocation.CommMode,
                DeviceSerialNo = mLocation.DeviceSerialNo,
                CommAddress = mLocation.CommAddress,
                CurrentPosition = mLocation.CurrentPosition,
                FavorPositionFirst = mLocation.FavorPositionFirst,
                FavorPositionrSecond = mLocation.FavorPositionrSecond,
                FavorPositionThird = mLocation.FavorPositionThird,
                ModifiedDate = DateTime.Now,
                CustomerId = _customerId
            });
            await _context.SaveChangesAsync();
        }

        public async Task Update(MLocation mLocation, string projectNo)
        {
            if (mLocation == null)
            {
                throw new ArgumentNullException();
            }

            int _customerId;

            // get groups with projectNo = projectNo
            var query = await GetCustomerByProjectNo(projectNo);
            if (query == null)
            {
                throw new KeyNotFoundException();
            }
            _customerId = query.CustomerId;

            var _single = query.DeviceLocations.Where(l => l.LocationId == mLocation.LocationId).Single();

            _single.CustomerId = _customerId;
            _single.Building = mLocation.Building;
            _single.Floor = mLocation.Floor;
            _single.RoomNo = mLocation.RoomNo;
            _single.Orientation = mLocation.Orientation;
            _single.InstallationNumber = mLocation.InstallationNumber;

            _single.DeviceType = mLocation.DeviceType;
            _single.CommMode = mLocation.CommMode;
            _single.DeviceSerialNo = mLocation.DeviceSerialNo;
            _single.CommAddress = mLocation.CommAddress;
            _single.CurrentPosition = mLocation.CurrentPosition;
            _single.FavorPositionFirst = mLocation.FavorPositionFirst;
            _single.FavorPositionrSecond = mLocation.FavorPositionrSecond;
            _single.FavorPositionThird = mLocation.FavorPositionThird;
            _single.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }
        public async Task Remove(int key, string projectNo)
        {
            int _customerId;

            // get groups with projectNo = projectNo
            var query = await GetCustomerByProjectNo(projectNo);
            _customerId = query.CustomerId;


            var location = query.DeviceLocations.FirstOrDefault(l => (l.LocationId == key) && (l.CustomerId == _customerId));
            if (location != null)
            {
                _context.Locations.Remove(location);
                await _context.SaveChangesAsync();
            }
            else {
                return;
            }
        }

        async Task<Customer> GetCustomerByProjectNo(string projectNo)
        {
            return await _context.Customers
                        .Include(c => c.DeviceLocations)
                        .Where(c => c.ProjectNo == projectNo)
                        .SingleAsync();
        }
    }
}
