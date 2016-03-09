using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using DpControl.Domain.Entities;
using DpControl.Domain.Models;
using DpControl.Domain.IRepository;
using DpControl.Domain.EFContext;
using DpControl.Domain.Execptions;

namespace DpControl.Domain.Repository
{
    public class DeviceLocationRepository : IDeviceLocationRepository
    {
        ShadingContext _context;
        private readonly IUserInfoRepository _userInfo;

        #region
        public DeviceLocationRepository()
        {

        }
        public DeviceLocationRepository(ShadingContext context)
        {
            _context = context;
        }

        public DeviceLocationRepository(ShadingContext context, IUserInfoRepository userInfo)
        {
            _context = context;
            _userInfo = userInfo;
        }
        #endregion

        public int Add(DeviceLocationAddModel mDeviceLocation)
        {
            var customer = _context.Projects.FirstOrDefault(l => l.ProjectId == mDeviceLocation.ProjectId);
            if (customer == null)
                throw new ExpectException("Could not find Project data which ProjectId equal to " + mDeviceLocation.ProjectId);

            //InstallationNumber must be unique
            var checkData = _context.DeviceLocations.Where(dl => dl.InstallationNumber == mDeviceLocation.InstallationNumber).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which InstallationNumber equal to '" + mDeviceLocation.InstallationNumber + "' already exist in system");

            //DeviceSerialNo must be unique
            checkData = _context.DeviceLocations.Where(dl => dl.DeviceSerialNo == mDeviceLocation.DeviceSerialNo).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which DeviceSerialNo equal to '" + mDeviceLocation.DeviceSerialNo + "' already exist in system");

            //Check Orientation
            if (!Enum.IsDefined(typeof(Orientation), mDeviceLocation.Orientation))
            {
                throw new ExpectException("Invalid Orientation.You can get correct Orientation values from API");
            }

            //Check DeviceType
            if (!Enum.IsDefined(typeof(DeviceType), mDeviceLocation.DeviceType))
            {
                throw new ExpectException("Invalid DeviceType.You can get correct DeviceType values from API");
            }

            //Check CommMode
            if (!Enum.IsDefined(typeof(CommMode), mDeviceLocation.CommMode))
            {
                throw new ExpectException("Invalid CommMode.You can get correct CommMode values from API");
            }

            //Get UserInfo
            var user = _userInfo.GetUserInfo();

            var model = new DeviceLocation
            {
                Building = mDeviceLocation.Building,
                CommAddress = mDeviceLocation.CommAddress,
                CommMode = mDeviceLocation.CommMode,
                CurrentPosition = mDeviceLocation.CurrentPosition,
                Description = mDeviceLocation.Description,
                DeviceSerialNo = mDeviceLocation.DeviceSerialNo,
                DeviceType = mDeviceLocation.DeviceType,
                FavorPositionFirst = mDeviceLocation.FavorPositionFirst,
                FavorPositionrSecond = mDeviceLocation.FavorPositionrSecond,
                FavorPositionThird = mDeviceLocation.FavorPositionThird,
                Floor = mDeviceLocation.Floor,
                InstallationNumber = mDeviceLocation.InstallationNumber,
                Orientation = mDeviceLocation.Orientation,
                ProjectId = mDeviceLocation.ProjectId,
                RoomNo = mDeviceLocation.RoomNo,
                Creator = user.UserName,
                CreateDate = DateTime.Now
            };
            _context.DeviceLocations.Add(model);
            _context.SaveChangesAsync();
            return model.DeviceLocationId;
        }

        public async Task<int> AddAsync(DeviceLocationAddModel mDeviceLocation)
        {
            var customer = _context.Projects.FirstOrDefault(l=>l.ProjectId == mDeviceLocation.ProjectId);
            if (customer == null)
                throw new ExpectException("Could not find Project data which ProjectId equal to " + mDeviceLocation.ProjectId);

            //InstallationNumber must be unique
            var checkData = await _context.DeviceLocations.Where(dl=>dl.InstallationNumber == mDeviceLocation.InstallationNumber).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which InstallationNumber equal to '" + mDeviceLocation.InstallationNumber + "' already exist in system");

            //DeviceSerialNo must be unique
            checkData = await _context.DeviceLocations.Where(dl => dl.DeviceSerialNo == mDeviceLocation.DeviceSerialNo).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which DeviceSerialNo equal to '" + mDeviceLocation.DeviceSerialNo + "' already exist in system");

            //Check Orientation
            if (!Enum.IsDefined(typeof(Orientation),mDeviceLocation.Orientation))
            {
                throw new ExpectException("Invalid Orientation.You can get correct Orientation values from API");
            }

            //Check DeviceType
            if (!Enum.IsDefined(typeof(DeviceType), mDeviceLocation.DeviceType))
            {
                throw new ExpectException("Invalid DeviceType.You can get correct DeviceType values from API");
            }

            //Check CommMode
            if (!Enum.IsDefined(typeof(CommMode), mDeviceLocation.CommMode))
            {
                throw new ExpectException("Invalid CommMode.You can get correct CommMode values from API");
            }

            //Get UserInfo
            var user = await _userInfo.GetUserInfoAsync();

            var aa = (CommMode)Enum.ToObject(typeof(CommMode), mDeviceLocation.CommMode);


            var model = new DeviceLocation
            {
                Building = mDeviceLocation.Building,
                CommAddress = mDeviceLocation.CommAddress,
                CommMode = mDeviceLocation.CommMode,
                CurrentPosition = mDeviceLocation.CurrentPosition,
                Description = mDeviceLocation.Description,
                DeviceSerialNo = mDeviceLocation.DeviceSerialNo,
                DeviceType = mDeviceLocation.DeviceType,
                FavorPositionFirst = mDeviceLocation.FavorPositionFirst,
                FavorPositionrSecond = mDeviceLocation.FavorPositionrSecond,
                FavorPositionThird = mDeviceLocation.FavorPositionThird,
                Floor = mDeviceLocation.Floor,
                InstallationNumber = mDeviceLocation.InstallationNumber,
                Orientation =mDeviceLocation.Orientation,
                ProjectId = mDeviceLocation.ProjectId,
                RoomNo = mDeviceLocation.RoomNo,
                Creator = user.UserName,
                CreateDate = DateTime.Now
            };
            _context.DeviceLocations.Add(model);
            await _context.SaveChangesAsync();
            return model.DeviceLocationId;
        }

        public DeviceLocationSearchModel FindById(int deviceLocationId)
        {
            var deviceLocation = _context.DeviceLocations.Where(v => v.DeviceLocationId == deviceLocationId)
                .Select(v => new DeviceLocationSearchModel()
                {
                    DeviceLocationId = v.DeviceLocationId,
                    ProjectId = v.ProjectId,
                    Building = v.Building,
                    CommAddress = v.CommAddress,
                    CommMode = v.CommMode,
                    CurrentPosition = v.CurrentPosition,
                    Description = v.Description,
                    DeviceSerialNo = v.DeviceSerialNo,
                    DeviceType = v.DeviceType,
                    FavorPositionFirst = v.FavorPositionFirst,
                    FavorPositionrSecond = v.FavorPositionrSecond,
                    FavorPositionThird = v.FavorPositionThird,
                    Floor = v.Floor,
                    InstallationNumber = v.InstallationNumber,
                    Orientation = v.Orientation,
                    RoomNo = v.RoomNo,
                    Creator = v.Creator,
                    CreateDate = v.CreateDate,
                    Modifier = v.Modifier,
                    ModifiedDate = v.ModifiedDate
                }).FirstOrDefault();

            return deviceLocation;
        }

        public async Task<DeviceLocationSearchModel> FindByIdAsync(int deviceLocationId)
        {
            var deviceLocation = await _context.DeviceLocations.Where(v => v.DeviceLocationId == deviceLocationId)
                .Select(v => new DeviceLocationSearchModel()
                {
                    DeviceLocationId = v.DeviceLocationId,
                    ProjectId = v.ProjectId,
                    Building = v.Building,
                    CommAddress = v.CommAddress,
                    CommMode = v.CommMode,
                    CurrentPosition = v.CurrentPosition,
                    Description = v.Description,
                    DeviceSerialNo = v.DeviceSerialNo,
                    DeviceType = v.DeviceType,
                    FavorPositionFirst = v.FavorPositionFirst,
                    FavorPositionrSecond = v.FavorPositionrSecond,
                    FavorPositionThird = v.FavorPositionThird,
                    Floor = v.Floor,
                    InstallationNumber = v.InstallationNumber,
                    Orientation = v.Orientation,
                    RoomNo = v.RoomNo,
                    Creator = v.Creator,
                    CreateDate = v.CreateDate,
                    Modifier = v.Modifier,
                    ModifiedDate = v.ModifiedDate
                }).FirstOrDefaultAsync();

            return deviceLocation;
        }

        public IEnumerable<DeviceLocationSearchModel> GetAll(Query query)
        {
            var queryData = from L in _context.DeviceLocations
                            select L;

            var result = QueryOperate<DeviceLocation>.Execute(queryData, query);

            //以下执行完后才会去数据库中查询
            var deviceLocations =  result.ToList();

            var deviceLocationsSearch = deviceLocations.Select(v => new DeviceLocationSearchModel
            {
                DeviceLocationId = v.DeviceLocationId,
                ProjectId = v.ProjectId,
                Building = v.Building,
                CommAddress = v.CommAddress,
                CommMode = v.CommMode,
                CurrentPosition = v.CurrentPosition,
                Description = v.Description,
                DeviceSerialNo = v.DeviceSerialNo,
                DeviceType = v.DeviceType,
                FavorPositionFirst = v.FavorPositionFirst,
                FavorPositionrSecond = v.FavorPositionrSecond,
                FavorPositionThird = v.FavorPositionThird,
                Floor = v.Floor,
                InstallationNumber = v.InstallationNumber,
                Orientation = v.Orientation,
                RoomNo = v.RoomNo,
                Creator = v.Creator,
                CreateDate = v.CreateDate,
                Modifier = v.Modifier,
                ModifiedDate = v.ModifiedDate
            });

            return deviceLocationsSearch;
        }

        public async Task<IEnumerable<DeviceLocationSearchModel>> GetAllAsync(Query query)
        {
            var queryData = from L in _context.DeviceLocations
                            select L;

            var result = QueryOperate<DeviceLocation>.Execute(queryData, query);

            //以下执行完后才会去数据库中查询
            var deviceLocations = await result.ToListAsync();

            var deviceLocationsSearch = deviceLocations.Select(v => new DeviceLocationSearchModel
            {
                DeviceLocationId = v.DeviceLocationId,
                ProjectId = v.ProjectId,
                Building = v.Building,
                CommAddress = v.CommAddress,
                CommMode = v.CommMode,
                CurrentPosition = v.CurrentPosition,
                Description = v.Description,
                DeviceSerialNo = v.DeviceSerialNo,
                DeviceType = v.DeviceType,
                FavorPositionFirst = v.FavorPositionFirst,
                FavorPositionrSecond = v.FavorPositionrSecond,
                FavorPositionThird = v.FavorPositionThird,
                Floor = v.Floor,
                InstallationNumber = v.InstallationNumber,
                Orientation = v.Orientation,
                RoomNo = v.RoomNo,
                Creator = v.Creator,
                CreateDate = v.CreateDate,
                Modifier = v.Modifier,
                ModifiedDate = v.ModifiedDate
            });

            return deviceLocationsSearch;
        }

        public void RemoveById(int deviceLocationId)
        {
            var deviceLocation = _context.DeviceLocations.FirstOrDefault(l => l.DeviceLocationId == deviceLocationId);
            if (deviceLocation == null)
                throw new ExpectException("Could not find data which DeviceLocation equal to " + deviceLocationId);

            _context.Remove(deviceLocation);
             _context.SaveChanges();
        }

        public async Task RemoveByIdAsync(int deviceLocationId)
        {
            var deviceLocation = _context.DeviceLocations.FirstOrDefault(l=>l.DeviceLocationId == deviceLocationId);
            if (deviceLocation == null)
                throw new ExpectException("Could not find data which DeviceLocation equal to " + deviceLocationId);

            _context.Remove(deviceLocation);
            await _context.SaveChangesAsync();
        }

        public int UpdateById(int deviceLocationId, DeviceLocationUpdateModel mDeviceLocation)
        {
            var deviceLocation = _context.DeviceLocations.FirstOrDefault(l => l.DeviceLocationId == deviceLocationId);
            if (deviceLocation == null)
                throw new ExpectException("Could not find data which DeviceLocation equal to " + deviceLocationId);

            //InstallationNumber must be unique
            var checkData = _context.DeviceLocations
                .Where(dl => dl.InstallationNumber == mDeviceLocation.InstallationNumber
                && dl.DeviceLocationId != deviceLocationId).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which InstallationNumber equal to '" + mDeviceLocation.InstallationNumber + "' already exist in system");

            //DeviceSerialNo must be unique
            checkData = _context.DeviceLocations
                .Where(dl => dl.DeviceSerialNo == mDeviceLocation.DeviceSerialNo
                && dl.DeviceLocationId != deviceLocationId).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which DeviceSerialNo equal to '" + mDeviceLocation.DeviceSerialNo + "' already exist in system");

            //Check Orientation
            if (!Enum.IsDefined(typeof(Orientation), mDeviceLocation.Orientation))
            {
                throw new ExpectException("Invalid Orientation.You can get correct Orientation values from API");
            }

            //Check DeviceType
            if (!Enum.IsDefined(typeof(DeviceType), mDeviceLocation.DeviceType))
            {
                throw new ExpectException("Invalid DeviceType.You can get correct DeviceType values from API");
            }

            //Check CommMode
            if (!Enum.IsDefined(typeof(CommMode), mDeviceLocation.CommMode))
            {
                throw new ExpectException("Invalid CommMode.You can get correct CommMode values from API");
            }

            //Get UserInfo
            var user = _userInfo.GetUserInfo();

            deviceLocation.Building = mDeviceLocation.Building;
            deviceLocation.CommAddress = mDeviceLocation.CommAddress;
            deviceLocation.CommMode = mDeviceLocation.CommMode;
            deviceLocation.CurrentPosition = mDeviceLocation.CurrentPosition;
            deviceLocation.Description = mDeviceLocation.Description;
            deviceLocation.DeviceSerialNo = mDeviceLocation.DeviceSerialNo;
            deviceLocation.DeviceType = mDeviceLocation.DeviceType;
            deviceLocation.FavorPositionFirst = mDeviceLocation.FavorPositionFirst;
            deviceLocation.FavorPositionrSecond = mDeviceLocation.FavorPositionrSecond;
            deviceLocation.FavorPositionThird = mDeviceLocation.FavorPositionThird;
            deviceLocation.Floor = mDeviceLocation.Floor;
            deviceLocation.InstallationNumber = mDeviceLocation.InstallationNumber;
            deviceLocation.Orientation = mDeviceLocation.Orientation;
            deviceLocation.RoomNo = mDeviceLocation.RoomNo;
            deviceLocation.Modifier = user.UserName;
            deviceLocation.ModifiedDate = DateTime.Now;

            _context.SaveChanges();
            return deviceLocation.DeviceLocationId;
        }

        public async Task<int> UpdateByIdAsync(int deviceLocationId, DeviceLocationUpdateModel mDeviceLocation)
        {
            var deviceLocation = _context.DeviceLocations.FirstOrDefault(l => l.DeviceLocationId == deviceLocationId);
            if (deviceLocation == null)
                throw new ExpectException("Could not find data which DeviceLocation equal to " + deviceLocationId);

            //InstallationNumber must be unique
            var checkData = await _context.DeviceLocations
                .Where(dl => dl.InstallationNumber == mDeviceLocation.InstallationNumber
                && dl.DeviceLocationId != deviceLocationId).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which InstallationNumber equal to '" + mDeviceLocation.InstallationNumber + "' already exist in system");

            //DeviceSerialNo must be unique
            checkData = await _context.DeviceLocations
                .Where(dl => dl.DeviceSerialNo == mDeviceLocation.DeviceSerialNo
                && dl.DeviceLocationId != deviceLocationId).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which DeviceSerialNo equal to '" + mDeviceLocation.DeviceSerialNo + "' already exist in system");

            //Check Orientation
            if (!Enum.IsDefined(typeof(Orientation), mDeviceLocation.Orientation))
            {
                throw new ExpectException("Invalid Orientation.You can get correct Orientation values from API");
            }

            //Check DeviceType
            if (!Enum.IsDefined(typeof(DeviceType), mDeviceLocation.DeviceType))
            {
                throw new ExpectException("Invalid DeviceType.You can get correct DeviceType values from API");
            }

            //Check CommMode
            if (!Enum.IsDefined(typeof(CommMode), mDeviceLocation.CommMode))
            {
                throw new ExpectException("Invalid CommMode.You can get correct CommMode values from API");
            }

            //Get UserInfo
            var user = await _userInfo.GetUserInfoAsync();

            deviceLocation.Building = mDeviceLocation.Building;
            deviceLocation.CommAddress = mDeviceLocation.CommAddress;
            deviceLocation.CommMode = mDeviceLocation.CommMode;
            deviceLocation.CurrentPosition = mDeviceLocation.CurrentPosition;
            deviceLocation.Description = mDeviceLocation.Description;
            deviceLocation.DeviceSerialNo = mDeviceLocation.DeviceSerialNo;
            deviceLocation.DeviceType = mDeviceLocation.DeviceType;
            deviceLocation.FavorPositionFirst = mDeviceLocation.FavorPositionFirst;
            deviceLocation.FavorPositionrSecond = mDeviceLocation.FavorPositionrSecond;
            deviceLocation.FavorPositionThird = mDeviceLocation.FavorPositionThird;
            deviceLocation.Floor = mDeviceLocation.Floor;
            deviceLocation.InstallationNumber = mDeviceLocation.InstallationNumber;
            deviceLocation.Orientation = mDeviceLocation.Orientation;
            deviceLocation.RoomNo = mDeviceLocation.RoomNo;
            deviceLocation.Modifier = user.UserName;
            deviceLocation.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return deviceLocation.DeviceLocationId;
        }
    }
}
