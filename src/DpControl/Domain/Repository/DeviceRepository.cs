using DpControl.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Models;
using DpControl.Domain.EFContext;
using DpControl.Domain.Entities;
using Microsoft.Data.Entity;
using DpControl.Domain.Execptions;

namespace DpControl.Domain.Repository
{
    public class DeviceRepository : IDeviceRepository
    {
        ShadingContext _context;

        #region
        public DeviceRepository()
        {

        }
        public DeviceRepository(ShadingContext context)
        {
            _context = context;
        }
        
        #endregion

        public int Add(DeviceAddModel mDevice)
        {
            var model = new Device
            {
                Voltage = mDevice.Voltage,
                Diameter = mDevice.Diameter,
                Torque = mDevice.Torque,

            };

            _context.Devices.Add(model);

            _context.SaveChanges();
            return model.DeviceId;
        }

        public async Task<int> AddAsync(DeviceAddModel mDevice)
        {
            var model = new Device
            {
                Voltage = mDevice.Voltage,
                Diameter = mDevice.Diameter,
                Torque = mDevice.Torque,
                
            };

            _context.Devices.Add(model);

            await _context.SaveChangesAsync();
            return model.DeviceId;
        }

        public DeviceSearchModel FindById(int deviceId)
        {
            var device = _context.Devices.Where(v => v.DeviceId == deviceId)
              .Select(v => new DeviceSearchModel()
              {
                  DeviceId = v.DeviceId,
                  Voltage = v.Voltage,
                  Diameter = v.Diameter,
                  Torque = v.Torque,
                  Locations = v.Locations.Select(p => new LocationSubSearchModel()
                  {
                      LocationId = p.LocationId,
                      ProjectId = p.ProjectId,
                      Building = p.Building,
                      CommAddress = p.CommAddress,
                      CommMode = p.CommMode,
                      CurrentPosition = p.CurrentPosition,
                      Description = p.Description,
                      DeviceSerialNo = p.DeviceSerialNo,
                      DeviceId = p.DeviceId,
                      DeviceType = p.DeviceType,
                      FavorPositionFirst = p.FavorPositionFirst,
                      FavorPositionrSecond = p.FavorPositionrSecond,
                      FavorPositionThird = p.FavorPositionThird,
                      Floor = p.Floor,
                      InstallationNumber = p.InstallationNumber,
                      Orientation = p.Orientation,
                      RoomNo = p.RoomNo,
                      Creator = p.Creator,
                      CreateDate = p.CreateDate,
                      Modifier = p.Modifier,
                      ModifiedDate = p.ModifiedDate
                  })
              }).FirstOrDefault();

            return device;
        }

        public async Task<DeviceSearchModel> FindByIdAsync(int deviceId)
        {
            var device = await _context.Devices.Where(v => v.DeviceId == deviceId)
               .Select(v => new DeviceSearchModel()
               {
                   DeviceId = v.DeviceId,
                   Voltage = v.Voltage,
                   Diameter = v.Diameter,
                   Torque = v.Torque,
                   Locations = v.Locations.Select(p => new LocationSubSearchModel()
                   {
                       LocationId = p.LocationId,
                       ProjectId = p.ProjectId,
                       Building = p.Building,
                       CommAddress = p.CommAddress,
                       CommMode = p.CommMode,
                       CurrentPosition = p.CurrentPosition,
                       Description = p.Description,
                       DeviceSerialNo = p.DeviceSerialNo,
                       DeviceId = p.DeviceId,
                       DeviceType = p.DeviceType,
                       FavorPositionFirst = p.FavorPositionFirst,
                       FavorPositionrSecond = p.FavorPositionrSecond,
                       FavorPositionThird = p.FavorPositionThird,
                       Floor = p.Floor,
                       InstallationNumber = p.InstallationNumber,
                       Orientation = p.Orientation,
                       RoomNo = p.RoomNo,
                       Creator = p.Creator,
                       CreateDate = p.CreateDate,
                       Modifier = p.Modifier,
                       ModifiedDate = p.ModifiedDate
                   })
               }).FirstOrDefaultAsync();

            return device;
        }

        public IEnumerable<DeviceSearchModel> GetAll(Query query)
        {
            var queryData = from D in _context.Devices
                            select D;

            var result = QueryOperate<Device>.Execute(queryData, query);
            var needExpandLocations = ExpandOperator.NeedExpand("Locations", query.expand);
            if (needExpandLocations)
                result = result.Include(d => d.Locations);

            //以下执行完后才会去数据库中查询
            var devices = result.ToList();

            var devicesSearch = devices.Select(v => new DeviceSearchModel
            {
                DeviceId = v.DeviceId,
                Voltage = v.Voltage,
                Diameter = v.Diameter,
                Torque = v.Torque,
                Locations = v.Locations.Select(p => new LocationSubSearchModel()
                {
                    LocationId = p.LocationId,
                    ProjectId = p.ProjectId,
                    Building = p.Building,
                    CommAddress = p.CommAddress,
                    CommMode = p.CommMode,
                    CurrentPosition = p.CurrentPosition,
                    Description = p.Description,
                    DeviceSerialNo = p.DeviceSerialNo,
                    DeviceId = p.DeviceId,
                    DeviceType = p.DeviceType,
                    FavorPositionFirst = p.FavorPositionFirst,
                    FavorPositionrSecond = p.FavorPositionrSecond,
                    FavorPositionThird = p.FavorPositionThird,
                    Floor = p.Floor,
                    InstallationNumber = p.InstallationNumber,
                    Orientation = p.Orientation,
                    RoomNo = p.RoomNo,
                    Creator = p.Creator,
                    CreateDate = p.CreateDate,
                    Modifier = p.Modifier,
                    ModifiedDate = p.ModifiedDate
                })
            });

            return devicesSearch;
        }

        public async Task<IEnumerable<DeviceSearchModel>> GetAllAsync(Query query)
        {
            var queryData = from D in _context.Devices
                            select D;

            var result = QueryOperate<Device>.Execute(queryData, query);
            var needExpandLocations = ExpandOperator.NeedExpand("Locations", query.expand);
            if (needExpandLocations)
                result = result.Include(d => d.Locations);

            //以下执行完后才会去数据库中查询
            var devices = await result.ToListAsync();

            var devicesSearch = devices.Select(v => new DeviceSearchModel
            {
                DeviceId = v.DeviceId,
                Voltage = v.Voltage,
                Diameter = v.Diameter,
                Torque = v.Torque,
                Locations = v.Locations.Select(p => new LocationSubSearchModel()
                {
                    LocationId = p.LocationId,
                    ProjectId = p.ProjectId,
                    Building = p.Building,
                    CommAddress = p.CommAddress,
                    CommMode = p.CommMode,
                    CurrentPosition = p.CurrentPosition,
                    Description = p.Description,
                    DeviceSerialNo = p.DeviceSerialNo,
                    DeviceId = p.DeviceId,
                    DeviceType = p.DeviceType,
                    FavorPositionFirst = p.FavorPositionFirst,
                    FavorPositionrSecond = p.FavorPositionrSecond,
                    FavorPositionThird = p.FavorPositionThird,
                    Floor = p.Floor,
                    InstallationNumber = p.InstallationNumber,
                    Orientation = p.Orientation,
                    RoomNo = p.RoomNo,
                    Creator = p.Creator,
                    CreateDate = p.CreateDate,
                    Modifier = p.Modifier,
                    ModifiedDate = p.ModifiedDate
                })
            });

            return devicesSearch;
        }

        public void RemoveById(int deviceId)
        {
            var device = _context.Devices.FirstOrDefault(l => l.DeviceId == deviceId);
            if (device == null)
                throw new ExpectException("Could not find data which DeviceId equal to " + deviceId);

            _context.Remove(device);
            _context.SaveChanges();
        }

        public async Task RemoveByIdAsync(int deviceId)
        {
            var device = _context.Devices.FirstOrDefault(l => l.DeviceId == deviceId);
            if (device == null)
                throw new ExpectException("Could not find data which DeviceId equal to " + deviceId);

            _context.Remove(device);
            await _context.SaveChangesAsync();
        }

        public int UpdateById(int deviceId, DeviceUpdateModel mDevice)
        {
            var device = _context.Devices.FirstOrDefault(l => l.DeviceId == deviceId);
            if (device == null)
                throw new ExpectException("Could not find data which DeviceId equal to " + deviceId);

            device.Voltage = mDevice.Voltage;
            device.Diameter = mDevice.Diameter;
            device.Torque = mDevice.Torque;

            _context.SaveChanges();
            return device.DeviceId;
        }

        public async Task<int> UpdateByIdAsync(int deviceId, DeviceUpdateModel mDevice)
        {
            var device = _context.Devices.FirstOrDefault(l => l.DeviceId == deviceId);
            if (device == null)
                throw new ExpectException("Could not find data which DeviceId equal to " + deviceId);

            device.Voltage = mDevice.Voltage;
            device.Diameter = mDevice.Diameter;
            device.Torque = mDevice.Torque;

            await _context.SaveChangesAsync();
            return device.DeviceId;
        }
    }
}
