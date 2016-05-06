using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Entities
{
    public class Utility
    {
    }

    /// <summary>
    /// 方向
    /// </summary>
    public enum Orientation
    {
        Null,       // for unidentified
        East,
        South,
        West,
        North,
        Southeast,
        Southwest,
        Northeast,
        Northwest
    }

    /// <summary>
    /// 设备类型
    /// </summary>
    public enum DeviceType
    {
        Null,
        Controller,
        TouchScreen,
        DryContact,
        RF,
        Tubular
    }

    /// <summary>
    /// 
    /// </summary>
    public enum CommMode
    {
        Null,
        RS485,
        ZigBee,
        Bluetooth,
        WiFi
    }

    /// <summary>
    /// 用户级别
    /// </summary>
    public enum UserLevel
    {
        SuperLevel,
        CustomerLevel,
        ProjectLevel
    }
}
