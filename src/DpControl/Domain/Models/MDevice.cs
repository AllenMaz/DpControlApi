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

    public class DeviceSearchModel: DeviceBaseModel
    {
        public int DeviceId { get; set; }
        public IEnumerable<LocationSearchModel> Locations { get; set; }
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
    }
}
