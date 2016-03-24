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
            var result = _context.Devices.Where(v => v.DeviceId == deviceId);
            result = (IQueryable<Device>)ExpandOperator.ExpandRelatedEntities<Device>(result);

            var device = result.FirstOrDefault();
            var deviceSearch = DeviceOperator.SetDeviceSearchModelCascade(device);
            return deviceSearch;
        }

        public async Task<DeviceSearchModel> FindByIdAsync(int deviceId)
        {
            var result = _context.Devices.Where(v => v.DeviceId == deviceId);
            result = (IQueryable<Device>)ExpandOperator.ExpandRelatedEntities<Device>(result);

            var device = await result.FirstOrDefaultAsync();
            var deviceSearch = DeviceOperator.SetDeviceSearchModelCascade(device);
            return deviceSearch;
        }

        public IEnumerable<DeviceSearchModel> GetAll()
        {
            var queryData = from D in _context.Devices
                            select D;

            var result = QueryOperate<Device>.Execute(queryData);
            result = (IQueryable<Device>)ExpandOperator.ExpandRelatedEntities<Device>(result);

            //以下执行完后才会去数据库中查询
            var devices = result.ToList();
            var devicesSearch = DeviceOperator.SetDeviceSearchModelCascade(devices);

            return devicesSearch;
        }

        public async Task<IEnumerable<DeviceSearchModel>> GetAllAsync()
        {
            var queryData = from D in _context.Devices
                            select D;

            var result = QueryOperate<Device>.Execute(queryData);
            result = (IQueryable<Device>)ExpandOperator.ExpandRelatedEntities<Device>(result);

            //以下执行完后才会去数据库中查询
            var devices = await result.ToListAsync();
            var devicesSearch = DeviceOperator.SetDeviceSearchModelCascade(devices);
            return devicesSearch;
        }

        public async Task<IEnumerable<LocationSubSearchModel>> GetLocationsByDeviceIdAsync(int deviceId)
        {
            var queryData = _context.Locations.Where(l=>l.DeviceId == deviceId);
            var result = QueryOperate<Location>.Execute(queryData);
            var locations = await result.ToListAsync();
            var locationsSearch = LocationOperator.SetLocationSubSearchModel(locations);
            return locationsSearch;
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
