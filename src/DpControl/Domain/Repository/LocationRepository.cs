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
            int custId;
            custId = await GetLocationIdByProjectNo(projectNo);
            return await _context.Locations.Where(l => l.CustomerId == custId).Select(l => new MLocation
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
            })
            .OrderBy(c => c.LocationId)
            .ToArrayAsync<MLocation>();
        }

        //finding an device location through device serial no
        public async Task<MLocationOnly> Find(string serialNo, string projectNo)
        {
            int custId = await GetLocationIdByProjectNo(projectNo);
            var location = await _context.Locations.FirstOrDefaultAsync(l => (l.DeviceSerialNo == serialNo) && (l.CustomerId == custId));
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

        public async void Add(MLocation customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException();
            }
            _context.Locations.Add(new Location
            {
                LocationId = customer.LocationId,
                Building = customer.Building,
                Floor = customer.Floor,
                RoomNo = customer.RoomNo,
                Orientation = customer.Orientation,
                InstallationNumber = customer.InstallationNumber,

                DeviceType = customer.DeviceType,
                CommMode = customer.CommMode,
                DeviceSerialNo = customer.DeviceSerialNo,
                CommAddress = customer.CommAddress,
                CurrentPosition = customer.CurrentPosition,
                FavorPositionFirst = customer.FavorPositionFirst,
                FavorPositionrSecond = customer.FavorPositionrSecond,
                FavorPositionThird = customer.FavorPositionThird,
                ModifiedDate=DateTime.Now
            });
            await _context.SaveChangesAsync();
        }

        public async void Update(MLocation mLocation)
        {
            var _location = await _context.Locations.FirstOrDefaultAsync(c => c.LocationId == mLocation.LocationId);
            if (_location == null)
            {
                throw new KeyNotFoundException();
            }
            _location.LocationId = mLocation.LocationId;
            _location.Building = mLocation.Building;
            _location.Floor = mLocation.Floor;
            _location.RoomNo = mLocation.RoomNo;
            _location.Orientation = mLocation.Orientation;
            _location.InstallationNumber = mLocation.InstallationNumber;

            _location.DeviceType = mLocation.DeviceType;
            _location.CommMode = mLocation.CommMode;
            _location.DeviceSerialNo = mLocation.DeviceSerialNo;
            _location.CommAddress = mLocation.CommAddress;
            _location.CurrentPosition = mLocation.CurrentPosition;
            _location.FavorPositionFirst = mLocation.FavorPositionFirst;
            _location.FavorPositionrSecond = mLocation.FavorPositionrSecond;
            _location.FavorPositionThird = mLocation.FavorPositionThird;
            _location.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }
        public async Task Remove(int key, string projectNo)
        {
            int custId = await GetLocationIdByProjectNo(projectNo);
            var location = await _context.Locations.FirstOrDefaultAsync(l => (l.LocationId == key) && (l.CustomerId == custId));
            if (location != null)
            {
                _context.Locations.Remove(location);
                await _context.SaveChangesAsync();
            }
            else {
                return;
            }
        }
        async Task<int> GetLocationIdByProjectNo(string projectNo)
        {
            var project = await _context.Customers.SingleAsync(c => c.ProjectNo == projectNo);
            if (project == null)
                throw new NullReferenceException();
            return project.CustomerId;
        }
    }
}
