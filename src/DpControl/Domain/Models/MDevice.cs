using DpControl.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{
    public class DeviceBaseModel
    {
        public float Voltage { get; set; }
        public float Diameter { get; set; }
        public float Torque { get; set; }
        
    }

    public class DeviceAddModel: DeviceBaseModel
    {

    }

    public class DeviceUpdateModel: DeviceBaseModel
    {

    }

    public class DeviceSubSearchModel: DeviceBaseModel
    {
        public int DeviceId { get; set; }
    }

    public class DeviceSearchModel: DeviceSubSearchModel
    {
        public IEnumerable<LocationSubSearchModel> Locations { get; set; }
    }

    public static class DeviceOperator
    {
        /// <summary>
        /// Cascade set DeviceSearchModel Results
        /// </summary>
        public static IEnumerable<DeviceSearchModel> SetDeviceSearchModelCascade(List<Device> devices)
        {
            var deviceSearchModels = devices.Select(c => SetDeviceSearchModelCascade(c));

            return deviceSearchModels;
        }

        /// <summary>
        /// Cascade set DeviceSearchModel Result
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public static DeviceSearchModel SetDeviceSearchModelCascade(Device device)
        {
            if (device == null) return null;
            var deviceSearchModel = new DeviceSearchModel
            {
                DeviceId = device.DeviceId,
                Voltage = device.Voltage,
                Diameter = device.Diameter,
                Torque = device.Torque,
                Locations = LocationOperator.SetLocationSearchModelCascade(device.Locations)
            };

            return deviceSearchModel;
        }

        /// <summary>
        /// Cascade set DeviceSubSearchModel Results
        /// </summary>
        public static IEnumerable<DeviceSubSearchModel> SetDeviceSubSearchModel(List<Device> devices)
        {
            var deviceSearchModels = devices.Select(c => SetDeviceSubSearchModel(c));

            return deviceSearchModels;
        }

        /// <summary>
        /// Cascade set DeviceSubSearchModel Result
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public static DeviceSubSearchModel SetDeviceSubSearchModel(Device device)
        {
            if (device == null) return null;
            var deviceSearchModel = new DeviceSubSearchModel
            {
                DeviceId = device.DeviceId,
                Voltage = device.Voltage,
                Diameter = device.Diameter,
                Torque = device.Torque
            };

            return deviceSearchModel;
        }
    }
}
