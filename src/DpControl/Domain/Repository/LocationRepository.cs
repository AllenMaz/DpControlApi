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
    public class LocationRepository : ILocationRepository
    {
        ShadingContext _context;
        private readonly ILoginUserRepository _loginUser;

        #region
        public LocationRepository()
        {

        }
        public LocationRepository(ShadingContext context)
        {
            _context = context;
        }

        public LocationRepository(ShadingContext context, ILoginUserRepository loginUser)
        {
            _context = context;
            _loginUser = loginUser;
        }
        #endregion

        public int Add(LocationAddModel mLocation)
        {
            var customer = _context.Projects.FirstOrDefault(l => l.ProjectId == mLocation.ProjectId);
            if (customer == null)
                throw new ExpectException("Could not find Project data which ProjectId equal to " + mLocation.ProjectId);

            var device = _context.Devices.FirstOrDefault(l => l.DeviceId == mLocation.DeviceId);
            if (device == null)
                throw new ExpectException("Could not find Device data which DeviceId equal to " + mLocation.DeviceId);

            //InstallationNumber must be unique
            var checkData = _context.Locations.Where(dl => dl.InstallationNumber == mLocation.InstallationNumber).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which InstallationNumber equal to '" + mLocation.InstallationNumber + "' already exist in system");

            //DeviceSerialNo must be unique
            checkData = _context.Locations.Where(dl => dl.DeviceSerialNo == mLocation.DeviceSerialNo).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which DeviceSerialNo equal to '" + mLocation.DeviceSerialNo + "' already exist in system");


            //Get UserInfo
            var user = _loginUser.GetLoginUserInfo();

            var model = new Location
            {
                Building = mLocation.Building,
                CommAddress = mLocation.CommAddress,
                CommMode = mLocation.CommMode,
                CurrentPosition = mLocation.CurrentPosition,
                Description = mLocation.Description,
                DeviceSerialNo = mLocation.DeviceSerialNo,
                DeviceId = mLocation.DeviceId,
                DeviceType = mLocation.DeviceType,
                FavorPositionFirst = mLocation.FavorPositionFirst,
                FavorPositionrSecond = mLocation.FavorPositionrSecond,
                FavorPositionThird = mLocation.FavorPositionThird,
                Floor = mLocation.Floor,
                InstallationNumber = mLocation.InstallationNumber,
                Orientation = mLocation.Orientation,
                ProjectId = mLocation.ProjectId,
                RoomNo = mLocation.RoomNo,
                Creator = user.UserName,
                CreateDate = DateTime.Now
            };
            _context.Locations.Add(model);
            _context.SaveChangesAsync();
            return model.LocationId;
        }

        public async Task<int> AddAsync(LocationAddModel mLocation)
        {
            var customer = _context.Projects.FirstOrDefault(l=>l.ProjectId == mLocation.ProjectId);
            if (customer == null)
                throw new ExpectException("Could not find Project data which ProjectId equal to " + mLocation.ProjectId);

            if (mLocation.DeviceId != null)
            {
                var device = _context.Devices.FirstOrDefault(l => l.DeviceId == mLocation.DeviceId);
                if (device == null)
                    throw new ExpectException("Could not find Device data which DeviceId equal to " + mLocation.DeviceId);

            }
            //InstallationNumber must be unique
            var checkData = await _context.Locations.Where(dl=>dl.InstallationNumber == mLocation.InstallationNumber).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which InstallationNumber equal to '" + mLocation.InstallationNumber + "' already exist in system");

            //DeviceSerialNo must be unique
            checkData = await _context.Locations.Where(dl => dl.DeviceSerialNo == mLocation.DeviceSerialNo).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which DeviceSerialNo equal to '" + mLocation.DeviceSerialNo + "' already exist in system");
            
            //Get UserInfo
            var user = _loginUser.GetLoginUserInfo();

            var model = new Location
            {
                Building = mLocation.Building,
                CommAddress = mLocation.CommAddress,
                CommMode = mLocation.CommMode,
                CurrentPosition = mLocation.CurrentPosition,
                Description = mLocation.Description,
                DeviceSerialNo = mLocation.DeviceSerialNo,
                DeviceId = mLocation.DeviceId,
                DeviceType = mLocation.DeviceType,
                FavorPositionFirst = mLocation.FavorPositionFirst,
                FavorPositionrSecond = mLocation.FavorPositionrSecond,
                FavorPositionThird = mLocation.FavorPositionThird,
                Floor = mLocation.Floor,
                InstallationNumber = mLocation.InstallationNumber,
                Orientation =mLocation.Orientation,
                ProjectId = mLocation.ProjectId,
                RoomNo = mLocation.RoomNo,
                Creator = user.UserName,
                CreateDate = DateTime.Now
            };
            _context.Locations.Add(model);
            await _context.SaveChangesAsync();
            return model.LocationId;
        }

        public LocationSearchModel FindById(int locationId)
        {
            var result = _context.Locations.Where(v => v.LocationId == locationId);
            result = (IQueryable<Location>)ExpandOperator.ExpandRelatedEntities<Location>(result);

            var location = result.FirstOrDefault();
            var locationSearch = LocationOperator.SetLocationSearchModelCascade(location);

            return locationSearch;
        }

        public async Task<LocationSearchModel> FindByIdAsync(int locationId)
        {
            var result = _context.Locations.Where(v => v.LocationId == locationId);
            result = (IQueryable<Location>)ExpandOperator.ExpandRelatedEntities<Location>(result);

            var location = await result.FirstOrDefaultAsync();
            var locationSearch = LocationOperator.SetLocationSearchModelCascade(location);
            
            return locationSearch;
        }
        

        public IEnumerable<LocationSearchModel> GetAll()
        {
            var queryData = from L in _context.Locations
                            select L;

            var result = QueryOperate<Location>.Execute(queryData);
            result = (IQueryable<Location>)ExpandOperator.ExpandRelatedEntities<Location>(result);

            //以下执行完后才会去数据库中查询
            var locations =  result.ToList();
            var locationsSearch = LocationOperator.SetLocationSearchModelCascade(locations);


            return locationsSearch;
        }

        public async Task<IEnumerable<LocationSearchModel>> GetAllAsync()
        {
            var queryData = from L in _context.Locations
                            select L;

            var result = QueryOperate<Location>.Execute(queryData);
            result = (IQueryable<Location>)ExpandOperator.ExpandRelatedEntities<Location>(result);

            //以下执行完后才会去数据库中查询
            var locations = await result.ToListAsync();
            var locationsSearch = LocationOperator.SetLocationSearchModelCascade(locations);

            return locationsSearch;
        }

        #region Relations
        public async Task<ProjectSubSearchModel> GetProjectByLocationIdAsync(int locationId)
        {
            var location = await _context.Locations
                .Include(l => l.Project)
                .Where(l => l.LocationId == locationId).FirstOrDefaultAsync();
            var project = location == null ? null : location.Project;
            var projectSearch = ProjectOperator.SetProjectSubSearchModel(project);
            return projectSearch;
        }

        public async Task<IEnumerable<AlarmSubSearchModel>> GetAlarmsByLocationIdAsync(int locationId)
        {
            var queryData = _context.Alarms.Where(l=>l.LocationId == locationId);
            var result = QueryOperate<Alarm>.Execute(queryData);
            var alarms = await result.ToListAsync();
            var alarmsSearch = AlarmOperator.SetAlarmSubSearchModel(alarms);
            return alarmsSearch;
        }

        public async Task<DeviceSubSearchModel> GetDeviceByLocationIdAsync(int locationId)
        {
            var location = await _context.Locations
                .Include(l => l.Device)
                .Where(l => l.LocationId == locationId).FirstOrDefaultAsync();
            var device = location == null ? null : location.Device;
            var deviceSearch = DeviceOperator.SetDeviceSubSearchModel(device);
            return deviceSearch;
        }

        public async Task<IEnumerable<LogSubSearchModel>> GetLogsByLocationIdAsync(int locationId)
        {
            var queryData = _context.Logs.Where(l => l.LocationId == locationId);
            var result = QueryOperate<Log>.Execute(queryData);
            var logs = await result.ToListAsync();
            var logsSearch = LogOperator.SetLogSubSearchModel(logs);
            return logsSearch;
        }

        public async Task<IEnumerable<GroupSubSearchModel>> GetGroupsByLocationIdAsync(int locationId)
        {
            var queryData = _context.GroupLocations
                .Where(gl => gl.LocationId == locationId)
                .Select(gl => gl.Group);

            var result = QueryOperate<Group>.Execute(queryData);
            var groups = await result.ToListAsync();
            var groupsSearch = GroupOperator.SetGroupSubSearchModel(groups);
            return groupsSearch;
        }
        #endregion

        public void RemoveById(int locationId)
        {
            var location = _context.Locations.FirstOrDefault(l => l.LocationId == locationId);
            if (location == null)
                throw new ExpectException("Could not find data which DeviceLocation equal to " + locationId);

            _context.Remove(location);
             _context.SaveChanges();
        }

        public async Task RemoveByIdAsync(int locationId)
        {
            var location = _context.Locations.FirstOrDefault(l=>l.LocationId == locationId);
            if (location == null)
                throw new ExpectException("Could not find data which LocationId equal to " + locationId);

            _context.Remove(location);
            await _context.SaveChangesAsync();
        }

        public int UpdateById(int locationId, LocationUpdateModel mLocation)
        {
            var location = _context.Locations.FirstOrDefault(l => l.LocationId == locationId);
            if (location == null)
                throw new ExpectException("Could not find data which LocationId equal to " + locationId);

            var device = _context.Devices.FirstOrDefault(l => l.DeviceId == mLocation.DeviceId);
            if (device == null)
                throw new ExpectException("Could not find Device data which DeviceId equal to " + mLocation.DeviceId);


            //InstallationNumber must be unique
            var checkData = _context.Locations
                .Where(dl => dl.InstallationNumber == mLocation.InstallationNumber
                && dl.LocationId != locationId).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which InstallationNumber equal to '" + mLocation.InstallationNumber + "' already exist in system");

            //DeviceSerialNo must be unique
            checkData = _context.Locations
                .Where(dl => dl.DeviceSerialNo == mLocation.DeviceSerialNo
                && dl.LocationId != locationId).ToList();
            if (checkData.Count > 0)
                throw new ExpectException("The data which DeviceSerialNo equal to '" + mLocation.DeviceSerialNo + "' already exist in system");

            
            //Get UserInfo
            var user = _loginUser.GetLoginUserInfo();

            location.Building = mLocation.Building;
            location.CommAddress = mLocation.CommAddress;
            location.CommMode = mLocation.CommMode;
            location.CurrentPosition = mLocation.CurrentPosition;
            location.Description = mLocation.Description;
            location.DeviceSerialNo = mLocation.DeviceSerialNo;
            location.DeviceId = mLocation.DeviceId;
            location.DeviceType = mLocation.DeviceType;
            location.FavorPositionFirst = mLocation.FavorPositionFirst;
            location.FavorPositionrSecond = mLocation.FavorPositionrSecond;
            location.FavorPositionThird = mLocation.FavorPositionThird;
            location.Floor = mLocation.Floor;
            location.InstallationNumber = mLocation.InstallationNumber;
            location.Orientation = mLocation.Orientation;
            location.RoomNo = mLocation.RoomNo;
            location.Modifier = user.UserName;
            location.ModifiedDate = DateTime.Now;

            _context.SaveChanges();
            return location.LocationId;
        }

        public async Task<int> UpdateByIdAsync(int locationId, LocationUpdateModel mLocation)
        {
            var location = _context.Locations.FirstOrDefault(l => l.LocationId == locationId);
            if (location == null)
                throw new ExpectException("Could not find data which DeviceLocation equal to " + locationId);

            var device = _context.Devices.FirstOrDefault(l => l.DeviceId == mLocation.DeviceId);
            if (device == null)
                throw new ExpectException("Could not find Device data which DeviceId equal to " + mLocation.DeviceId);


            //InstallationNumber must be unique
            var checkData = await _context.Locations
                .Where(dl => dl.InstallationNumber == mLocation.InstallationNumber
                && dl.LocationId != locationId).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which InstallationNumber equal to '" + mLocation.InstallationNumber + "' already exist in system");

            //DeviceSerialNo must be unique
            checkData = await _context.Locations
                .Where(dl => dl.DeviceSerialNo == mLocation.DeviceSerialNo
                && dl.LocationId != locationId).ToListAsync();
            if (checkData.Count > 0)
                throw new ExpectException("The data which DeviceSerialNo equal to '" + mLocation.DeviceSerialNo + "' already exist in system");

           

            //Get UserInfo
            var user = _loginUser.GetLoginUserInfo();

            location.Building = mLocation.Building;
            location.CommAddress = mLocation.CommAddress;
            location.CommMode = mLocation.CommMode;
            location.CurrentPosition = mLocation.CurrentPosition;
            location.Description = mLocation.Description;
            location.DeviceSerialNo = mLocation.DeviceSerialNo;
            location.DeviceId = mLocation.DeviceId;
            location.DeviceType = mLocation.DeviceType;
            location.FavorPositionFirst = mLocation.FavorPositionFirst;
            location.FavorPositionrSecond = mLocation.FavorPositionrSecond;
            location.FavorPositionThird = mLocation.FavorPositionThird;
            location.Floor = mLocation.Floor;
            location.InstallationNumber = mLocation.InstallationNumber;
            location.Orientation = mLocation.Orientation;
            location.RoomNo = mLocation.RoomNo;
            location.Modifier = user.UserName;
            location.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return location.LocationId;
        }

        public async Task CreateRelationsAsync(int locationId, string navigationProperty, List<string> navigationPropertyIds)
        {
            var location = await _context.Locations.FirstOrDefaultAsync(u => u.LocationId == locationId);
            if (location == null)
                throw new ExpectException("Could not find data which LocationId equal to " + locationId);

            switch (navigationProperty)
            {
                case "Users":

                    foreach (string userId in navigationPropertyIds)
                    {
                        //is navigationProperty already exist in system
                        var user = _context.Users.FirstOrDefault(u => u.Id == userId);
                        if (user == null)
                            throw new ExpectException("User data which UserId equal to " + userId + " not exist in system");
                        //is relationship already exist in system
                        var userlocation = _context.UserLocations
                            .Where(ul => ul.LocationId == locationId && ul.UserId == userId).ToList();
                        if (userlocation.Count > 0)
                            throw new ExpectException("Relation:" + userId + " already exist in system");
                        //add relations
                        var relation = new UserLocation() { UserId = userId, LocationId = locationId };
                        _context.UserLocations.Add(relation);
                    }


                    break;
                case "Groups":

                    foreach (string navigationId in navigationPropertyIds)
                    {
                        //conver navigationId to int
                        int groupId = Utilities.ConverRelationIdToInt(navigationId);
                        //is navigationProperty already exist in system
                        var group = _context.Groups.FirstOrDefault(r => r.GroupId == groupId);
                        if (group == null)
                            throw new ExpectException("Group data which GroupId equal to " + navigationId + " not exist in system");
                        //is relationship already exist in system
                        var grouplocation = _context.GroupLocations
                            .Where(ul => ul.LocationId == locationId && ul.GroupId == groupId).ToList();
                        if (grouplocation.Count > 0)
                            throw new ExpectException("Relation:" + navigationId + " already exist in system");
                        //add relations
                        var relation = new GroupLocation() { GroupId = groupId, LocationId = locationId };
                        _context.GroupLocations.Add(relation);
                    }

                    break;
                default:
                    throw new ExpectException("No relation:" + navigationProperty);
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveRelationsAsync(int locationId, string navigationProperty, List<string> navigationPropertyIds)
        {
            var location = await _context.Locations.FirstOrDefaultAsync(u => u.LocationId == locationId);
            if (location == null)
                throw new ExpectException("Could not find data which LocationId equal to " + locationId);

            switch (navigationProperty)
            {
                case "Users":

                    foreach (string userId in navigationPropertyIds)
                    {//is relationship already exist in system
                        var userlocation = _context.UserLocations
                            .Where(ul => ul.LocationId == locationId && ul.UserId == userId).FirstOrDefault();
                        if (userlocation == null)
                            throw new ExpectException("Relation:" + userId + " not exist in system");
                        //remove relations
                        _context.UserLocations.Remove(userlocation);
                    }


                    break;
                case "Groups":

                    foreach (string navigationId in navigationPropertyIds)
                    {
                        //conver navigationId to int
                        int groupId = Utilities.ConverRelationIdToInt(navigationId);
                       //is relationship already exist in system
                        var grouplocation = _context.GroupLocations
                            .Where(ul => ul.LocationId == locationId && ul.GroupId == groupId).FirstOrDefault();
                        if (grouplocation == null)
                            throw new ExpectException("Relation:" + navigationId + " not exist in system");
                        //remove relations
                        _context.GroupLocations.Remove(grouplocation);
                    }

                    break;
                default:
                    throw new ExpectException("No relation:" + navigationProperty);
            }

            await _context.SaveChangesAsync();
        }
    }
}
